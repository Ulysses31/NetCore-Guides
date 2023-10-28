using System.Reflection;
using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CommonServiceCollection.Swagger
{
    /// <summary>
    /// CommonSwaggerExtension class
    /// </summary>
    public static class CommonSwaggerExtension
    {
        /// <summary>
        /// CommonSwaggerSetup function
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection CommonSwaggerSetup(
            this IServiceCollection services,
            string? xmlDocumentFile
        )
        {
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
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlDocumentFile!));

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

            return services;
        }


        /// <summary>
        /// UseCommonSwagger function
        /// </summary>
        /// <param name="app">WebApplication</param>
        /// <returns>WebApplication</returns>
        public static WebApplication UseCommonSwagger(
            this WebApplication app
        )
        {
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

            return app;
        }

    }
}