using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DatabaseLibrary.MongoDbDatabase.Settings
{
  public class MongoServiceSettings
  {
    private readonly IConfiguration _configuration;

    public MongoServiceSettings(IConfiguration configuration)
    {
      this._configuration = configuration;
    }

    public string ServiceName => String.Empty;
    // this._configuration
    //   .GetSection(nameof(MongoServiceSettings))
    //   .GetValue<string>(nameof(ServiceName));
  }
}