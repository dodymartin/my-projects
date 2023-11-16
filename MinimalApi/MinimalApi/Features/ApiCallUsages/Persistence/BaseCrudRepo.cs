using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.ApiCallUsages;

public class BaseCrudRepo<TEntity, TKey>(IApiCallUsageDbContext context) : IBaseCrudRepo<TEntity, TKey>
    where TEntity : class
{
    protected readonly DbContext _dbContext = (DbContext)context;

    private readonly DbSet<TEntity>? _dbSet;
    protected DbSet<TEntity> DbSet => _dbSet ?? _dbContext.Set<TEntity>();

    #region IBaseRepo

    public virtual IEnumerable<TEntity> Get(
        Expression<Func<TEntity, bool>> filter = null!,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null!,
        string includeProperties = "")
    {
        IQueryable<TEntity> query = DbSet;

        if (filter != null)
            query = query.Where(filter);

        foreach (var includeProperty in includeProperties.Split
            (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
            return orderBy(query).ToList();
        return query.ToList();
    }

    public TEntity GetById(TKey id)
    {
        return DbSet.Find(id)!;
    }

    public void Insert(TEntity entityToInsert)
    {
        DbSet.Add(entityToInsert);
    }

    public virtual void Delete(TKey id)
    {
        TEntity entityToDelete = DbSet.Find(id)!;
        Delete(entityToDelete);
    }

    public void Delete(TEntity entityToDelete)
    {
        if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
            DbSet.Attach(entityToDelete);
        DbSet.Remove(entityToDelete);
    }

    public void Update(TEntity entityToUpdate)
    {
        DbSet.Attach(entityToUpdate);
        _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
    }

    public bool ExecuteSQL(string query)
    {
        return _dbContext.Database.ExecuteSqlRaw(query) > 0;
    }

    public int Save()
    {
        return _dbContext.SaveChanges();
    }

    public async Task<int> SaveAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    #endregion

    #region IDisposable

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
            if (disposing)
                _dbContext.Dispose();
        disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion
}

