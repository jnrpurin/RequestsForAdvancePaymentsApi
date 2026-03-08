using Asp.Versioning;
using Anticipation.API.Filters;
using Anticipation.Application.Handlers;
using Anticipation.Application.Interfaces;
using Anticipation.Application.Services;
using Anticipation.Domain.Services;
using Anticipation.Infrastructure.DependencyInjection;

namespace Anticipation.API.Extensions;

public static class DependencyInjection
{
    private const string FrontendCorsPolicy = "frontend";

    public static IServiceCollection AddApiDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
            ?? new[] { "http://localhost:5173", "http://localhost:4173" };

        services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());
        services.AddCors(options =>
        {
            options.AddPolicy(FrontendCorsPolicy, policy =>
            {
                policy.WithOrigins(allowedOrigins)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = new UrlSegmentApiVersionReader();
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = "Anticipation API",
                Version = "v1",
                Description = "Requests for advance payments API"
            });
        });

        services.AddScoped<AnticipationDomainService>();
        services.AddScoped<CreateAnticipationHandler>();
        services.AddScoped<SimulateAnticipationHandler>();
        services.AddScoped<ApproveAnticipationHandler>();
        services.AddScoped<RejectAnticipationHandler>();
        services.AddScoped<GetAllAnticipationsHandler>();
        services.AddScoped<GetByCreatorHandler>();
        services.AddScoped<IAnticipationService, AnticipationService>();

        services.AddInfrastructure(configuration);

        return services;
    }
}