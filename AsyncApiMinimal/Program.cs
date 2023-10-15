using System.Security.Cryptography;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using AsyncApiMinimal.Data;
using AsyncApiMinimal.Dtos;
using AsyncApiMinimal.Helpers;
using AsyncApiMinimal.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlite("Data Source=RequestDB.db"));


var app = builder.Build();

app.UseHttpsRedirection();


// Endpoints


#region Domain Users
// Create Domain USers
app.MapPost(
    "api/v1/domainusers",
    () =>
    {
        BatchProcesses b = StartBatch();

        Task.Run(async () =>
        {
            try
            {
                Console.WriteLine("Begin to add domain users...");

                await AddBatchProcessItem(b.RequestId, "Starting...");
                await ChangeStatusBatch(b.RequestId, StatusEnum.PENDING);

                var scope = app.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                for (int i = 0; i < 10000; i++)
                {
                    var user = new DomainUsers()
                    {
                        Name = $"NewUser_{i}",
                        UserName = $"username_{i}",
                        Email = $"newuser_{i}@domain.com"
                    };

                    await context!.DomainUsers.AddAsync(user);
                    Console.WriteLine($"User {user.Name} added..");
                }
                var results = await context!.SaveChangesAsync();

                // var j = JsonConvert.SerializeObject(context!.DomainUsers.ToList());

                if (results > 0)
                {
                    await AddBatchProcessItem(b.RequestId, "Domain Users added...");
                    await ChangeStatusBatch(b.RequestId, StatusEnum.COMPLETED);
                }
            }
            catch (System.Exception e)
            {
                await AddBatchProcessItem(b.RequestId, $"Failed... {e.Message} - {e.InnerException!.Message}");
                await ChangeStatusBatch(b.RequestId, StatusEnum.FAILED);
                Console.WriteLine($"--> {e.Message} - {e.InnerException!.Message}");
                return Results.Problem(new ProblemDetails()
                {
                    Title = "Internal Server Error",
                    Detail = $"{e.Message} - ${e.InnerException!.Message}",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Instance = "api/v1/domainusers"
                });
            }

            await Task.Delay(2000);
            return Results.Empty;

        });

        return Results.Ok(b);
    }
);
#endregion

#region Batch_Process_EndPoints
// Start Endpoint
app.MapPost(
    "api/v1/batchprocess/start",
    async (HttpContext httpContext, AppDbContext context, BatchProcesses batchProcesses) =>
    {
        if (batchProcesses == null) return Results.BadRequest();

        batchProcesses.RequestStatus = StatusEnum.ACCEPT;
        batchProcesses.EstimateCompetionTime = "2023-02-06:14:00:00";
        batchProcesses.RequestStatusUrl =
            $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}/api/v1/batchprocess/status/{batchProcesses.RequestId}";

        await context.BatchProcesses.AddAsync(batchProcesses);
        await context.SaveChangesAsync();

        return Results.Accepted(
            $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}/api/v1/batchprocess/status/{batchProcesses.RequestId}", batchProcesses
        );
    }
);

// Change Status EndPoint
app.MapGet(
    "api/v1/batchprocess/status/change/{requestid}/{status}",
    async (HttpContext httpContext, AppDbContext context, string requestId, string status) =>
    {
        if (string.IsNullOrEmpty(requestId) || string.IsNullOrEmpty(status))
            return Results.BadRequest();

        var batchProcess =
            await context.BatchProcesses.FirstOrDefaultAsync(
                l => l.RequestId == requestId
            );

        if (batchProcess == null) return Results.NotFound();

        batchProcess.RequestStatus = status.ToUpper();
        batchProcess.RequestBody = $"RequestId: [{requestId}] - Status: [{status}]";

        await Task.Run(() => context.BatchProcesses.Update(batchProcess));
        await context.SaveChangesAsync();

        if (status.ToUpper() == StatusEnum.COMPLETED)
        {
            await AddBatchProcessItem(requestId, "Finished...");
        }

        return Results.Ok();
    }
);

// Get Status Endpoint
app.MapGet(
    "api/v1/batchprocess/status/{requestid}",
    (HttpContext httpContext, AppDbContext context, string requestId) =>
    {
        var batchProcess =
            context.BatchProcesses.FirstOrDefault(
                l => l.RequestId == requestId
            );

        if (batchProcess == null) return Results.NotFound();

        BatchProcessStatus batchProcessStatus = new BatchProcessStatus()
        {
            RequestStatus = batchProcess.RequestStatus,
            ResourceUrl = String.Empty,
            RequestId = requestId
        };

        if (batchProcess.RequestStatus!.ToUpper() == StatusEnum.COMPLETED)
        {
            batchProcessStatus.ResourceUrl =
                $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}/api/v1/batchprocess/{Guid.NewGuid().ToString()}";
            //return Results.Ok(listingStatus);

            // auto redirect to final endpoint
            return Results.Redirect(
                $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}/{batchProcessStatus.ResourceUrl}"
            );
        }

        batchProcessStatus.EstimatedCompetionTime = "2023-02-06:15:00:00";

        return Results.Ok(batchProcessStatus);
    }
);

// Final Endpoint
app.MapGet("api/v1/batchprocess/{requestId}", (string requestId) =>
{
    return Results.Ok("This is where you would pass back the final result.");
});

// Batch Process Add Items 
app.MapPost(
    "api/v1/batchprocess/items",
    async (
        HttpContext httpContext,
        AppDbContext context,
        BatchProcessItems batchProcessItems
    ) =>
    {
        if (batchProcessItems == null) return Results.BadRequest();

        await context.BatchProcessItems.AddAsync(batchProcessItems);
        await context.SaveChangesAsync();

        return Results.Ok();
    }
);
#endregion




static BatchProcesses StartBatch()
{
    Console.WriteLine("--> Starting Batch Process...");

    // var scope = app.Services.CreateScope();
    // var client = scope.ServiceProvider.GetRequiredService<HttpClient>();

    var client = new HttpClient();

    string json = JsonConvert.SerializeObject(
      new BatchProcesses() { RequestBody = "Batch process start" }
    );

    StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

    var result = client.PostAsync(
      "https://localhost:7286/api/v1/batchprocess/start",
      httpContent
    )
    .GetAwaiter()
    .GetResult();

    var msg = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

    Console.WriteLine(msg);

    var b = JsonConvert.DeserializeObject<BatchProcesses>(msg);

    return b!;
};

static async Task ChangeStatusBatch(string? requestId, string? status)
{
    Console.WriteLine($"--> Changing Batch Process [{requestId}] Status to {status}...");

    var client = new HttpClient();

    var result = await client.GetAsync(
      $"https://localhost:7286/api/v1/batchprocess/status/change/{requestId}/{status}"
    );

    var msg = await result.Content.ReadAsStringAsync();

    Console.WriteLine(msg);
};

// static void GetStatusBatch() { };

static async Task AddBatchProcessItem(string? requestId, string? message)
{
    Console.WriteLine($"--> Add Batch Process Item with Batch Process RequestId [{requestId}]...");

    var client = new HttpClient();

    BatchProcessItems batchProcessItems = new BatchProcessItems()
    {
        RequestId = requestId,
        ItemMessage = message
    };

    HttpContent body = new StringContent(
        JsonConvert.SerializeObject(batchProcessItems),
        Encoding.UTF8,
        "application/json"
    );

    var result = await client.PostAsync(
      $"https://localhost:7286/api/v1/batchprocess/items",
      body
    );

    var msg = await result.Content.ReadAsStringAsync();

    Console.WriteLine(msg);
}

app.Run();

