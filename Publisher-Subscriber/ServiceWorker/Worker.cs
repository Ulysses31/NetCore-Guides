using System.Globalization;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using MailKit.Net.Smtp;
using MimeKit;
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
        List<Task> mailTasks = new List<Task>();

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

            // Prepare to send emails
            foreach (var msg in newMessages!)
            {
                var newMailTask = SendEmailMessage($"[{DateTime.Now}] {msg.Id} - {msg.TopicMessage} - {msg.MessageStatus}");
                mailTasks.Add(newMailTask);
            }

            // Send emails
            if (mailTasks.Any())
            {
                Thread.Sleep(250);
                Task.WhenAll(mailTasks).Start();
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

    private static async Task SendEmailMessage(string? messageBody)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"--> [{DateTime.Now}] Sending email...");

        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Subscriber Background Service", "test@mailtrap.com"));
            message.To.Add(new MailboxAddress("Mrs. Iordanidis Chris", "iordanidischr@gmail.com"));
            message.Subject = $"Subscriber - {messageBody}";

            message.Body = new TextPart("html")
            {
                Text = $"<p>{messageBody}</p>"
            };

            using var client = new SmtpClient();
            // await client.ConnectAsync("smtp.gmail.com", 587, false);
            // await client.ConnectAsync("smtp.office365.com", 587, false);
            // MailTrap
            await client.ConnectAsync("sandbox.smtp.mailtrap.io", 2525, false);

            // Note: only needed if the SMTP server requires authentication
            // await client.AuthenticateAsync("iordanidischr@gmail.com", "lneafouykxxsfrqs");
            //await client.AuthenticateAsync("nazgoul78@hotmail.com", "lneafouykxxsfrqs");
            // MailTrap
            await client.AuthenticateAsync("bed5a2f0e0600a", "f849cb11459b84");

            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            Thread.Sleep(250);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"--> [{DateTime.Now}] Sending email succedded...");
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"--> {ex.Message}");
        }
        finally
        {
            Console.ResetColor();
        }
    }
}
