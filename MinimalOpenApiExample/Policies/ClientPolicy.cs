using Polly;
using Polly.Retry;

namespace MinimalOpenApiExample.Policies
{
    /// <summary>
    /// ClientPolicy class
    /// </summary>
    public class ClientPolicy
    {
        /// <summary>
        /// ImmediateHttpRetry
        /// </summary>
        /// <value>AsyncRetryPolicy of HttpResponseMessage</value>
        public AsyncRetryPolicy<HttpResponseMessage> ImmediateHttpRetry { get; }

        /// <summary>
        /// LinearHttpRetry
        /// </summary>
        /// <value>AsyncRetryPolicy HttpResponseMessage</value>
        public AsyncRetryPolicy<HttpResponseMessage> LinearHttpRetry { get; }

        /// <summary>
        /// ExponentialHttpRetry
        /// </summary>
        /// <value>AsyncRetryPolicy HttpResponseMessage</value>
        public AsyncRetryPolicy<HttpResponseMessage> ExponentialHttpRetry { get; }

        /// <summary>
        /// ClientPolicy constructor
        /// </summary>
        public ClientPolicy()
        {
            // Retry Policy (5 times) if success status code is failure        
            ImmediateHttpRetry = Policy.HandleResult<HttpResponseMessage>(
                res => !res.IsSuccessStatusCode
            ).RetryAsync(5);

            // Retry Policy (5 times) every 3 seconds if success status code is failure        
            LinearHttpRetry = Policy.HandleResult<HttpResponseMessage>(
                res => !res.IsSuccessStatusCode
            ).WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(3));

            // Retry Policy (5 times) randomly exponential if success status code is failure        
            ExponentialHttpRetry = Policy.HandleResult<HttpResponseMessage>(
                res => !res.IsSuccessStatusCode
            ).WaitAndRetryAsync(
              5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            );
        }
    }
}