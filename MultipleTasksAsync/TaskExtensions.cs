using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultipleTasksAsync
{
    public class MultiTaskExtensions
    {
        protected readonly ILogger? Logger;
        private readonly ILoggerFactory? _loggerFactory;
        private readonly string _methodName;

        public MultiTaskExtensions(ILoggerFactory loggerFactory)
        {
            _methodName = GetType().Name;
            _loggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger<MultiTaskExtensions>();
        }

        public async Task<(T1, T2, T3)> WhenAll<T1, T2, T3>(Task<T1> task1, Task<T2> task2, Task<T3> task3)
        {
            var allTasks = Task.WhenAll(task1, task2, task3);

            try
            {
                await allTasks;
            }
            catch (Exception exp)
            {
                Logger!.LogError("Task Exception", exp);

                throw allTasks.Exception!;
            }

            return (task1.Result, task2.Result, task3.Result);
        }
    }
}
