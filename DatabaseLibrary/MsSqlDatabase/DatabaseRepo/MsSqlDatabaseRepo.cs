using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace DatabaseLibrary.MsSqlDatabase.DatabaseRepo
{
  public class MsSqlDatabaseRepo<TEntity> : MsSqlDatabaseBaseRepo<TEntity> where TEntity : class

  {
    private const string NullEntity = "You must provide an entity.";

    private readonly DatabaseContextMsSql _context;

    public MsSqlDatabaseRepo(DatabaseContextMsSql context) : base()
    {
      this._context = context ?? throw new ArgumentNullException(nameof(context));
    }

    #region Sync
    public override IQueryable<TEntity> Filter()
    {
      try
      {
        return _context.Set<TEntity>();
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex.InnerException);
      }
    }

    public override IQueryable<TEntity> FilterAsNoTracking()
    {
      try
      {
        return _context.Set<TEntity>().AsNoTracking();
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex.InnerException);
      }
    }

    public override IEnumerable<TEntity> Filter(Func<TEntity, bool> predicate)
    {
      try
      {
        return _context.Set<TEntity>().Where(predicate);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex.InnerException);
      }
    }

    public override IEnumerable<TEntity> FilterAsNoTracking(Func<TEntity, bool> predicate)
    {
      try
      {
        return _context.Set<TEntity>().AsNoTracking().Where(predicate);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex.InnerException);
      }
    }

    public override TEntity Filter(string id)
    {
      try
      {
        if (id == null)
        {
          throw new ArgumentNullException(NullEntity);
        }

        return _context.Set<TEntity>().Find(Convert.ToInt32(id));

      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex.InnerException);
      }
    }

    public override TEntity Create(TEntity entity)
    {
      try
      {
        if (entity == null)
        {
          throw new ArgumentNullException(NullEntity);
        }

        _context.Set<TEntity>().Add(entity);

        SaveChanges(_context);

        return entity;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex.InnerException);
      }
    }

    public override TEntity Delete(TEntity entity)
    {
      try
      {
        if (entity == null)
        {
          throw new ArgumentNullException(NullEntity);
        }

        _context.Set<TEntity>().Remove(entity);

        SaveChanges(_context);

        return entity;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex.InnerException);
      }
    }

    public override TEntity Update(Func<TEntity, bool> predicate, TEntity entity)
    {
      try
      {
        if (entity == null)
        {
          throw new ArgumentNullException(NullEntity);
        }

        TEntity entityToUpdate = Filter(predicate).FirstOrDefault();

        entityToUpdate = entity;

        _context.Entry(entity).State = EntityState.Modified;

        SaveChanges(_context);

        return entityToUpdate;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex.InnerException);
      }
    }

    #endregion

    #region Async

    public override async Task<IQueryable<TEntity>> FilterAsync()
    {
      try
      {
        return await Task.FromResult(_context.Set<TEntity>());
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex.InnerException);
      }
    }

    public override async Task<IQueryable<TEntity>> FilterAsNoTrackingAsync()
    {
      try
      {
        return await Task.FromResult(_context.Set<TEntity>().AsNoTracking());
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex.InnerException);
      }
    }

    public override async Task<IEnumerable<TEntity>> FilterAsync(Func<TEntity, bool> predicate)
    {
      try
      {
        return await Task.FromResult(_context.Set<TEntity>().Where(predicate));
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex.InnerException);
      }
    }

    public override async Task<IEnumerable<TEntity>> FilterAsNoTrackingAsync(Func<TEntity, bool> predicate)
    {
      try
      {
        return await Task.FromResult(_context.Set<TEntity>()
          .AsNoTracking().Where(predicate));
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex.InnerException);
      }
    }

    public override async Task<TEntity> FilterAsync(string id)
    {
      try
      {
        if (id == null)
        {
          throw new ArgumentNullException(NullEntity);
        }


        return await _context.Set<TEntity>().FindAsync(Convert.ToInt32(id));

      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex.InnerException);
      }
    }

    public override async Task<TEntity> CreateAsync(TEntity entity)
    {
      try
      {
        if (entity == null)
        {
          throw new ArgumentNullException(NullEntity);
        }

        await _context.Set<TEntity>().AddAsync(entity);

        await SaveChangesAsync(_context);

        return entity;
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex.InnerException);
      }
    }

    public override async Task<TEntity> UpdateAsync(Func<TEntity, bool> predicate, TEntity entity)
    {
      try
      {
        if (entity == null)
        {
          throw new ArgumentNullException(NullEntity);
        }

        var entityToUpdateTmp = await FilterAsNoTrackingAsync(predicate);


        TEntity entityToUpdate = entityToUpdateTmp.FirstOrDefault();


        entityToUpdate = entity;

        _context.Entry(entity).State = EntityState.Modified;

        await SaveChangesAsync(_context);

        return await Task.FromResult(entityToUpdate);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex.InnerException);
      }
    }

    public override async Task<TEntity> DeleteAsync(TEntity entity)
    {
      try
      {
        if (entity == null)
        {
          throw new ArgumentNullException(NullEntity);
        }

        _context.Set<TEntity>().Remove(entity);

        await SaveChangesAsync(_context);

        return await Task.FromResult(entity);
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex.InnerException);
      }
    }

    #endregion

    public override int SaveChanges(DbContext context)
    {
      try
      {
        return context.SaveChanges();
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex.InnerException);
      }
    }

    public override async Task<int> SaveChangesAsync(DbContext context)
    {
      try
      {
        return await _context.SaveChangesAsync();
      }
      catch (Exception ex)
      {
        throw new Exception(ex.Message, ex.InnerException);
      }
    }

  }
}
