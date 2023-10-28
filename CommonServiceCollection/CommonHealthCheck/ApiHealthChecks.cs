using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CommonServiceCollection.CommonHealthCheck
{
    /// <summary>
    /// ApiHealthChecks class
    /// </summary>
    public class ApiHealthChecks : IHealthCheck
    {
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// ApiHealthChecks constructor
        /// </summary>
        /// <param name="httpClientFactory">IHttpClientFactory</param>
        public ApiHealthChecks(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

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
            HttpClient httpClient = _httpClientFactory.CreateClient("api-health-check");

            HttpResponseMessage response =
                await httpClient.GetAsync(httpClient.BaseAddress, cancellationToken);

            return response.StatusCode == HttpStatusCode.OK ?
                await Task.FromResult(new HealthCheckResult(
                      status: HealthStatus.Healthy,
                      description: $"The API {httpClient.BaseAddress} is healthy 😃")) :
                await Task.FromResult(new HealthCheckResult(
                      status: HealthStatus.Unhealthy,
                      description: $"The API {httpClient.BaseAddress} is sick 😒"));
        }
    }
}