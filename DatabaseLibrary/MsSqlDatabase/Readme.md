# Shared Database Repo

## Packages Installation

### Entity Framework
```bash
dotnet add .\DatabaseLibrary.csproj package Microsoft.EntityFrameworkCore
```

```bash
dotnet add .\DatabaseLibrary.csproj package Microsoft.EntityFrameworkCore.SqlServer
```

```bash
dotnet add .\DatabaseLibrary.csproj package Microsoft.EntityFrameworkCore.Design
```

```bash
dotnet add .\DatabaseLibrary.csproj package Microsoft.EntityFrameworkCore.Tools
```

### Configuration
```bash
dotnet add .\DatabaseLibrary.csproj package Microsoft.Extensions.Configuration
```

```bash
dotnet add .\DatabaseLibrary.csproj package Microsoft.Extensions.Configuration.Json
```

```bash
dotnet add .\DatabaseLibrary.csproj package Microsoft.Extensions.DependencyInjection
```

## Entity Framework Core Migrations

Migrations are different in MsSQL and MySQL so we output in different folder ( -o MigrationsSQL or -o MigrationsMySQL )


```bash
dotnet ef migrations add Init --project .\DatabaseLibrary.csproj -o MsSqlDatabase\Migrations -v
```

```bash
dotnet ef --project ..\DatabaseLibrary\DatabaseLibrary.csproj -v database update
```

```bash
dotnet ef --project ..\DatabaseLibrary\DatabaseLibrary.csproj -v database drop
```