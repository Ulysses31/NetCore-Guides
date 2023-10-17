

using System.Diagnostics.Metrics;
using System.Drawing;
using System.Net.Http.Json;
using Subscriber.Dtos;

do
{
    HttpClient httpClient = new HttpClient();

    Console.Title = "Subscriber";

    Console.WriteLine("Press ESC to stop");

    Console.WriteLine("Listening...");

    while (!Console.KeyAvailable)
    {
        List<int> ackIds = await GetMessagesAsync(httpClient);

        Thread.Sleep(2000);

        if (ackIds.Any())
        {
            await AckMessagesAsync(httpClient, ackIds);
        }
    }
} while (Console.ReadKey(true).Key != ConsoleKey.Escape);

static async Task<List<int>> GetMessagesAsync(HttpClient httpClient)
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

static async Task AckMessagesAsync(
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
    finally {
        Console.ResetColor();
    }

}
