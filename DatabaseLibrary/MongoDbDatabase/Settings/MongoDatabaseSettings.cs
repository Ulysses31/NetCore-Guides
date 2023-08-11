using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace DatabaseLibrary.MongoDbDatabase.Settings
{
  public class MongoDatabaseSettings
  {
    private readonly IConfiguration _configuration;

    public MongoDatabaseSettings(IConfiguration configuration)
    {
      this._configuration = configuration;
    }

    public string Host => String.Empty;
    // this._configuration
    //   .GetSection(nameof(MongoDatabaseSettings))
    //   .GetValue<string>(nameof(Host));


    public int Port => 0;
    // this._configuration
    //   .GetSection(nameof(MongoDatabaseSettings))
    //   .GetValue<int>(nameof(Port));

    public string ConnectionString => $"mongodb://{Host}:{Port}";
  }
}