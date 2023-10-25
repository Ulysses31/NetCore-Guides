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
        public const string MyRateLimit = "MyRateLimit";

        /// <summary>
        /// PermitLimit
        /// </summary>
        /// <value>int</value>
        public int PermitLimit { get; set; } = 2;

        /// <summary>
        /// Window
        /// </summary>
        /// <value>int</value>
        public int Window { get; set; } = 5;

        /// <summary>
        /// ReplenishmentPeriod
        /// </summary>
        /// <value>int</value>
        public int ReplenishmentPeriod { get; set; } = 10;

        /// <summary>
        /// QueueLimit
        /// </summary>
        /// <value>int</value>
        public int QueueLimit { get; set; } = 0;

        /// <summary>
        /// SegmentsPerWindow
        /// </summary>
        /// <value>int</value>
        public int SegmentsPerWindow { get; set; } = 8;

        /// <summary>
        /// TokenLimit
        /// </summary>
        /// <value>int</value>
        public int TokenLimit { get; set; } = 10;

        /// <summary>
        /// TokenLimit2
        /// </summary>
        /// <value>int</value>
        public int TokenLimit2 { get; set; } = 20;

        /// <summary>
        /// TokensPerPeriod
        /// </summary>
        /// <value>int</value>
        public int TokensPerPeriod { get; set; } = 10;

        /// <summary>
        /// AutoReplenishment
        /// </summary>
        /// <value>bool</value>
        public bool AutoReplenishment { get; set; } = true;
    }
}