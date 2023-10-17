using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageBroker
{
    public static class MessageStatusOptions
    {
        public static string NEW => "NEW";

        public static string REQUESTED => "REQUESTED";

        public static string SENT => "SENT";
    }
}
