using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Namotion.Reflection;
using Serilog;
using WebAPI;
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
  // setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
  // setupAction.OutputFormatters.Add(new XmlSerializerOutputFormatter());
  // setupAction.InputFormatters.Add(new XmlSerializerInputFormatter(setupAction));
  // setupAction.InputFormatters.Add(new XmlDataContractSerializerInputFormatter(setupAction));
  // setupAction.Filters.Add(new ReqResLogger());
  // setupAction.Filters.Add(
  //   new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
  // setupAction.Filters.Add(
  //     new ProducesResponseTypeAttribute(StatusCodes.Status401Unauthorized));
  // setupAction.Filters.Add(
  //     new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
  // setupAction.Filters.Add(
  //     new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
  // setupAction.Filters.Add(
  //     new ProducesDefaultResponseTypeAttribute());
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
      document.Info.Description = "ASP.NET Core WebAPI API";
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
      document.Info.Description = "ASP.NET Core WebAPI API";
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

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
#endregion App
// ########  App ######### //
