using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebAPI.Controllers.v1
{
  /// <summary>
  /// HealthController
  /// </summary>
  [ApiController]
  [ApiVersion("1.0")]
  [Route("api/v{version:apiVersion}/[controller]")]
  public class HealthController : ControllerBase
  {
    private readonly ILogger<HealthController> _logger;
    private readonly HealthCheckService _service;

    /// <summary>
    /// HealthController constructor
    /// </summary>
    /// <param name="logger">ILogger</param>
    /// <param name="service">HealthCheckService</param>
    public HealthController(
        ILogger<HealthController> logger,
        HealthCheckService service
    )
    {
      _logger = logger;
      _service = service;
    }

    /// <summary>
    /// Get
    /// </summary>
    /// <returns>Task IActionResult</returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
      // Summary:
      //     Indicates that the health check determined that the component was unhealthy,
      //     or an unhandled exception was thrown while executing the health check.
      // Unhealthy = 0,
      //
      // Summary:
      //     Indicates that the health check determined that the component was in a degraded
      //     state.
      // Degraded = 1,
      //
      // Summary:
      //     Indicates that the health check determined that the component was healthy.
      // Healthy = 2

      var report = await _service.CheckHealthAsync();

      _logger.LogInformation($"Get Health Information: {report}");

      return report.Status ==
        HealthStatus.Healthy
            ? Ok(report)
            : StatusCode((int)HttpStatusCode.ServiceUnavailable, report);
    }
  }

  /// <summary>
  /// ApiCommandsHealthChecks
  /// </summary>
  public class ApiCommandsHealthChecks : IHealthCheck
  {
    /// <summary>
    /// CheckHealthAsync 
    /// </summary>
    /// <param name="context">HealthCheckContext</param>
    /// <param name="cancellationToken">CancellationToken</param>
    /// <returns>Task HealthCheckResult</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default
    )
    {
      var catUrl = "https://localhost:7044/api/v1/commands";

      var client = new HttpClient();

      client.BaseAddress = new Uri(catUrl);

      HttpResponseMessage response = await client.GetAsync("");

      return response.StatusCode == HttpStatusCode.OK ?
          await Task.FromResult(new HealthCheckResult(
                status: HealthStatus.Healthy,
                description: "The API [api/v1/commands] is healthy 😃")) :
          await Task.FromResult(new HealthCheckResult(
                status: HealthStatus.Unhealthy,
                description: "The API [api/v1/commands] is sick 😒"));
    }
  }
}