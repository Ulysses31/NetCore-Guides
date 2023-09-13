using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;

namespace RequestService.Policies
{
  public class ClientPolicy
  {

    public AsyncRetryPolicy<HttpResponseMessage> ImmediateHttpRetry { get; }
    public AsyncRetryPolicy<HttpResponseMessage> LinearHttpRetry { get; }
    public AsyncRetryPolicy<HttpResponseMessage> ExponentialHttpRetry { get; }

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