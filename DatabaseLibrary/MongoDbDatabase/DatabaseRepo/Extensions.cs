using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseLibrary.MongoDbDatabase.Models;
using DatabaseLibrary.MongoDbDatabase.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;


namespace DatabaseLibrary.MongoDbDatabase.DatabaseRepo
{
  public static class Extensions
  {
    public static IServiceCollection AddMongo(this IServiceCollection services)
    {
      var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

      // MongoDB friendly contents
      BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
      BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

      // #### MongoDB Configuration ####
      MongoDbDatabase.Settings.MongoDatabaseSettings dbSettings = new MongoDbDatabase.Settings.MongoDatabaseSettings(configuration);
      MongoServiceSettings serveSettings = new MongoServiceSettings(configuration);
      string connectionString = dbSettings.ConnectionString;
      string serviceName = serveSettings.ServiceName;

      services.AddSingleton(serviceProvider =>
      {
        var mongoClient = new MongoClient(connectionString);
        return mongoClient.GetDatabase(serviceName);
      });
      // #### MongoDB Configuration ####

      return services;
    }

    public static IServiceCollection AddMongoRepository<T>(
        this IServiceCollection services,
        string collectionName
    )
      where T : IBaseEntity
    {
      services.AddSingleton<IMongoDatabaseRepo<T>>(serviceProvider =>
      {
        var database = serviceProvider.GetRequiredService<IMongoDatabase>();
        return new MongoDatabaseRepo<T>(database, collectionName);
      });

      return services;
    }
  }
}