# NetCoreMediatrSample
Sample and template for Mediatr pattern in .NET Core

Some of the  dependencies are:
 - AutoFixture: For testing purpose
 - Moq: For testing purpose by mocking dependencies
 - AutoMapper: Eliminate a lot of boilerplate
 - FluentValidation: For validating requests before they are handled
 - MediatR: For dispatching request/response, commands, queries, notifications and events

 Running on .NET Core 2.1
 
 Structure is simple:
  - Src: Here goes the application
  - DataModel: Here goes all the implementations for the database/store
  - Test: Here goes all tests for the application
  
 Src is structured by having a feature in a single file. That gives us the following structure:
  - Controllers: Here goes all the controllers without any logic
  - Features: Here goes all the features (eg. User/GetUser.cs)
  - Infrastructure: Here goes the implementation for the application it self (eg. Middlewares, Filters, Pipeline)
  - ThirdParty: Here goes the implementation for third party services (eg. Facebook login)


Build and run with Docker
```
$ docker build -t aspnetapp .
$ docker run -d -p 8080:80 --name myapp aspnetapp
```
