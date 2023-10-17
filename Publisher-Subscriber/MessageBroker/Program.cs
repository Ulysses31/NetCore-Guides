using MessageBroker;
using MessageBroker.Data;
using MessageBroker.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source=MessageBus.db")
        .EnableDetailedErrors()
        .EnableSensitiveDataLogging();
});

var app = builder.Build();

app.UseHttpsRedirection();


// Endpoints

/// <summary>
/// Get all topics
/// </summary>
/// <param name="_context">AppDbContext</param>
/// <returns>List of Topic</returns>
app.MapGet("api/v1/topics", async (AppDbContext _context) =>
{
    try
    {
        var topics = await _context.Topics.ToListAsync();

        return Results.Ok(topics);
    }
    catch (System.Exception ex)
    {
        Console.WriteLine($"--> {ex}");
        throw;
    }
});

/// <summary>
/// Create topic
/// </summary>
/// <param name=""api/v1/topics"">string</param>
/// <param name="_context">AppDbContext</param>
/// <param name="topic">Topic</param>
/// <returns>Created</returns>
app.MapPost(
    "api/v1/topics",
    async (AppDbContext _context, Topic topic) =>
    {
        if (topic == null) return Results.BadRequest();

        try
        {
            await _context.Topics.AddAsync(topic);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"--> {ex}");
            throw;
        }

        return Results.Created($"api/v1/topics/{topic.Id}", topic);
    });

/// <summary>
/// Publish Mesage
/// </summary>
/// <value>string</value>
app.MapPost(
    "api/v1/topics/{id}/messages",
    async (AppDbContext _context, int? id, Message message) =>
    {
        if (id == null || message == null) return Results.BadRequest();

        try
        {
            bool topics = await _context.Topics.AnyAsync(t => t.Id == id);

            if (!topics) return Results.NotFound("Topic not found");

            var subs =
                _context.Subscriptions.Where(s => s.TopicId == id);

            if (!subs.Any())
                return Results.NotFound("There are no subscriptions for this topic");

            foreach (var sub in subs)
            {
                Message msg = new Message()
                {
                    TopicMessage = message.TopicMessage,
                    SubscriptionId = sub.Id,
                    ExpiresAfter = message.ExpiresAfter,
                    MessageStatus = message.MessageStatus
                };

                await _context.Messages.AddAsync(msg);
            }

            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"--> {ex}");
            throw;
        }

        return Results.Ok("Message has been published");
    });

/// <summary>
/// Create subscription
/// </summary>
/// <value>Subscription</value>
app.MapPost(
    "api/v1/topics/{id}/subscriptions",
    async (AppDbContext _context, int? id, Subscription subscription) =>
    {
        if (id == null || subscription == null) return Results.BadRequest();

        try
        {
            bool topics = await _context.Topics.AnyAsync(t => t.Id == id);

            if (!topics) return Results.NotFound("Topic not found");

            subscription.TopicId = id;

            await _context.Subscriptions.AddAsync(subscription);
            await _context.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"--> {ex}");
            throw;
        }

        return Results.Created(
            $"api/v1/topics/{id}/subscriptions/{subscription.Id}",
            subscription
        );
    });

/// <summary>
/// Get Subscriber Messages
/// </summary>
/// <value>Messages</value>
app.MapGet(
    "api/v1/subscriptions/{id}/messages",
    async (AppDbContext _context, int? id) =>
    {
        if (id == null) return Results.BadRequest();

        try
        {
            bool subscriptions =
                await _context.Subscriptions.AnyAsync(s => s.Id == id);

            if (!subscriptions) return Results.NotFound("Subscription not found");

            var messages =
                _context.Messages.Where(
                    m => m.SubscriptionId == id 
                        && m.MessageStatus != MessageStatusOptions.SENT
                );

            if (!messages.Any()) return Results.NotFound("No new messages");

            foreach (var msg in messages)
            {
                msg.MessageStatus = MessageStatusOptions.REQUESTED;
            }

            await _context.SaveChangesAsync();

            return Results.Ok(messages);
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"--> {ex}");
            throw;
        }
    });


/// <summary>
/// Status change for completed messages
/// </summary>
/// <value>string</value>
app.MapPost(
    "api/v1/subscriptions/{id}/messages",
    async (AppDbContext _context, int? id, int[] confs) =>
    {
        if (id == null || !confs.Any()) return Results.BadRequest();

        try
        {
            bool subscriptions =
              await _context.Subscriptions.AnyAsync(s => s.Id == id);

            if (!subscriptions) return Results.NotFound("Subscription not found");

            int vCnt = 0;
            foreach (var i in confs)
            {
                var msg = _context.Messages.FirstOrDefault(m => m.Id == i);

                if (msg != null)
                {
                    msg.MessageStatus = MessageStatusOptions.SENT;
                    await _context.SaveChangesAsync();
                    vCnt++;
                }
            }

            return Results.Ok($"Status changes for {vCnt} of {confs.Length} messages");
        }
        catch (System.Exception ex)
        {
            Console.WriteLine($"--> {ex}");
            throw;
        }
    });




app.Run();

