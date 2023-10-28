using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommonServiceCollection.CommonRateLimit
{
    /// <summary>
    /// CommonRateLimitExtension class
    /// </summary>
    public static class CommonRateLimitExtension
    {
        public const string? FixedPolicy = "fixed";
        public const string? TokenPolicy = "token";

        /// <summary>
        /// CommonRateLimitSetup function
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <returns>IServiceCollection</returns>
        public static IServiceCollection CommonRateLimitSetup(
            this IServiceCollection services
        )
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            var rateLimitOptions = new CommonRateLimitOptions();
            services.Configure<CommonRateLimitOptions>(
                 configuration.GetSection(CommonRateLimitOptions.MyRateLimit)
            );
            configuration.GetSection(CommonRateLimitOptions.MyRateLimit).Bind(rateLimitOptions);
            services.AddRateLimiter(opt =>
            {
                opt.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                opt.AddFixedWindowLimiter(FixedPolicy!, options =>
                {
                    options.PermitLimit = rateLimitOptions.FixedWindowLimiter!.PermitLimit;               //2
                    options.Window = TimeSpan.FromSeconds(rateLimitOptions.FixedWindowLimiter.Window);    //5s
                    options.QueueProcessingOrder = QueueProcessingOrder.NewestFirst;
                    options.QueueLimit = rateLimitOptions.FixedWindowLimiter.QueueLimit;                  //5
                });
                opt.AddTokenBucketLimiter(policyName: TokenPolicy!, options =>
                {
                    options.TokenLimit = rateLimitOptions.TokenBucketLimiter!.TokenLimit;
                    options.QueueProcessingOrder = QueueProcessingOrder.NewestFirst;
                    options.QueueLimit = rateLimitOptions.TokenBucketLimiter.QueueLimit;
                    options.ReplenishmentPeriod = TimeSpan.FromSeconds(rateLimitOptions.TokenBucketLimiter.ReplenishmentPeriod);
                    options.TokensPerPeriod = rateLimitOptions.TokenBucketLimiter.TokensPerPeriod;
                    options.AutoReplenishment = rateLimitOptions.TokenBucketLimiter.AutoReplenishment;
                });
            });

            return services;
        }
    }
}