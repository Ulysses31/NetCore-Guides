using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonServiceCollection.DatabaseOptions;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommonServiceCollection.CommonHealthCheck
{
    /// <summary>
    /// CommonHealthCheckExtension class
    /// </summary>
    public static class CommonHealthCheckExtension
    {
        /// <summary>
        /// CommonHealthCheckSetup function
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection CommonHealthCheckSetup<T>(
          this IServiceCollection services,
          string apiEndpoint,
          string dbTypeEnum
        ) where T : DbContext
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            string? connString = string.Empty;

            switch (dbTypeEnum)
            {
                case "MsSql":
                    connString = configuration["ConnectionStrings:MsSqlConnection"];
                    break;
                case "MySql":
                    connString = configuration["ConnectionStrings:MySqlConnection"];
                    break;
                case "SqLite":
                    connString = configuration["ConnectionStrings:SqlLiteConnection"];
                    break;
                case "MongoDb":
                    connString = configuration["ConnectionStrings:MongoDbConnection"];
                    break;
            }

            // HTTP Clients
            services.AddHttpClient("api-health-check", options =>
            {
                // options.BaseAddress = new Uri("https://localhost:7000/api/orders/1");
                options.BaseAddress = new Uri(apiEndpoint);
                options.DefaultRequestHeaders.Add("x-api-version", "1.0");
            });

            services.AddHealthChecks()
                //.AddSqlServer(connString!)
                //.AddDbContextCheck<T>()
                .AddCheck<ApiHealthChecks>($"API {apiEndpoint}");

            services.AddHealthChecksUI(options =>
            {
                options.SetEvaluationTimeInSeconds(60);       //Sets the time interval in which HealthCheck will be triggered
                options.MaximumHistoryEntriesPerEndpoint(10); //Sets the maximum number of records displayed in history
                options.AddHealthCheckEndpoint("Health Checks API", "/health"); //Sets the Health Check endpoint
            }).AddInMemoryStorage(); //Here is the memory bank configuration

            return services;
        }

        /// <summary>
        /// CommonHealthCheckUseSetup function
        /// </summary>
        /// <param name="app">WebApplication</param>
        /// <returns>WebApplication</returns>
        public static WebApplication CommonHealthCheckUseSetup(
            this WebApplication app
        )
        {
            //Sets Health Check dashboard options
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = p => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            //Sets the Health Check dashboard configuration
            app.UseHealthChecksUI(options => { options.UIPath = "/dashboard"; });

            return app;
        }
    }
}