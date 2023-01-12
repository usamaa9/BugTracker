using PurpleRock.BugTracker.Infrastructure;

namespace PurpleRock.BugTracker.WebApi;

/// <summary>
/// Startup class configures services and the app's request pipeline.
/// </summary>
public class Startup : CommonStartup
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Startup"/> class.
    /// </summary>
    /// <param name="configuration"></param>
    public Startup(IConfiguration configuration)
        : base(configuration)
    {
    }

    /// <inheritdoc />
    protected override void ConfigureServicesHook(IServiceCollection services, AppOptions appOptions)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder => { builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
        });
    }

    protected override void ConfigureCorsHook(IApplicationBuilder app)
    {
        app.UseCors();
    }


    protected override void ConfigureRepositoriesHook(IServiceCollection services, IConfiguration configuration)
    {
        base.ConfigureRepositoriesHook(services, configuration);
        services.RegisterRepositories(configuration);
    }
}
