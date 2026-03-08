using Anticipation.Domain.Repositories;
using Anticipation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Anticipation.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseName = configuration["Persistence:InMemoryDatabaseName"] ?? "AnticipationDb";

        services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(databaseName));
        services.AddScoped<IAnticipationRepository, AnticipationRepository>();

        return services;
    }
}