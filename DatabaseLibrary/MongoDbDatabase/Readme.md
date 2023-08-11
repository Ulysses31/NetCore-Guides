# Shared Database Repo

## Packages Installation

### MongoDb Driver
```bash
dotnet add .\DatabaseLibrary.csproj package Mongo.Driver
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

## Mongo Database Repo Installation

```bash
// ### SERVICES ###
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
  .AddMongo()
  .AddMongoRepository<Item>("items");
```

```bash
 public class Item : IBaseEntity
  {
    public Guid Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string Description { get; set; } = String.Empty;
    public decimal Price { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
  }
```