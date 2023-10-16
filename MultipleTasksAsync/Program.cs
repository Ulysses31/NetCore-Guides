using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;
using MultipleTasksAsync;
using MultipleTasksAsync.Models;
using MultipleTasksAsync.Repository;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

// Endpoints
#region Endpoints
/// <summary>
/// api/v1/executeinsequence/{id}
/// (Execute Multiple Tasks in Sequence using async and await)
/// </summary>
/// <param name="id">Guid</param>
/// <returns>EmployeeProfile</returns>
app.MapGet("api/v1/execute-in-sequence/{id}", async (Guid id) =>
{
    var _employeeApiFacade = new EmployeeApiFacade();

    var employeeDetails = await _employeeApiFacade.GetEmployeeDetails(id);
    var employeeSalary = await _employeeApiFacade.GetEmployeeSalary(id);
    var employeeRating = await _employeeApiFacade.GetEmployeeRating(id);

    var employeeProfile = new EmployeeProfile(
        employeeDetails,
        employeeSalary,
        employeeRating
    );

    Console.WriteLine($"--> Result: ({employeeProfile})");

    return Results.Ok(employeeProfile);
}).AddEndpointFilter<TrackTimeFilter>();

/// <summary>
/// api/v1/executeinparallel/{id}
/// (Execute Multiple Tasks in Parallel using Task.WhenAll)
/// </summary>
/// <param name="id">Guid</param>
/// <returns>EmployeeProfile</returns>
app.MapGet("api/v1/execute-in-parallel/{id}", async (Guid id) =>
{
    var _employeeApiFacade = new EmployeeApiFacade();

    var employeeDetailsTask = _employeeApiFacade.GetEmployeeDetails(id);
    var employeeSalaryTask = _employeeApiFacade.GetEmployeeSalary(id);
    var employeeRatingTask = _employeeApiFacade.GetEmployeeRating(id);

    var (
            employeeDetails,
                      employeeSalary,
                          employeeRating
    ) = await MultiTaskExtensions.WhenAll(
        employeeDetailsTask,
        employeeSalaryTask,
        employeeRatingTask
    );

    var employeeProfile = new EmployeeProfile(
        employeeDetails,
        employeeSalary,
        employeeRating
    );

    Console.WriteLine($"--> Result: ({employeeProfile})");

    return Results.Ok(employeeProfile);
}).AddEndpointFilter<TrackTimeFilter>();


/// <summary>
/// api/v1/execute-using-normal-foreach
/// (Sequential Task Execution using async, await, and foreach Loop)
/// </summary>
/// <param name=""api/v1/execute-using-normal-foreach"">string</param>
/// <param name="request">HttpRequest</param>
/// <returns>EmployeeDetails</returns>
app.MapPost(
    "api/v1/execute-using-normal-foreach",
    async (HttpRequest request) =>
    {
        EmployeeApiFacade? _employeeApiFacade = new();
        List<EmployeeDetails> employeeDetails = new();

        IEnumerable<string>? employeeIds =
            await request.ReadFromJsonAsync<IEnumerable<string>>();

        foreach (var id in employeeIds!)
        {
            var guidId = new Guid(id);

            EmployeeDetails? employeeDetail =
                await _employeeApiFacade.GetEmployeeDetails(guidId);

            Console.WriteLine($"--> Result: ({JsonSerializer.Serialize(employeeDetail)})");

            if (employeeDetail != null) employeeDetails.Add(employeeDetail);
        }

        return employeeDetails;
    }).AddEndpointFilter<TrackTimeFilter>();

/// <summary>
/// api/v1/execute-using-parallel-foreach
/// (Parallel Tasks Execution using Parallel.Foreach)
/// </summary>
/// <param name=""api/v1/execute-using-parallel-foreach"">string</param>
/// <param name="request"></param>
/// <returns>ConcurrentBag<EmployeeDetails></returns>
app.MapPost(
    "api/v1/execute-using-parallel-foreach",
    async (HttpRequest request) =>
    {
        ParallelOptions parallelOptions = new()
        {
            MaxDegreeOfParallelism = 3
        };

        EmployeeApiFacade? _employeeApiFacade = new();
        ConcurrentBag<EmployeeDetails> employeeDetails = new();

        IEnumerable<string>? employeeIds =
            await request.ReadFromJsonAsync<IEnumerable<string>>();

        Parallel.ForEach(employeeIds!, parallelOptions, (id) =>
        {
            var guidId = new Guid(id);

            EmployeeDetails? employeeDetail =
                _employeeApiFacade.GetEmployeeDetails(guidId)
                .GetAwaiter()
                .GetResult();

            Console.WriteLine($"--> Result: ({JsonSerializer.Serialize(employeeDetail)})");

            if (employeeDetail != null) employeeDetails.Add(employeeDetail);
        });

        return employeeDetails;
    }).AddEndpointFilter<TrackTimeFilter>();

/// <summary>
/// api/v1/execute-using-parallel-foreach-async
/// (Parallel Tasks Execution using Parallel.ForeachAsync)
/// </summary>
/// <param name=""api/v1/execute-using-parallel-foreach-async"">string</param>
/// <param name="request">HttpRequest</param>
/// <returns> ConcurrentBag<EmployeeDetails></returns>
app.MapPost(
    "api/v1/execute-using-parallel-foreach-async",
    async (HttpRequest request) =>
    {
        ParallelOptions parallelOptions = new()
        {
            MaxDegreeOfParallelism = 3
        };

        EmployeeApiFacade? _employeeApiFacade = new();
        ConcurrentBag<EmployeeDetails> employeeDetails = new();

        IEnumerable<string>? employeeIds =
                await request.ReadFromJsonAsync<IEnumerable<string>>();

        await Parallel.ForEachAsync(
            employeeIds!,
            parallelOptions,
            async (id, _) =>
               {
                   var guidId = new Guid(id);

                   EmployeeDetails? employeeDetail =
                       await _employeeApiFacade.GetEmployeeDetails(guidId);

                   Console.WriteLine($"--> Result: ({JsonSerializer.Serialize(employeeDetail)})");

                   if (employeeDetail != null) employeeDetails.Add(employeeDetail);
               });

        return employeeDetails;
    }).AddEndpointFilter<TrackTimeFilter>();

// Details Endpoint
app.MapGet("api/v1/details/{id}", async (Guid id) =>
{
    return await Task.FromResult(Results.Ok(new
    {
        Id = id,
        Name = $"Sam_{id}",
        DateOfBirth = DateTime.Now.AddYears(-1 * new Random().Next(20, 30)).Date,
        Address = "Employee Dummy Address"
    }));
});

// Salary Endpoint
app.MapGet("api/v1/salary/{id}", async (Guid id) =>
{
    return await Task.FromResult(Results.Ok(new
    {
        Id = id,
        SalaryInEuro = 25000
    }));
});

// Rating Endpoint
app.MapGet("api/v1/rating/{id}", async (Guid id) =>
{
    return await Task.FromResult(Results.Ok(new
    {
        Id = id,
        Rating = 4
    }));
});
#endregion



app.Run();
