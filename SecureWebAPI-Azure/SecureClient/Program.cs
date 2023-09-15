// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

using System.Net.Http.Headers;
using Microsoft.Identity.Client;
using SecureClient;

internal class Program
{
  private static void Main()
  {
    AuthConfig config = AuthConfig.ReadFromJsonFile("appsettings.json");

    Console.WriteLine($"Authority: {config.Authority}");

    Console.WriteLine("Making the call...");

    RunAsync().GetAwaiter().GetResult();

  }

  private static async Task RunAsync()
  {
    AuthConfig config = AuthConfig.ReadFromJsonFile("appsettings.json");

    IConfidentialClientApplication app
      = ConfidentialClientApplicationBuilder.Create(config.ClientId)
        .WithClientSecret(config.ClientSecret)
        .WithAuthority(new Uri(config.Authority))
        .Build();

    string[] ResourceIds = new string[] { config.ResourceID };

    AuthenticationResult result = null;

    try
    {
      result = await app.AcquireTokenForClient(ResourceIds).ExecuteAsync();
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("Token acquired \n");
      Console.WriteLine($"[TokenType]: {result.TokenType}");
      Console.WriteLine($"[AccessToken]: {result.AccessToken}");
      Console.WriteLine($"[Expires]: {result.ExpiresOn}");

      Console.ResetColor();
    }
    catch (MsalClientException ex)
    {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine(ex.Message);
      Console.ResetColor();
    }

    if (!string.IsNullOrEmpty(result.AccessToken))
    {
      var httpClient = new HttpClient();
      var defaultRequestHeaders = httpClient.DefaultRequestHeaders;

      if (defaultRequestHeaders.Accept == null ||
         !defaultRequestHeaders.Accept.Any(
          m => m.MediaType == "application/json"))
      {
        httpClient.DefaultRequestHeaders.Accept.Add(new
          MediaTypeWithQualityHeaderValue("application/json"));
      }
      defaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("bearer", result.AccessToken);

      HttpResponseMessage response = await httpClient.GetAsync(config.BaseAddress);
      if (response.IsSuccessStatusCode)
      {
        Console.ForegroundColor = ConsoleColor.Green;
        string json = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"\n {json}");
      }
      else
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\n Failed to call the Web Api: {response.StatusCode}");
        string content = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Content: {content}");
      }
      Console.ResetColor();
    }
  }
}