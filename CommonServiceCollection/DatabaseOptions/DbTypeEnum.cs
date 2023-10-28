using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonServiceCollection.DatabaseOptions
{
    public class DbTypeEnum
    {
        public static string MsSql { get; } = "MsSql";
        public static string MySql { get; } = "MySql";
        public static string SqLite { get; } = "SqLite";
        public static string MongoDb { get; } = "MongoDb";
    }
}