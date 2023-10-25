using System;
using System.Collections.Generic;
using System.Net.Http;
using MinimalOpenApiExample;
using Asp.Versioning;
using Asp.Versioning.Conventions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using OrderV1 = MinimalOpenApiExample.Models.V1.Order;
using OrderV2 = MinimalOpenApiExample.Models.V2.Order;
using OrderV3 = MinimalOpenApiExample.Models.V3.Order;
using PersonV1 = MinimalOpenApiExample.Models.V1.Person;
using PersonV2 = MinimalOpenApiExample.Models.V2.Person;
using PersonV3 = MinimalOpenApiExample.Models.V3.Person;
using System.Reflection;
using System.IO;
using Microsoft.OpenApi.Models;
using Serilog;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net;
using MinimalOpenApiExample.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Add services to the container.
services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
    options.ResponseHeaders.Add("minimal-openapi-axample-api");
    options.MediaTypeOptions.AddText("application/json");
});

// SERILOG
var _logger = new LoggerConfiguration()
  .Enrich.WithProperty("Source", "WebAPI")
  .Enrich.WithProperty("OSVersion", Environment.OSVersion)
  .Enrich.WithProperty("ServerName", System.Net.Dns.GetHostName())
  .Enrich.WithProperty("UserName", Environment.UserName)
  .Enrich.WithProperty("UserDomainName", Environment.UserDomainName)
  .Enrich.WithProperty("Address", new Shared().GetHostIpAddress())
  .ReadFrom.Configuration(builder.Configuration)
  .CreateLogger();
builder.Logging.AddSerilog(_logger);

// Health Checks
builder.Services
  .AddHealthChecks()
  // .AddSqlServer(builder.Configuration["ConnectionStrings:MsSqlConnection"])
  // .AddDbContextCheck<CommandContext>()
  .AddCheck<ApiHealthChecks>("API api/orders/1");

builder.Services.AddHealthChecksUI(options =>
{
    options.SetEvaluationTimeInSeconds(60);       //Sets the time interval in which HealthCheck will be triggered
    options.MaximumHistoryEntriesPerEndpoint(10); //Sets the maximum number of records displayed in history
    options.AddHealthCheckEndpoint("Health Checks API", "/health"); //Sets the Health Check endpoint
}).AddInMemoryStorage(); //Here is the memory bank configuration

#region Swagger
services.AddAuthentication().AddJwtBearer();

services.AddAuthorizationBuilder()
  .AddPolicy("admin_greetings", policy =>
    policy
      .RequireRole("admin")
      .RequireClaim("scope", "greetings_api"));

services.AddEndpointsApiExplorer();
services.AddApiVersioning(
        options =>
        {
            // reporting api versions will return the headers
            // "api-supported-versions" and "api-deprecated-versions"
            options.ReportApiVersions = true;

            options.ApiVersionReader = ApiVersionReader.Combine(
              new UrlSegmentApiVersionReader(),
              new HeaderApiVersionReader("x-api-version"),
              new MediaTypeApiVersionReader("x-api-version")
            );

            options.Policies.Sunset(0.9)
                            .Effective(DateTimeOffset.Now.AddDays(60))
                            .Link("policy.html")
                                .Title("Versioning Policy")
                                .Type("text/html");
        })
    .AddApiExplorer(
        options =>
        {
            // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            options.GroupNameFormat = "'v'VVV";

            // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
            // can also be used to control the format of the API version in route templates
            options.SubstituteApiVersionInUrl = true;
        });
services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
services.AddSwaggerGen(options =>
{
    options.OperationFilter<SwaggerDefaultValues>();
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
      new OpenApiSecurityScheme
      {
        Reference = new OpenApiReference
        {
          Type = ReferenceType.SecurityScheme,
          Id = "Bearer"
        }
      },
      Array.Empty<string>()
    }
  });
});
#endregion Swagger

// Configure the HTTP request pipeline.
var app = builder.Build();
var healthChecks = app.NewApiVersionSet("HealthChecks").Build();
var orders = app.NewApiVersionSet("Orders").Build();
var people = app.NewApiVersionSet("People").Build();

#region ENDPOINTS
// HealthCheck
app.MapGet("/health", async () =>
{
    HealthCheckService healthCheckService = app.Services.GetService<HealthCheckService>();
    var report = await healthCheckService.CheckHealthAsync();
    // _logger.LogInformation($"Get Health Information: {report}");
    return Results.Ok(report.Status ==
       HealthStatus.Healthy
           ? report
           : (int)HttpStatusCode.ServiceUnavailable);
})
   .Produces(200)
   .Produces(503)
   .WithApiVersionSet(healthChecks)
   .HasApiVersion(1.0);

// 1.0
app.MapGet("/api/orders/{id:int}", async (int? id) =>
{
    return Results.Ok(new OrderV1() { Id = id, Customer = "John Doe" });
})
   //  .RequireAuthorization("admin_greetings")
   .Produces<OrderV1>(200, "application/json", new string[] { "application/xml" })
   .Produces(401)
   .Produces(404)
   .WithApiVersionSet(orders)
   .HasDeprecatedApiVersion(0.9)
   .HasApiVersion(1.0);

app.MapPost("/api/orders", async (HttpRequest request, OrderV1 order) =>
    {
        order.Id = 42;
        var scheme = request.Scheme;
        var host = request.Host;
        var location = new Uri($"{scheme}{Uri.SchemeDelimiter}{host}/api/orders/{order.Id}");
        return Results.Created(location, order);
    })
   .RequireAuthorization("admin_greetings")
   .Accepts<OrderV1>("application/json", new string[] { "application/xml" })
   .Produces<OrderV1>(201, "application/json", new string[] { "application/xml" })
   .Produces(400)
   .Produces(401)
   .WithApiVersionSet(orders)
   .HasApiVersion(1.0);

app.MapMethods(
  "/api/orders/{id:int}",
  new[] { HttpMethod.Patch.Method },
  async (int id, OrderV1 order) => Results.NoContent())
   .RequireAuthorization("admin_greetings")
   .Accepts<OrderV1>("application/json", new string[] { "application/xml" })
   .Produces(204)
   .Produces(400)
   .Produces(401)
   .Produces(404)
   .WithApiVersionSet(orders)
   .HasApiVersion(1.0);

// 2.0
app.MapGet("/api/orders", async () =>
{
    return Results.Ok(new OrderV2[]
    {
      new(){ Id = 1, Customer = "John Doe" },
      new(){ Id = 2, Customer = "Bob Smith" },
      new(){ Id = 3, Customer = "Jane Doe", EffectiveDate = DateTimeOffset.UtcNow.AddDays( 7d ) },
    });
})
   .RequireAuthorization("admin_greetings")
   .Produces<IEnumerable<OrderV2>>(200, "application/json", new string[] { "application/xml" })
   .Produces(401)
   .Produces(404)
   .WithApiVersionSet(orders)
   .HasApiVersion(2.0);

app.MapGet("/api/orders/{id:int}", async (int id) =>
{
    return Results.Ok(new OrderV2() { Id = id, Customer = "John Doe" });
})
   .RequireAuthorization("admin_greetings")
   .Produces<OrderV2>(200, "application/json", new string[] { "application/xml" })
   .Produces(401)
   .Produces(404)
   .WithApiVersionSet(orders)
   .HasApiVersion(2.0);

app.MapPost("/api/orders", async (HttpRequest request, OrderV2 order) =>
    {
        order.Id = 42;
        var scheme = request.Scheme;
        var host = request.Host;
        var location = new Uri($"{scheme}{Uri.SchemeDelimiter}{host}/api/orders/{order.Id}");
        return Results.Created(location, order);
    })
   .RequireAuthorization("admin_greetings")
   .Accepts<OrderV2>("application/json", new string[] { "application/xml" })
   .Produces<OrderV2>(201, "application/json", new string[] { "application/xml" })
   .Produces(400)
   .Produces(401)
   .WithApiVersionSet(orders)
   .HasApiVersion(2.0);

app.MapMethods(
  "/api/orders/{id:int}",
  new[] { HttpMethod.Patch.Method },
  async (int id, OrderV2 order) => Results.NoContent())
   .RequireAuthorization("admin_greetings")
   .Accepts<OrderV2>("application/json", new string[] { "application/xml" })
   .Produces(204)
   .Produces(400)
   .Produces(401)
   .Produces(404)
   .WithApiVersionSet(orders)
   .HasApiVersion(2.0);

// 3.0
app.MapGet("/api/orders", async () =>
{
    return Results.Ok(new OrderV3[]
    {
                new(){ Id = 1, Customer = "John Doe" },
                new(){ Id = 2, Customer = "Bob Smith" },
                new(){ Id = 3, Customer = "Jane Doe", EffectiveDate = DateTimeOffset.UtcNow.AddDays( 7d ) },
    });
})
   .RequireAuthorization("admin_greetings")
   .Produces<IEnumerable<OrderV3>>(200, "application/json", new string[] { "application/xml" })
   .Produces(401)
   .Produces(404)
   .WithApiVersionSet(orders)
   .HasApiVersion(3.0);

app.MapGet("/api/orders/{id:int}", async (int id) =>
{
    return Results.Ok(new OrderV3() { Id = id, Customer = "John Doe" });
})
   .RequireAuthorization("admin_greetings")
   .Produces<OrderV3>(200, "application/json", new string[] { "application/xml" })
   .Produces(401)
   .Produces(404)
   .WithApiVersionSet(orders)
   .HasApiVersion(3.0);

app.MapPost("/api/orders", async (HttpRequest request, OrderV3 order) =>
    {
        order.Id = 42;
        var scheme = request.Scheme;
        var host = request.Host;
        var location = new Uri($"{scheme}{Uri.SchemeDelimiter}{host}/api/orders/{order.Id}");
        return Results.Created(location, order);
    })
   .RequireAuthorization("admin_greetings")
   .Accepts<OrderV3>("application/json", new string[] { "application/xml" })
   .Produces<OrderV3>(201, "application/json", new string[] { "application/xml" })
   .Produces(400)
   .Produces(401)
   .WithApiVersionSet(orders)
   .HasApiVersion(3.0);

app.MapDelete("/api/orders/{id:int}", async (int id) => Results.NoContent())
   .RequireAuthorization("admin_greetings")
   .Produces(204)
   .Produces(401)
   .WithApiVersionSet(orders)
   .HasApiVersion(3.0);

// 1.0
app.MapGet("/api/v{version:apiVersion}/people/{id:int}", async (int id) =>
{
    return Results.Ok(new PersonV1()
    {
        Id = id,
        FirstName = "John",
        LastName = "Doe",
    });
})
   .RequireAuthorization("admin_greetings")
   .Produces<PersonV1>(200, "application/json", new string[] { "application/xml" })
   .Produces(401)
   .Produces(404)
   .WithApiVersionSet(people)
   .HasDeprecatedApiVersion(0.9)
   .HasApiVersion(1.0);

// 2.0
app.MapGet("/api/v{version:apiVersion}/people", async () =>
{
    return Results.Ok(new PersonV2[]
    {
        new()
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@somewhere.com",
        },
        new()
        {
            Id = 2,
            FirstName = "Bob",
            LastName = "Smith",
            Email = "bob.smith@somewhere.com",
        },
        new()
        {
            Id = 3,
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@somewhere.com",
        },
    });
})
   .RequireAuthorization("admin_greetings")
   .Produces<IEnumerable<PersonV2>>(200, "application/json", new string[] { "application/xml" })
   .Produces(401)
   .Produces(404)
   .WithApiVersionSet(people)
   .HasApiVersion(2.0);

app.MapGet("/api/v{version:apiVersion}/people/{id:int}", async (int id) =>
{
    return Results.Ok(new PersonV2()
    {
        Id = id,
        FirstName = "John",
        LastName = "Doe",
        Email = "john.doe@somewhere.com",
    });
})
   .RequireAuthorization("admin_greetings")
   .Produces<PersonV2>(200, "application/json", new string[] { "application/xml" })
   .Produces(401)
   .Produces(404)
   .WithApiVersionSet(people)
   .HasApiVersion(2.0);

// 3.0
app.MapGet("/api/v{version:apiVersion}/people", async () =>
{
    return Results.Ok(new PersonV3[]
    {
        new()
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@somewhere.com",
            Phone = "555-987-1234",
        },
        new()
        {
            Id = 2,
            FirstName = "Bob",
            LastName = "Smith",
            Email = "bob.smith@somewhere.com",
            Phone = "555-654-4321",
        },
        new()
        {
            Id = 3,
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@somewhere.com",
            Phone = "555-789-3456",
        },
    });
})
   .RequireAuthorization("admin_greetings")
   .Produces<IEnumerable<PersonV3>>(200, "application/json", new string[] { "application/xml" })
   .Produces(401)
   .Produces(404)
   .WithApiVersionSet(people)
   .HasApiVersion(3.0);

app.MapGet("/api/v{version:apiVersion}/people/{id:int}", async (int id) =>
{
    return Results.Ok(new PersonV3()
    {
        Id = id,
        FirstName = "John",
        LastName = "Doe",
        Email = "john.doe@somewhere.com",
        Phone = "555-987-1234",
    });
})
   .RequireAuthorization("admin_greetings")
   .Produces<PersonV3>(200, "application/json", new string[] { "application/xml" })
   .Produces(401)
   .Produces(404)
   .WithApiVersionSet(people)
   .HasApiVersion(3.0);

app.MapPost("/api/v{version:apiVersion}/people", async (HttpRequest request, PersonV3 person) =>
    {
        person.Id = 42;
        var scheme = request.Scheme;
        var host = request.Host;
        var version = request.HttpContext.GetRequestedApiVersion();
        var location = new Uri($"{scheme}{Uri.SchemeDelimiter}{host}/v{version}/api/people/{person.Id}");
        return Results.Created(location, person);
    })
   .RequireAuthorization("admin_greetings")
   .Accepts<PersonV3>("application/json", new string[] { "application/xml" })
   .Produces<PersonV3>(201, "application/json", new string[] { "application/xml" })
   .Produces(400)
   .Produces(401)
   .WithApiVersionSet(people)
   .HasApiVersion(3.0);
#endregion ENDPOINTS

app.UseSwagger();
app.UseSwaggerUI(
    options =>
    {
        var descriptions = app.DescribeApiVersions();

        // build a swagger endpoint for each discovered API version
        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });

app.UseHttpLogging();
// app.UseAuthentication();
// app.UseAuthorization();

//Sets Health Check dashboard options
app.UseHealthChecks("/health", new HealthCheckOptions
{
    Predicate = p => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

//Sets the Health Check dashboard configuration
app.UseHealthChecksUI(options => { options.UIPath = "/dashboard"; });

app.Run();