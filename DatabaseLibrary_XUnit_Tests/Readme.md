# Shared Database Repo xUnit Tests

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

## Add Database Library Reference into xUnit Tests Project
```bash
 dotnet add .\DatabaseLibrary-XUnit-Tests\DatabaseLibrary-XUnit-Tests.csproj reference .\DatabaseLibrary\DatabaseLibrary.csproj
```

## Run Tests
```bash
dotnet test .\DatabaseLibrary_XUnit_Tests\DatabaseLibrary_XUnit_Tests.csproj
```