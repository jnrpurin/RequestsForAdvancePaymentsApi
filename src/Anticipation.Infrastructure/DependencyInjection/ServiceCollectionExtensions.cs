using Anticipation.Domain.Repositories;
using Anticipation.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Anticipation.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["Persistence:ConnectionString"]
            ?? "Data Source=AnticipationDb;Mode=Memory;Cache=Shared";

        services.AddSingleton(_ =>
        {
            var connection = new SqliteConnection(connectionString);
            connection.Open();
            return connection;
        });

        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            var connection = serviceProvider.GetRequiredService<SqliteConnection>();
            options.UseSqlite(connection);
        });

        services.AddScoped<IAnticipationRepository, AnticipationRepository>();

        return services;
    }
}