using System.Collections.Concurrent;
using MultipleTasksAsync;
using MultipleTasksAsync.Models;
using MultipleTasksAsync.Repository;

namespace ApiBenchmarkDotNet
{
    /// <summary>
    /// Executor Repository
    /// </summary>
    public class Executor
    {
        private readonly IEmployeeApiFacade _employeeApiFacade;

        /// <summary>
        /// Executor Constructor
        /// </summary>
        /// <param name="employeeApiFacade">IEmployeeApiFacade</param>
        public Executor(IEmployeeApiFacade employeeApiFacade)
        {
            _employeeApiFacade = employeeApiFacade;
        }

        /// <summary>
        /// ExecuteInSequence function
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns>Task<EmployeeProfile></returns>
        public async Task<EmployeeProfile> ExecuteInSequence(Guid id)
        {
            var employeeDetails = await _employeeApiFacade.GetEmployeeDetails(id);
            var employeeSalary = await _employeeApiFacade.GetEmployeeSalary(id);
            var employeeRating = await _employeeApiFacade.GetEmployeeRating(id);

            var employeeProfile = new EmployeeProfile(
                  employeeDetails,
                  employeeSalary,
                  employeeRating
              );

            return employeeProfile;
        }

        /// <summary>
        /// ExecuteInParallel function
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns>Task<EmployeeProfile></returns>
        public async Task<EmployeeProfile> ExecuteInParallel(Guid id)
        {
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

            return employeeProfile;
        }

        /// <summary>
        /// ExecuteUsingNormalForEach function
        /// </summary>
        /// <param name="employeeIds">IEnumerable<Guid></param>
        /// <returns>Task<IEnumerable<EmployeeDetails>></returns>
        public async Task<IEnumerable<EmployeeDetails>> ExecuteUsingNormalForEach(
            IEnumerable<Guid> employeeIds
        )
        {
            List<EmployeeDetails> employeeDetails = new();

            foreach (var id in employeeIds)
            {
                var employeeDetail =
                    await _employeeApiFacade.GetEmployeeDetails(id);

                employeeDetails.Add(employeeDetail);
            }

            return employeeDetails;
        }

        /// <summary>
        /// ExecuteUsingParallelForeach function
        /// </summary>
        /// <param name="employeeIds">IEnumerable<Guid></param>
        /// <returns>IEnumerable<EmployeeDetails></returns>
        public IEnumerable<EmployeeDetails> ExecuteUsingParallelForeach(
            IEnumerable<Guid> employeeIds
        )
        {
            ParallelOptions parallelOptions = new()
            {
                MaxDegreeOfParallelism = 3
            };

            ConcurrentBag<EmployeeDetails> employeeDetails = new();

            Parallel.ForEach(employeeIds, parallelOptions, id =>
            {
                var employeeDetail =
                    _employeeApiFacade.GetEmployeeDetails(id).GetAwaiter().GetResult();

                employeeDetails.Add(employeeDetail);
            });

            return employeeDetails;
        }

        /// <summary>
        /// ExecuteUsingParallelForeachAsync function
        /// </summary>
        /// <param name="employeeIds">IEnumerable<Guid></param>
        /// <returns>Task<IEnumerable<EmployeeDetails>></returns>
        public async Task<IEnumerable<EmployeeDetails>> ExecuteUsingParallelForeachAsync(
            IEnumerable<Guid> employeeIds
        )
        {
            ParallelOptions parallelOptions = new()
            {
                MaxDegreeOfParallelism = 3
            };

            ConcurrentBag<EmployeeDetails> employeeDetails = new();

            await Parallel.ForEachAsync(employeeIds, parallelOptions, async (id, _) =>
            {
                var employeeDetail =
                    await _employeeApiFacade.GetEmployeeDetails(id);

                employeeDetails.Add(employeeDetail);
            });

            return employeeDetails;
        }

    }
}
