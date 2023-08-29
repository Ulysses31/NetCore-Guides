# Web API Project

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
