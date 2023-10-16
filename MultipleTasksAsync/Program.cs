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
app.MapGet("api/v1/executeinsequence/{id}", async (Guid id) =>
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

    Console.WriteLine($"--> Rsult: ({employeeProfile})");

    return Results.Ok(employeeProfile);
}).AddEndpointFilter<TrackTimeFilter>();

/// <summary>
/// api/v1/executeinparallel/{id}
/// (Execute Multiple Tasks in Parallel using Task.WhenAll)
/// </summary>
/// <param name="id">Guid</param>
/// <returns>EmployeeProfile</returns>
app.MapGet("api/v1/executeinparallel/{id}", async (Guid id) =>
{
    var logFactory = app.Services.GetService<ILoggerFactory>();

    var _employeeApiFacade = new EmployeeApiFacade();

    var employeeDetailsTask = _employeeApiFacade.GetEmployeeDetails(id);
    var employeeSalaryTask = _employeeApiFacade.GetEmployeeSalary(id);
    var employeeRatingTask = _employeeApiFacade.GetEmployeeRating(id);

    var (
            employeeDetails,
                      employeeSalary,
                          employeeRating
    ) = await new MultiTaskExtensions(logFactory!).WhenAll(
        employeeDetailsTask,
        employeeSalaryTask,
        employeeRatingTask
    );

    var employeeProfile = new EmployeeProfile(
        employeeDetails,
        employeeSalary,
        employeeRating
    );

    Console.WriteLine($"--> Rsult: ({employeeProfile})");

    return Results.Ok(employeeProfile);
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
