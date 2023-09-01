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
Console.WriteLine(hangfireConnectionString);

builder.Services
    .AddDataModel(dataModelConnectionString)
    .AddCQRS()
    .AddHangfire(hangfireConnectionString)
    .AddEventHandlers()
    .AddHangfireWorker();

var app = builder.Build();

app
    .UseHangfireUI("")
    .Run();