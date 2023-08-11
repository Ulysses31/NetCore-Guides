using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseLibrary.MsSqlDatabase;
using DatabaseLibrary.MsSqlDatabase.DatabaseRepo;
using Microsoft.EntityFrameworkCore;

namespace DatabaseLibrary_XUnit_Tests.MsSqlDatabase
{
  public class TestHelpers
  {
    public DbContextOptions<DatabaseContextMsSql> _dbContextOptions { get; }

    public TestHelpers(DbContextOptions<DatabaseContextMsSql> dbContextOptions)
    {
      this._dbContextOptions = dbContextOptions;
    }

    public MsSqlDatabaseRepo<TEntity> CreateRepository<TEntity>() where TEntity : class
    {
      DatabaseContextMsSql context = new DatabaseContextMsSql(_dbContextOptions);
      // PopulateData(context);
      return new MsSqlDatabaseRepo<TEntity>(context);
    }

    public async Task<MsSqlDatabaseRepo<TEntity>> CreateRepositoryAsync<TEntity>() where TEntity : class
    {
      DatabaseContextMsSql context = new DatabaseContextMsSql(_dbContextOptions);
      // PopulateData(context);
      return await Task.FromResult(new MsSqlDatabaseRepo<TEntity>(context));
    }
  }
}