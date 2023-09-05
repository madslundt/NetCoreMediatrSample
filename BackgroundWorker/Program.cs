using DataModel;
using EventHandlers;
using Infrastructure.BackgroundJob;
using Infrastructure.BackgroundJob.Hangfire;
using Infrastructure.CQRS;

var builder = WebApplication.CreateBuilder(args);


var hangfireConnectionString = builder.Configuration.GetConnectionString(BackgroundJobOptions.ConnectionString) ??
                               throw new Exception(
                                   $"{BackgroundJobOptions.ConnectionString} is not found in configuration");

var dataModelConnectionString = builder.Configuration.GetConnectionString(DataModelOptions.ConnectionString) ??
                                throw new Exception(
                                    $"{DataModelOptions.ConnectionString} is not found in configuration");

builder.Services
    .AddDataModel(dataModelConnectionString)
    .AddHangfire(hangfireConnectionString)
    .AddCQRS()
    .AddEventHandlers()
    .AddHangfireWorker();

var app = builder.Build();

app
    .UseHangfireUI("/hangfire")
    .Run();