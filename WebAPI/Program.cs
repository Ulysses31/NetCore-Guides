using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using AspNetCoreRateLimit;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Namotion.Reflection;
using Serilog;
using WebAPI;
using WebAPI.Controllers.v1;
using WebAPI.Models;


var builder = WebApplication.CreateBuilder(args);
var envName = builder.Environment.EnvironmentName;
Debug.Write("--> Environment: " + envName);

// ########  Add services to the container. ######### //
#region Services
builder.Services.AddHttpLogging(o =>
{
  o.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
  o.ResponseHeaders.Add("WebAPI");
  o.MediaTypeOptions.AddText("application/json");
});

builder.Services.AddControllers(setupAction =>
{
  setupAction.ReturnHttpNotAcceptable = true;
  setupAction.EnableEndpointRouting = false;
  setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
  setupAction.OutputFormatters.Add(new XmlSerializerOutputFormatter());
  setupAction.InputFormatters.Add(new XmlSerializerInputFormatter(setupAction));
  setupAction.InputFormatters.Add(new XmlDataContractSerializerInputFormatter(setupAction));
  // setupAction.Filters.Add(new ReqResLogger());
  //setupAction.Filters.Add(
  //  new ProducesResponseTypeAttribute(StatusCodes.Status200OK));
  setupAction.Filters.Add(
    new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
  setupAction.Filters.Add(
    new ProducesResponseTypeAttribute(StatusCodes.Status404NotFound));
  //setupAction.Filters.Add(
  //    new ProducesResponseTypeAttribute(StatusCodes.Status401Unauthorized));
  setupAction.Filters.Add(
    new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
  setupAction.Filters.Add(
    new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
  //setupAction.Filters.Add(
  //  new ProducesDefaultResponseTypeAttribute());
}).AddXmlDataContractSerializerFormatters();

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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

builder.Services.AddApiVersioning(setup =>
{
  setup.DefaultApiVersion = new ApiVersion(1, 0);
  setup.ApiVersionReader = ApiVersionReader.Combine(
    new UrlSegmentApiVersionReader(),
    new HeaderApiVersionReader("x-api-version"),
    new MediaTypeApiVersionReader("x-api-version")
  );
  setup.AssumeDefaultVersionWhenUnspecified = true;
  setup.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(setup =>
{
  setup.GroupNameFormat = "'v'VV";
  setup.SubstituteApiVersionInUrl = true;
});

builder.Services.AddOpenApiDocument(
  config =>
  {
    config.DocumentName = "v1";
    config.ApiGroupNames = new[] { "v1.0" };

    // config.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
    // {
    //   Type = OpenApiSecuritySchemeType.ApiKey,
    //   Name = "Authorization",
    //   In = OpenApiSecurityApiKeyLocation.Header,
    //   Description = "Type into the textbox: Bearer {your JWT token}."
    // });

    // config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));

    config.PostProcess = document =>
    {
      document.Info.Version = "v1.0";
      document.Info.Title = "WebAPI API";
      document.Info.Description = "ASP.NET Core WebAPI API - Health Check Dashboard UI -> https://localhost:7044/dashboard";
      document.Info.TermsOfService = "None";
      document.Info.License = new NSwag.OpenApiLicense
      {
        Name = "MIT License",
        Url = new Uri("https://opensource.org/licenses/MIT").ToString()
      };
      document.Info.Contact = new NSwag.OpenApiContact
      {
        Email = "info@datacenter.com",
        Name = "Iordanidis Chris",
        Url = new Uri("https://www.linkedin.com/in/iordanidischristos/").ToString()
      };

      XmlDocs.ClearCache();
      CachedType.ClearCache();
      GC.Collect();
    };
  }).AddOpenApiDocument(
  config =>
  {
    config.DocumentName = "v2";
    config.ApiGroupNames = new[] { "v2.0" };

    // config.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
    // {
    //   Type = OpenApiSecuritySchemeType.ApiKey,
    //   Name = "Authorization",
    //   In = OpenApiSecurityApiKeyLocation.Header,
    //   Description = "Type into the textbox: Bearer {your JWT token}."
    // });

    // config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));

    config.PostProcess = document =>
    {
      document.Info.Version = "v2.0";
      document.Info.Title = "WebAPI API";
      document.Info.Description = "ASP.NET Core WebAPI API - Health Check Dashboard UI -> https://localhost:7044/dashboard";
      document.Info.TermsOfService = "None";
      document.Info.License = new NSwag.OpenApiLicense
      {
        Name = "MIT License",
        Url = new Uri("https://opensource.org/licenses/MIT").ToString()
      };
      document.Info.Contact = new NSwag.OpenApiContact
      {
        Email = "info@datacenter.com",
        Name = "Iordanidis Chris",
        Url = new Uri("https://www.linkedin.com/in/iordanidischristos/").ToString()
      };

      XmlDocs.ClearCache();
      CachedType.ClearCache();
      GC.Collect();
    };
  });


// Database Context
builder.Services.AddDbContext<CommandContext>((options)
  => options.UseSqlServer(builder.Configuration["ConnectionStrings:MsSqlConnection"])
      .LogTo(
        message => Console.WriteLine(message),
        envName == "Development" ? LogLevel.Trace : LogLevel.Error,
        DbContextLoggerOptions.DefaultWithUtcTime
      )
      .LogTo(
        message => Debug.WriteLine(message),
        envName == "Development" ? LogLevel.Trace : LogLevel.Error,
        DbContextLoggerOptions.DefaultWithUtcTime
      )
      .EnableDetailedErrors(envName == "Development")
      .EnableSensitiveDataLogging(envName == "Development")
      .ConfigureWarnings(
        w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning)
      )
);

// AspNetCoreRateLimit
builder.Services.AddOptions();
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(
  builder.Configuration.GetSection("IpRateLimiting")
);
// builder.Services.Configure<IpRateLimitPolicies>(
//   builder.Configuration.GetSection("IpRateLimitPolicies")
// );
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();


// Health Checks
builder.Services
  .AddHealthChecks()
  .AddSqlServer(builder.Configuration["ConnectionStrings:MsSqlConnection"])
  .AddDbContextCheck<CommandContext>()
  .AddCheck<ApiCommandsHealthChecks>("API /api/v1/commands");


// Health Checks UI
builder.Services.AddHealthChecksUI(options =>
{
  options.SetEvaluationTimeInSeconds(60); //Sets the time interval in which HealthCheck will be triggered
  options.MaximumHistoryEntriesPerEndpoint(10); //Sets the maximum number of records displayed in history
  options.AddHealthCheckEndpoint("Health Checks API", "/health"); //Sets the Health Check endpoint
}).AddInMemoryStorage(); //Here is the memory bank configuration


#endregion Services
// ########  Add services to the container. ######### //

// ########  App ######### //
#region App
var app = builder.Build();

Log.Information("Starting up WebAPI...");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();

  app.UseOpenApi();
  // app.UseSwaggerUI();
  app.UseSwaggerUi3(options =>
  {
    options.CustomStylesheetPath = "./assets/custom-ui.css";
  });
}
else
{
  app.UseExceptionHandler("/Error");
  // The default HSTS value is 30 days. You may want to change this for production scenarios,
  // see https://aka.ms/aspnetcore-hsts.
  app.UseHsts();
}

app.UseIpRateLimiting();

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

//Sets Health Check dashboard options
app.UseHealthChecks("/health", new HealthCheckOptions
{
  Predicate = p => true,
  ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

//Sets the Health Check dashboard configuration
app.UseHealthChecksUI(options => { options.UIPath = "/dashboard"; });


app.Run();
#endregion App
// ########  App ######### //
