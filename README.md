# NetCoreMediatrSample
Sample and template for Mediatr pattern in .NET Core.

Some of the  dependencies are:
 - App.metrics: Real time metrics and monitoring set up with InfluxDb and Grafana.
 - AutoFixture: Auto generate test objects.
 - AutoMapper: Eliminate a lot of boilerplate by auto mapping objects (eg. request and response).
 - CorrelationId: Add Correlation ID to Http context to track it.
 - FluentAssertions: Better and easier assertions in tests.
 - FluentValidation: Validating requests before they are handled.
 - Hangfire: Background worker.
 - MediatR: Dispatching request/response, commands, queries, notifications and events.
 - Microsoft.EntityFrameworkCore: Object-relatoinal mapping.
 - Microsoft.Extensions.Logging: Logging API that allow other providers.
 - Moq: Mocking framework used for testing.
 - StructureMap: IoC Container.
 - Xunit: Testing framework.

 Running on .NET Core 2.1
 
 ## Structure
  - Src: The application source.
  - DataModel: Models for the database/store.
  - Test: Application tests (right now only unit tests).
 
 ### Src
 Src is structured by having a feature in a single file. That gives us the following structure:
  - Controllers: All the controllers.
  - Features: All features (eg. User/GetUser.cs).
  - Infrastructure: Infrastructure for the application it self (eg. Middlewares, Filters, Pipeline).
  - ThirdParty: Third party services (eg. Facebook login).
  - Helpers: Shared services that can be used as helper function to different features (eg. UserHelper to retrieve a user by id, email, etc.)

## Setting up application
The appliaction require 2 databases - one for the application and one for Hangfire.
 1. Create a new appsettings to your *ASPNETCORE_ENVIRONMENT* (eg appsettings.Development.json) and add the 2 new connection strings for application and Hangfire.
 2. Run database changes to the application database by running the command `dotnet ef database update -s ../Src` inside DataModel folder (see most of the commands in *DataModel/DatabaseContext.cs*).
 
## Setting up real time metrics
Real time metrics require Grafana and InfluxDb.
 1. Add InfluxDb options to appsettings.
 2. Download Grafana dashboard [here](https://grafana.com/dashboards/2125).
 
## Logging
Logging is set up with Microsoft.Extensions.Logging which means you can add logging providers by your self to it.
As now it is set up as follow:
 - Status codes 5xx, that are caused by an exception, are logged as critical.
 - Other status codes, that are caused by an exception, are logged as warning.
 - The whole pipeline (request to response) is logged as information.

Critical and warning logs are named `<endpoint> :: [<status code>] <exception message>` and contain request, stacktrace and correlation id.

The user receives error message and correlation id in production. For non-production environments the stacktrace is also included.

## Build and run with Docker
```
$ docker build -t aspnetapp .
$ docker run -d -p 8080:80 --name myapp aspnetapp
```
