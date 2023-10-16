using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MultipleTasksAsync.Models;

namespace MultipleTasksAsync.Repository
{
    public interface IEmployeeApiFacade
    {
        public Task<EmployeeDetails> GetEmployeeDetails(Guid id);

        public Task<decimal> GetEmployeeSalary(Guid id);

        public Task<int> GetEmployeeRating(Guid id);
    }
}
