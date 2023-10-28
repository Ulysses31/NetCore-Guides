using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonServiceCollection.Swagger
{
    public class CommonSwaggerOptions
    {
        public const string MySwagger = "Swagger";

        public static string? Description { get; set; }

        public static string? Title { get; set; }

        public static string? TermsOfService { get; set; }

        public static Options? Options { get; set; }

        public static License? License { get; set; }

        public static Contact? Contact { get; set; }
    }

    public class Options
    {
        public string? Deprecate_Version_Description { get; set; }
        public string? Sunset_Policy_Description { get; set; }
    }

    public class License
    {
        public string? Name { get; set; }
        public string? Url { get; set; }
    }

    public class Contact
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Url { get; set; }
    }
}