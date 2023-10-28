namespace CommonServiceCollection.Swagger;

using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Text;

/// <summary>
/// Configures the Swagger generation options.
/// </summary>
/// <remarks>This allows API versioning to define a Swagger document per API version after the
/// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider provider;
    private readonly IConfiguration configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
    /// </summary>
    /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
    /// <param name="configuration">The <see cref="IConfiguration"></see></param>
    public ConfigureSwaggerOptions(
      IApiVersionDescriptionProvider provider,
      IConfiguration configuration
    )
    {
        this.provider = provider;
        this.configuration = configuration;

        this.configuration!
          .GetSection(CommonSwaggerOptions.MySwagger)
          .Bind(new CommonSwaggerOptions());
    }

    /// <inheritdoc />
    public void Configure(SwaggerGenOptions options)
    {
        // add a swagger document for each discovered API version
        // note: you might choose to skip or document deprecated API versions differently
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var text =
          new StringBuilder("An example application with OpenAPI, Swashbuckle, and API versioning.");

        var info = new OpenApiInfo()
        {
            // Title = "Example API",
            Title = CommonSwaggerOptions.Title,
            Version = description.ApiVersion.ToString(),
            TermsOfService = new Uri(CommonSwaggerOptions.TermsOfService!),
            Contact = new OpenApiContact()
            {
                Name = CommonSwaggerOptions.Contact!.Name,
                Email = CommonSwaggerOptions.Contact!.Email,
                Url = new Uri(CommonSwaggerOptions.Contact.Url!)
            },
            License = new OpenApiLicense()
            {
                Name = CommonSwaggerOptions.License!.Name,
                Url = new Uri(CommonSwaggerOptions.License.Url!)
            }
        };

        if (description.IsDeprecated)
        {
            text.Append(CommonSwaggerOptions.Options!.Deprecate_Version_Description);
        }

        if (description.SunsetPolicy is SunsetPolicy policy)
        {
            if (policy.Date is DateTimeOffset when)
            {
                text.Append(CommonSwaggerOptions.Options!.Sunset_Policy_Description)
                    .Append(when.Date.ToShortDateString())
                    .Append('.');
            }

            if (policy.HasLinks)
            {
                text.AppendLine();

                for (var i = 0; i < policy.Links.Count; i++)
                {
                    var link = policy.Links[i];

                    if (link.Type == "text/html")
                    {
                        text.AppendLine();

                        if (link.Title.HasValue)
                        {
                            text.Append(link.Title.Value).Append(": ");
                        }

                        text.Append(link.LinkTarget.OriginalString);
                    }
                }
            }
        }

        info.Description = text.ToString();

        return info;
    }
}