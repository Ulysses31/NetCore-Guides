using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DatabaseLibrary.MsSqlDatabase.DatabaseRepo
{
  public abstract class MsSqlDatabaseBaseRepo<TEntity> : IMsSqlDatabaseBaseRepo<TEntity> where TEntity : class
  {
    protected MsSqlDatabaseBaseRepo()
    { }

    #region Sync

    public abstract IQueryable<TEntity> Filter();

    public abstract IEnumerable<TEntity> Filter(Func<TEntity, bool> predicate);

    public abstract IQueryable<TEntity> FilterAsNoTracking();

    public abstract IEnumerable<TEntity> FilterAsNoTracking(Func<TEntity, bool> predicate);

    public abstract TEntity Filter(string id);

    public abstract TEntity Create(TEntity entity);

    public abstract TEntity Delete(TEntity entity);

    public abstract TEntity Update(Func<TEntity, bool> predicate, TEntity entity);

    #endregion Sync

    #region Async

    public abstract Task<IQueryable<TEntity>> FilterAsync();

    public abstract Task<IEnumerable<TEntity>> FilterAsync(Func<TEntity, bool> predicate);

    public abstract Task<IQueryable<TEntity>> FilterAsNoTrackingAsync();

    public abstract Task<IEnumerable<TEntity>> FilterAsNoTrackingAsync(Func<TEntity, bool> predicate);

    public abstract Task<TEntity> FilterAsync(string id);

    public abstract Task<TEntity> CreateAsync(TEntity entity);

    public abstract Task<TEntity> UpdateAsync(Func<TEntity, bool> predicate, TEntity entity);

    public abstract Task<TEntity> DeleteAsync(TEntity entity);

    #endregion Async

    public abstract void SaveChanges(DbContext context);

    public abstract void SaveChangesAsync(DbContext context);
  }
}