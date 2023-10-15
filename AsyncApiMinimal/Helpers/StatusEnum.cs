using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncApiMinimal.Helpers
{
    public static class StatusEnum
    {
        public static string? ACCEPT { get { return "ACCEPT"; } }
        public static string? PENDING { get { return "PENDING"; } }
        public static string? CANCELED { get { return "CANCELED"; } }
        public static string? FAILED { get { return "FAILED"; } }
        public static string? COMPLETED { get { return "COMPLETED"; } }
    }
}
