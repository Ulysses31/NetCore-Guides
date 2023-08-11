using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseLibrary.MongoDbDatabase.Models;

namespace DatabaseLibrary.MongoDbDatabase.DatabaseRepo
{
  public interface IMongoDatabaseRepo<T> where T : IBaseEntity
  {
    Task CreateAsync(T entity);
    Task<IReadOnlyCollection<T>> GetAllAsync();
    Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);
    Task<T> GetAsync(Guid id);
    Task<T> GetAsync(Expression<Func<T, bool>> filter);
    Task RemoveAsync(Guid id);
    Task UpdateAsync(T entity);
  }
}