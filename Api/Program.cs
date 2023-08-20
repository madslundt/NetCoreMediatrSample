using Components;
using DataModel;
using EventHandlers;
using Infrastructure.Cors;
using Infrastructure.CQRS;
using Infrastructure.Swagger;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DataModelConnectionString");

// Add services to the container.
builder.Services
    .AddEventHandlers()
    .AddSwagger()
    .AddComponents()
    .AddDataModel(connectionString!)
    .AddCQRS()
    .AddControllers();

var allowAllOrigins = "_allowAllOrigins";
builder.Services.AddCorsPolicy(allowAllOrigins);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCorsPolicy(allowAllOrigins);
    app.UseSwaggerWithUI();
}

app.UseHttpsRedirection();

app
    .UseCQRS()
    .MapControllers();

app.Run();