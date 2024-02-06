# NetCoreAuthSeed

# Migrations

## Add new Migration
dotnet ef migrations add InitialMigration --startup-project ../NetCoreAuthSeed.Api/

## Update Database
dotnet ef database update --startup-project ../NetCoreAuthSeed.Api/