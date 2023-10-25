using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DatabaseLibrary.MsSqlDatabase.DatabaseRepo
{
    public interface IMsSqlDatabaseBaseRepo<TEntity>
    {
        #region Sync

        abstract IQueryable<TEntity> Filter();

        abstract IQueryable<TEntity> FilterAsNoTracking();

        abstract IEnumerable<TEntity> Filter(Func<TEntity, bool> predicate);

        abstract IEnumerable<TEntity> FilterAsNoTracking(Func<TEntity, bool> predicate);

        abstract TEntity Filter(string id);

        abstract TEntity Create(TEntity entity);

        abstract TEntity Update(Func<TEntity, bool> predicate, TEntity entity);

        abstract TEntity Delete(TEntity entity);

        #endregion Sync

        #region Async

        abstract Task<IQueryable<TEntity>> FilterAsync();

        abstract Task<IQueryable<TEntity>> FilterAsNoTrackingAsync();

        abstract Task<IEnumerable<TEntity>> FilterAsync(Func<TEntity, bool> predicate);

        abstract Task<IEnumerable<TEntity>> FilterAsNoTrackingAsync(Func<TEntity, bool> predicate);

        abstract Task<TEntity> FilterAsync(string id);

        abstract Task<TEntity> CreateAsync(TEntity entity);

        abstract Task<TEntity> UpdateAsync(Func<TEntity, bool> predicate, TEntity entity);

        abstract Task<TEntity> DeleteAsync(TEntity entity);

        #endregion Async

        abstract int SaveChanges(DbContext context);

        abstract Task<int> SaveChangesAsync(DbContext context);
    }
}
