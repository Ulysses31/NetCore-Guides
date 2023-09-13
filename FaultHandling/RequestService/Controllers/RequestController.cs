using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RequestService.Policies;

namespace RequestService.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class RequestController : ControllerBase
  {
    private readonly IHttpClientFactory _clientFactory;
    // private readonly ClientPolicy _clientPolicy;

    public RequestController(
      IHttpClientFactory clientFactory
    // ClientPolicy clientPolicy
    )
    {
      _clientFactory = clientFactory;
      // _clientPolicy = clientPolicy;
    }

    // GET api/request

    [HttpGet]
    public async Task<ActionResult> MakeRequest()
    {
      //var client = new HttpClient();

      HttpClient client = _clientFactory.CreateClient("TestClient");

      HttpResponseMessage response
        = await client.GetAsync(client.BaseAddress.ToString());

      // Execute using immediate policy
      // var response = await _clientPolicy.ImmediateHttpRetry.ExecuteAsync(
      //   () => client.GetAsync("https://localhost:7100/api/response/25")
      // );

      // Execute using every 3 seconds delay policy
      // var response = await _clientPolicy.LinearHttpRetry.ExecuteAsync(
      //   () => client.GetAsync("https://localhost:7100/api/response/25")
      // );

      // Execute using random seconds exponential delay policy
      // var response = await _clientPolicy.ExponentialHttpRetry.ExecuteAsync(
      //   () => client.GetAsync("https://localhost:7100/api/response/25")
      // );

      if (response.IsSuccessStatusCode)
      {
        Console.WriteLine("--> ResponseService returned Success!");
        return Ok();
      }

      Console.WriteLine("--> ResponseService returned Failure!");
      return StatusCode(StatusCodes.Status500InternalServerError);
    }
  }
}