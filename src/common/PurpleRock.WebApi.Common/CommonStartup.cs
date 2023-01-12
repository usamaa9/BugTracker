using System.Text.Json.Serialization;
using Hellang.Middleware.ProblemDetails;
using Joonasw.AspNetCore.SecurityHeaders;
using Joonasw.AspNetCore.SecurityHeaders.XContentTypeOptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using PurpleRock.Application.Common.Abstractions;
using PurpleRock.Application.Common.Configuration;
using PurpleRock.Application.Common.Extensions;
using PurpleRock.WebApi.Common.Converters;
using PurpleRock.WebApi.Common.Extensions;
using PurpleRock.WebApi.Common.Filters;
using PurpleRock.WebApi.Common.Health;
using PurpleRock.WebApi.Common.Mediator;
using RobotsTxt;

namespace PurpleRock.WebApi.Common;

/// <summary>
/// Initialisation logic which is common to all WebApi solutions.
/// </summary>
[ExcludeFromCodeCoverage]
public abstract class CommonStartup
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CommonStartup"/> class.
    /// </summary>
    /// <param name="configuration"></param>
    protected CommonStartup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    /// <summary>
    /// App configuration.
    /// </summary>
    protected IConfiguration Configuration { get; }

    /// <summary>
    /// This method gets called by the runtime.
    /// Adds common services to the container.
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        RegisterAbstractions(services);

        // Depends on AddAbstractionsDefaultImplementations
        var entryAssembly = GetEntryAssembly(services, Configuration);
        var purpleRockAssemblies = entryAssembly.GetPurpleRockAssemblies();

        services.AddStaticRobotsTxt(builder => builder.DenyAll());
        services.AddHttpsRedirection(options => options.HttpsPort = 443);

        services.RegisterOptions(Configuration);

        var sp = services.BuildServiceProvider();
        var appOptions = sp.GetRequiredService<IOptions<AppOptions>>().Value;

        services
            .AddControllers(options => options.Filters.Add(typeof(RequestResponseLoggingActionFilterAttribute)))
            .AddApplicationPart(entryAssembly)
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.Converters.Add(new JsonStringTrimConverter());
            });

        services.AddProblemDetails(opts =>
        {
            // Include exception details in problem details response
            opts.IncludeExceptionDetails = (_, _) => true;
        });

        // Extra logging for invalid model reporting
        services.ConfigureInvalidModelStateMiddleware();

        // Add http context
        services.AddHttpContextAccessor();

        // Health checks
        services.AddHealthChecks();

        // CSP
        services.AddCsp();

        services.ConfigureSwagger(appOptions, purpleRockAssemblies);

        services
            .AddCommandFramework<CommandBus>(purpleRockAssemblies)
            .AddApplicationMappers(purpleRockAssemblies);

        ConfigureRepositoriesHook(services, Configuration);
        ConfigureClientsHook(services, appOptions);
        ConfigureServicesHook(services, appOptions);
    }

    /// <summary>
    /// This method gets called by the runtime.
    /// Configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    /// <param name="serviceSettingsOptions"> <see cref="ServiceSettingsOptions"/>.</param>
    /// <param name="appOptions"> <see cref="AppOptions"/>.</param>
    public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env,
        IOptions<ServiceSettingsOptions> serviceSettingsOptions,
        IOptions<AppOptions> appOptions)
    {
        if (env.IsDevelopment())
        {
            IdentityModelEventSource.ShowPII = true;
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/error");
            app.UseHsts();
        }

        if (serviceSettingsOptions.Value.ServiceType != "Web")
        {
            // Swagger UI
            app.UseSwagger(options =>
            {
                options.PreSerializeFilters.Add((swagger, req) =>
                {
                    swagger.Servers = new List<OpenApiServer>
                    {
                        new() { Url = $"https://{req.Host}" }
                    };
                });
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{appOptions.Value.ApplicationDisplayName} v1");

                c.RoutePrefix = string.Empty;
            });
        }

        app.UseRouting();

        app.UseProblemDetails();

        ConfigureCorsHook(app);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = HealthCheckResponseWriter.WriteResponseAsJsonAsync
            });
        });

        // Call out to the consuming hook.
        ConfigureHook(app, env, appOptions);

        // Apply Content security policy after the hook
        // Due to swagger having inline scripts, having csp middleware after swagger is a workaround
        // https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/505
        ApplyContentSecurityPolicies(app);
    }

    /// <summary>
    /// Hook for adding CORs.
    /// </summary>
    /// <param name="app"></param>
    protected virtual void ConfigureCorsHook(IApplicationBuilder app)
    {
    }

    /// <summary>
    /// Register all abstractions.
    /// </summary>
    /// <param name="services"></param>
    protected virtual void RegisterAbstractions(IServiceCollection services)
    {
        services.AddAbstractionsDefaultImplementations();
    }

    /// <summary>
    /// Hook that executes at the end of ConfigureServices <see cref="ConfigureServices"/>.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="appOptions"></param>
    protected abstract void ConfigureServicesHook(IServiceCollection services, AppOptions appOptions);

    /// <summary>
    /// Hook that executes at the end of ConfigureServices <see cref="ConfigureServices"/>
    /// to register clients and easily replace them in integration tests.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="appOptions"></param>
    protected virtual void ConfigureClientsHook(
        IServiceCollection services,
        AppOptions appOptions)
    {
    }

    /// <summary>
    /// Hook that executes at the end of ConfigureServices <see cref="ConfigureServices"/>
    /// to register repositories and easily replace them in integration tests.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    protected virtual void ConfigureRepositoriesHook(
        IServiceCollection services, IConfiguration configuration)
    {
    }

    /// <summary>
    /// Hook that executes at the end of Configure <see cref="Configure"/>.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    /// <param name="appOptions"></param>
    protected virtual void ConfigureHook(IApplicationBuilder app, IWebHostEnvironment env,
        IOptions<AppOptions> appOptions)
    {
    }

    private static void ApplyContentSecurityPolicies(IApplicationBuilder app)
    {
        app.UseRobotsTxt();

        // Default to nosniff
        app.UseMiddleware<XContentTypeOptionsMiddleware>();

        app.UseCsp(csp =>
        {
            csp.AllowScripts
                .FromSelf();

            csp.AllowStyles
                .FromSelf();

            csp.AllowFonts
                .FromSelf();

            csp.AllowImages
                .FromSelf();

            csp.AllowFrames
                .FromNowhere();
        });
    }

    private static Assembly GetEntryAssembly(IServiceCollection services, IConfiguration configuration)
    {
        Assembly entryAssembly;
        if (configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT") == "CI")
        {
            entryAssembly = Assembly.GetExecutingAssembly();
        }
        else
        {
            var sp = services.BuildServiceProvider();
            var assemblyProvider = sp.GetService<IAssemblyProvider>()
                                   ?? throw new InvalidOperationException(
                                       $"Could not resolve {nameof(IAssemblyProvider)}! Have you forgot to register it?");

            entryAssembly = assemblyProvider.GetEntryAssembly();
        }

        // Make it available for anything that cannot get it injected
        StaticAssemblyProvider.GetEntryAssemblyFunc = () => entryAssembly;

        return entryAssembly;
    }
}
