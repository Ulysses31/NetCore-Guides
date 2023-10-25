using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MinimalOpenApiExample.HealthChecks
{
    /// <summary>
    /// ApiHealthChecks class
    /// </summary>
    public class ApiHealthChecks : IHealthCheck
    {
        /// <summary>
        /// CheckHealthAsync
        /// </summary>
        /// <param name="context">HealthCheckContext</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns></returns>
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default
        )
        {
            var catUrl = "https://localhost:7000/api/orders/1";

            var client = new HttpClient();

            client.BaseAddress = new Uri(catUrl);

            HttpResponseMessage response = await client.GetAsync("");

            return response.StatusCode == HttpStatusCode.OK ?
                await Task.FromResult(new HealthCheckResult(
                      status: HealthStatus.Healthy,
                      description: $"The API {catUrl} is healthy 😃")) :
                await Task.FromResult(new HealthCheckResult(
                      status: HealthStatus.Unhealthy,
                      description: $"The API {catUrl} is sick 😒"));
        }
    }
}