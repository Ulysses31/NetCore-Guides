using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using MultipleTasksAsync.Models;

namespace MultipleTasksAsync.Repository
{
    public class EmployeeApiFacade : IEmployeeApiFacade
    {
        private static HttpClient _httpClient => new();
        private readonly JsonSerializerOptions _serializerOptions;

        public EmployeeApiFacade()
        {
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<EmployeeDetails> GetEmployeeDetails(Guid id)
        {
            Console.WriteLine($"--> GetEmployeeDetails called...");

            var response = await _httpClient.GetStringAsync($"https://localhost:7172/api/v1/details/{id}");
            var employeeDetails = JsonSerializer.Deserialize<EmployeeDetails>(response, _serializerOptions);

            // await Task.Delay(2000);
            // throw new Exception("--> GetEmployeeDetails Exception...");

            await Task.Delay(10);
            return employeeDetails!;
        }

        public async Task<int> GetEmployeeRating(Guid id)
        {
            Console.WriteLine($"--> GetEmployeeRating called...");

            var response = await _httpClient.GetStringAsync($"https://localhost:7172/api/v1/rating/{id}");
            var rating = JsonSerializer.Deserialize<AppraisalRating>(response, _serializerOptions);

            await Task.Delay(10);
            return rating!.Rating;
        }

        public async Task<decimal> GetEmployeeSalary(Guid id)
        {
            Console.WriteLine($"--> GetEmployeeSalary called...");

            var response = await _httpClient.GetStringAsync($"https://localhost:7172/api/v1/salary/{id}");
            var salary = JsonSerializer.Deserialize<Salary>(response, _serializerOptions);

            // await Task.Delay(2000);
            // throw new Exception("--> GetEmployeeSalary Exception...");

            await Task.Delay(10);
            return salary!.SalaryInEuro;
        }
    }
}
