using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace MultipleTasksAsync
{
    public class TrackTimeFilter : IEndpointFilter
    {
        protected readonly ILogger Logger;
        private readonly string _methodName;

        public TrackTimeFilter(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<TrackTimeFilter>();
            _methodName = GetType().Name;
        }

        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context,
            EndpointFilterDelegate next
        )
        {
            Stopwatch stopWatch = new();
            stopWatch.Start();

            var result = await next(context);

            stopWatch.Stop();

            Logger.LogInformation(
                $"--> Execution Time: {stopWatch.ElapsedMilliseconds} ms",
                _methodName
            );

            return result;
        }
    }
}
