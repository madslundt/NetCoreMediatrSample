using Components;
using DataModel;
using Infrastructure.BackgroundJob;
using Infrastructure.BackgroundJob.Hangfire;
using Infrastructure.Cors;
using Infrastructure.CQRS;
using Infrastructure.ExceptionHandling;
using Infrastructure.HealthChecks;
using Infrastructure.StronglyTypedIds;
using Infrastructure.Swagger;

var builder = WebApplication.CreateBuilder(args);

var dataModelConnectionString = builder.Configuration.GetConnectionString(DataModelOptions.ConnectionString) ??
                                throw new Exception(
                                    $"{DataModelOptions.ConnectionString} is not found in configuration");

var hangfireConnectionString = builder.Configuration.GetConnectionString(BackgroundJobOptions.ConnectionString) ??
                               throw new Exception(
                                   $"{BackgroundJobOptions.ConnectionString} is not found in configuration");

builder.Services
    .AddSwagger()
    .AddComponents()
    .AddDataModel(dataModelConnectionString)
    .AddHangfire(hangfireConnectionString)
    .AddCQRS()
    .AddExceptionHandling()
    .AddApiHealthChecks()
    .AddControllers()
    .AddStronglyTypedIds();

var allowAllOrigins = "_allowAllOrigins";
builder.Services.AddCorsPolicy(allowAllOrigins);

var app = builder.Build();

if (!app.Environment.IsProduction())
{
    app.UseCorsPolicy(allowAllOrigins);
    app.UseSwaggerWithUI();
}

app.UseHttpsRedirection();

app
    .UseCQRS()
    .UseExceptionhandling()
    .UseApiHealthChecks()
    .MapControllers();

app.Run();

// Needed for Api.IntegrationTests project
public partial class Program
{
}