using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Api.Common;

namespace MinimalApi.Api.Features.ApiCallUsages;

public class ApiCallUsageRepo<TEntity, TKey>(IApiCallUsageDbContext context) : IBaseCrudRepo<TEntity, TKey>
    where TEntity : class
{
    private static readonly char[] _commaSeparator = [','];
    protected readonly DbContext DbContext = (DbContext)context;

    private DbSet<TEntity>? _dbSet;
    protected DbSet<TEntity> DbSet => _dbSet ??= DbContext.Set<TEntity>();

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
            (_commaSeparator, StringSplitOptions.RemoveEmptyEntries))
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
        var entityToDelete = DbSet.Find(id)!;
        Delete(entityToDelete);
    }

    public void Delete(TEntity entityToDelete)
    {
        if (DbContext.Entry(entityToDelete).State == EntityState.Detached)
            DbSet.Attach(entityToDelete);
        DbSet.Remove(entityToDelete);
    }

    public void Update(TEntity entityToUpdate)
    {
        DbSet.Attach(entityToUpdate);
        DbContext.Entry(entityToUpdate).State = EntityState.Modified;
    }

    public bool ExecuteSQL(string query)
    {
        return DbContext.Database.ExecuteSqlRaw(query) > 0;
    }

    public int Save()
    {
        return DbContext.SaveChanges();
    }

    public async Task<int> SaveAsync()
    {
        return await DbContext.SaveChangesAsync();
    }

    #endregion

    #region IDisposable

    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing)
                DbContext.Dispose();
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion
}

