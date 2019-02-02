# NetCoreMediatrSample
Sample using Command and Query Responsibility Segregation (CQRS) implemented in .NET Core by using MediatR and identityserver4, background workers, real-time metrics, monitoring, logging, validations, swagger and more

Some of the  dependencies are:
 - App.Metrics: Real time metrics and monitoring set up with InfluxDb and Grafana.
 - AutoFixture: Auto generate test objects.
 - AutoMapper: Eliminate a lot of boilerplate by auto mapping objects (eg. request and response).
 - CorrelationId: Add Correlation ID to Http context to easier track errors.
 - FluentAssertions: Better and easier assertions in tests.
 - FluentValidation: Validating requests before they are handled.
 - Hangfire: Background worker.
 - Identityserver4: OpenID Connect and OAuth 2.0 framework.
 - MediatR: Dispatching request/response, commands, queries, notifications and events.
 - Microsoft.EntityFrameworkCore: Object-relational mapping.
 - Microsoft.Extensions.Logging: Logging API that allow other providers.
 - Moq: Mocking framework used for testing.
 - Sentry.io: Logging provider
 - StructureMap: IoC Container.
 - Swagger: API documentation page
 - Xunit: Testing framework.

 Running on .NET Core 2.2
 
 See my project [knowledge-quiz-api](https://github.com/madslundt/knowledge-quiz-api) for real-world usage.
 
 ## Structure
  - API: Source of the application.
  - DataModel: Models for the database/store.
  - UnitTest: Unit tests for the application.
  - IDP: Identity Provider using Identityserver4.

 ### API
 API is structured by having each feature in a single file. That gives the following structure:
  - Controllers: All the controllers with endpoints exposed.
  - Features: All features (eg. User/GetUser.cs).
  - Infrastructure: Infrastructure for the application it self (eg. Middlewares, Filters, Pipeline).
  - ThirdParty: Third party services (eg. Facebook login).

## Setting up application
The application require 2 databases - one for the application it self and one for Hangfire.
 1. Create a new appsettings to your *ASPNETCORE_ENVIRONMENT* (eg appsettings.Development.json) and add the 2 new connection strings for application and Hangfire.
 2. Run database changes to the application database by running the command `dotnet ef database update -s ../API` inside DataModel folder (see commands in *DataModel/DatabaseContext.cs*).
 3. Add connection string to the Identity server (IDP/appsettings.json or change environment and add a new appsettings). The database used for the application can also be used to the Identity server. Once the Identity server project is ran it will run the migrations for it.
 
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

The user receives error message and correlation id in production. For development environment the stack trace is also included.

Sentry.io logging provider has been added to the project. This can be used or removed.

## Build and run with Docker
```
$ docker build -t aspnetapp .
$ docker run -d -p 8080:80 --name myapp aspnetapp
```
