using Anticipation.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiDependencies(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Anticipation API v1");
});

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program;
