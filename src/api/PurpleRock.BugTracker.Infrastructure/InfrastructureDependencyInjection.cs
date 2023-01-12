using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PurpleRock.BugTracker.Infrastructure.Persistence;

namespace PurpleRock.BugTracker.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static IServiceCollection RegisterRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCosmosRepository();

        services.AddTransient<IWriteBugRepository, WriteBugRepository>();
        services.AddTransient<IReadBugRepository, ReadBugRepository>();
        services.AddTransient<IReadPersonRepository, ReadPersonRepository>();
        services.AddTransient<IWritePersonRepository, WritePersonRepository>();
        return services;
    }
}
