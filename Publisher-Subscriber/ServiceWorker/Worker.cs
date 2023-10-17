using System.Globalization;
using System.Net.Http.Json;
using ServiceWorker.Dtos;

namespace ServiceWorker;

public class Worker : BackgroundService
{
    public readonly ILogger<Worker> _logger;
    private readonly HttpClient _httpClient;

    public Worker(
        ILogger<Worker> logger,
        HttpClient httpClient
    )
    {
        _logger = logger;
        _httpClient = httpClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.Title = "Subscriber Background Service";

        // Background Service will sync every 5 seconds
        TimeSpan interval = TimeSpan.FromSeconds(5);
        using PeriodicTimer timer = new PeriodicTimer(interval);

        _logger.LogInformation(
            "Worker running and listening at: {time} sync every {seconds} seconds...", 
            DateTimeOffset.Now, 
            interval
        );

        while (!stoppingToken.IsCancellationRequested && 
            await timer.WaitForNextTickAsync(stoppingToken)
        )
        {
            Console.WriteLine($"--> Last sync at {DateTime.Now}...");
           
            List<int> ackIds = await GetMessagesAsync(_httpClient);

            await Task.Delay(2000, stoppingToken);

            if (ackIds.Any())
            {
                await AckMessagesAsync(_httpClient, ackIds);
            }
        }
    }

    private static async Task<List<int>> GetMessagesAsync(HttpClient httpClient)
    {
        List<int> ackIds = new List<int>();

        try
        {
            List<MessageReadDto>? newMessages =
                await httpClient.GetFromJsonAsync<List<MessageReadDto>>(
                    "https://localhost:7112/api/v1/subscriptions/2/messages"
                );

            if (!newMessages!.Any()) return ackIds;

            foreach (var msg in newMessages!)
            {
                Thread.Sleep(250);

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[{DateTime.Now}] {msg.Id} - {msg.TopicMessage} - {msg.MessageStatus}");

                ackIds.Add(msg.Id);
            }
        }
        catch (System.Exception)
        {
            // There are no more messages so return 0
            return ackIds;
        }
        finally
        {
            Console.ResetColor();
        }

        return ackIds;
    }

    private static async Task AckMessagesAsync(
        HttpClient httpClient,
        List<int> ackIds
    )
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync(
                "https://localhost:7112/api/v1/subscriptions/2/messages",
                ackIds
            );

            var returnMessage = await response.Content.ReadAsStringAsync();

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($"--> {returnMessage}\n");
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"--> {ex}");
            throw;
        }
        finally
        {
            Console.ResetColor();
        }

    }

}
