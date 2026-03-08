using Anticipation.API.Extensions;
using Anticipation.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiDependencies(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Anticipation API v1");
});

app.MapGet("/healthz", () => Results.Ok(new { status = "ok" }));

app.UseHttpsRedirection();
app.UseCors("frontend");
app.MapControllers();

app.Run();

public partial class Program;
