# Web API Project

[YouTube - .Net Core MVC REST API](https://www.youtube.com/watch?v=mUAZ-EbGBOg)

## Packages Installation

### MVC

```bash
dotnet add .\WebAPI.csproj Microsoft.AspNetCore.Mvc.Versioning
```

```bash
dotnet add .\WebAPI.csproj Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
```

### Entity Framework

```bash
dotnet add .\WebAPI.csproj package Microsoft.EntityFrameworkCore
```

```bash
dotnet add .\WebAPI.csproj package Microsoft.EntityFrameworkCore.SqlServer
```

```bash
dotnet add .\WebAPI.csproj package Microsoft.EntityFrameworkCore.Design
```

```bash
dotnet add .\WebAPI.csproj package Microsoft.EntityFrameworkCore.Tools
```

### Configuration

```bash
dotnet add .\WebAPI.csproj package Microsoft.Extensions.Configuration
```

```bash
dotnet add .\WebAPI.csproj package Microsoft.Extensions.Configuration.Json
```

```bash
dotnet add .\WebAPI.csproj package Microsoft.Extensions.DependencyInjection
```

### Log

```bash
dotnet add .\WebAPI.csproj package Microsoft.Extensions.Logging
```

```bash
dotnet add .\WebAPI.csproj package Microsoft.Extensions.Logging.Configuration
```

```bash
dotnet add .\WebAPI.csproj package Microsoft.Extensions.Logging.Console
```

```bash
dotnet add .\WebAPI.csproj package Microsoft.Extensions.Logging.Debug
```

```bash
dotnet add .\WebAPI.csproj package Microsoft.Extensions.Logging.EventLog
```

### Serilog

```bash
dotnet add .\WebAPI.csproj package Serilog
```

```bash
dotnet add .\WebAPI.csproj package Serilog.AspNetCore
```

```bash
dotnet add .\WebAPI.csproj package Serilog.Extensions.Hosting
```

```bash
dotnet add .\WebAPI.csproj package Serilog.Extensions.Logging
```

```bash
dotnet add .\WebAPI.csproj package Serilog.Formatting.Compact
```

```bash
dotnet add .\WebAPI.csproj package Serilog.Settings.Configuration
```

```bash
dotnet add .\WebAPI.csproj package Serilog.Sinks.Console
```

```bash
dotnet add .\WebAPI.csproj package Serilog.Sinks.Debug
```

```bash
dotnet add .\WebAPI.csproj package Serilog.Sinks.Email
```

```bash
dotnet add .\WebAPI.csproj package Serilog.Sinks.File
```

```bash
dotnet add .\WebAPI.csproj package Serilog.Sinks.Http
```

```bash
dotnet add .\WebAPI.csproj package Serilog.Sinks.MSSqlServer
```

### Nswag
```bash
dotnet add .\WebAPI.csproj package Microsoft.AspNetCore.OpenApi
```

```bash
dotnet add .\WebAPI.csproj package NSwag.AspNetCore
```

```bash
dotnet add .\WebAPI.csproj package NSwag.MSBuild
```

### API Versioning
```bash
dotnet add .\WebAPI.csproj package Microsoft.AspNetCore.Mvc.Versioning
```

```bash
dotnet add .\WebAPI.csproj package Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
```

## API AspNetCoreRateLimit

[IpRateLimitMiddleware GitHub Docs](https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/IpRateLimitMiddleware)

```bash
dotnet add .\WebAPI.csproj package AspNetCoreRateLimit
```

```bash
dotnet add .\WebAPI.csproj package AspNetCoreRateLimit.Redis
```

## HealthChecks SQL Server

[Health-Checks](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-7.0)

```bash
dotnet add .\WebAPI.csproj package AspNetCore.HealthChecks.SqlServer
```

```bash
dotnet add .\WebAPI.csproj package Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore
```

```bash
dotnet add .\WebAPI.csproj package AspNetCore.HealthChecks.UI
```

```bash
dotnet add .\WebAPI.csproj package AspNetCore.HealthChecks.UI.Client
```

```bash
dotnet add .\WebAPI.csproj package AspNetCore.HealthChecks.UI.InMemory.Storage
```



## Entity Framework Core Migrations

Migrations are different in MsSQL and MySQL so we output in different folder ( -o MigrationsSQL or -o MigrationsMySQL )

```bash
dotnet ef migrations add Init --project .\WebAPI.csproj -o Migrations -v
```

```bash
dotnet ef --project ..\WebAPI\WebAPI.csproj -v database update
```

```bash
dotnet ef --project ..\WebAPI\WebAPI.csproj -v database drop
```

## Set Running Environment
```bash
dotnet run --environment "Development"
dotnet run --environment "Production"
``` 