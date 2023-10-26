using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalOpenApiExample.RateLimitConfig
{
    /// <summary>
    /// MyRateLimitOptions class
    /// </summary>
    public class MyRateLimitOptions
    {
        /// <summary>
        /// MyRateLimit
        /// </summary>
        public const string MyRateLimit = "RateLimitSettings";

        /// <summary>
        /// FixedWindowLimiter class
        /// </summary>
        public FixedWindowLimiter FixedWindowLimiter { get; set; }

        /// <summary>
        /// TokenBucketLimiter class
        /// </summary>
        public TokenBucketLimiter TokenBucketLimiter { get; set; } 
    }

    /// <summary>
    /// MasterLimiter class
    /// </summary>
    public class MasterLimiter {
        /// <summary>
        /// PermitLimit
        /// </summary>
        /// <value>int</value>
        public int PermitLimit { get; set; }

        /// <summary>
        /// Window
        /// </summary>
        /// <value>int</value>
        public int Window { get; set; }

        /// <summary>
        /// ReplenishmentPeriod
        /// </summary>
        /// <value>int</value>
        public int ReplenishmentPeriod { get; set; }

        /// <summary>
        /// QueueLimit
        /// </summary>
        /// <value>int</value>
        public int QueueLimit { get; set; }

        /// <summary>
        /// SegmentsPerWindow
        /// </summary>
        /// <value>int</value>
        public int SegmentsPerWindow { get; set; }

        /// <summary>
        /// TokenLimit
        /// </summary>
        /// <value>int</value>
        public int TokenLimit { get; set; }

        /// <summary>
        /// TokenLimit2
        /// </summary>
        /// <value>int</value>
        public int TokenLimit2 { get; set; }

        /// <summary>
        /// TokensPerPeriod
        /// </summary>
        /// <value>int</value>
        public int TokensPerPeriod { get; set; }

        /// <summary>
        /// AutoReplenishment
        /// </summary>
        /// <value>bool</value>
        public bool AutoReplenishment { get; set; }
    }

    /// <summary>
    /// FixedWindowLimiter class
    /// </summary>
    public class FixedWindowLimiter : MasterLimiter {}

    /// <summary>
    /// TokenBucketLimiter class
    /// </summary>
    public class TokenBucketLimiter : MasterLimiter {}
}