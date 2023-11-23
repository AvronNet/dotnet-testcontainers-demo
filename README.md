# Events Demo application

This .NET applicaiton showcases Clean Architecture and Integration testing using testcontainers. 
It is a simple application that allows users to create and edit events.

## Prerequisites
- .NET 7 SDK
- Docker
- Docker Compose

## Running the application

## Steps to run in Local Environment

1. Open project in Visual Studio
2. Set Startup project to "Web"
3. Set Launch profile to "Web"
4. Run
    - a testcontainer will be spun up for your DB
    - DB will automatically populate with tables using EF migrations


## Entity Framework Migrations

After creating new entities for a DB context or changeing them add migration for the changes by running
`dotnet ef migrations add NewMigrationName -p src\Events.Infrastructure -s src\Events.Api -o Migrations --context EventsDBContext`
This will generate migration files in the Migrations folder of the Core project.

Note: To run this you will need to install the dotnet dotnet-ef tool.

More details on Entity Framework Core tools can be found in MS docs [here](https://docs.microsoft.com/en-us/ef/core/cli/dotnet).
More info about Entity Framework Migrations can be found in the [Entity Framework tutorial pages](https://www.entityframeworktutorial.net/efcore/entity-framework-core-migration.aspx)

## Testcontainers

This solution contains an IntegrationTest project that uses Testcontainers. 
Testcontainers is a library to support tests with throwaway instances of Docker containers.
[testcontainers-dotnet github page](https://github.com/testcontainers/testcontainers-dotnet)

More details about the approach - [ASP.NET Core Integration Tests with Test Containers & Postgres](https://www.azureblue.io/asp-net-core-integration-tests-with-test-containers-and-postgres/)

## HTTPS Certificate
You need a development certificate to use HTTPS on API container, to do this, follow the next steps. 
1. Open git-bash.
1. Create a local https certificate by running these two lines. (Set the exact password below for now--dotnet requires a password for pfx files, and this specific password is referenced in the docker-compose.yml file.)
```bash
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p pass1234
dotnet dev-certs https --trust
```
Details about [Hosting ASP.NET Core images with Docker Compose over HTTPS](https://docs.microsoft.com/en-us/aspnet/core/security/docker-compose-https?view=aspnetcore-6.0#windows-using-linux-containers) are on the given link.