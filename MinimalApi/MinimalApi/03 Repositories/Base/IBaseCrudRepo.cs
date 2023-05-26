using System.Linq.Expressions;

namespace MinimalApi.Repositories;

public interface IBaseCrudRepo<TEntity, TKey> : IDisposable
{
    IEnumerable<TEntity> Get(
        Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
        string includeProperties);
    TEntity GetById(TKey id);
    void Insert(TEntity entity);
    void Delete(TEntity entity);
    void Update(TEntity entity);
    int Save();
    Task<int> SaveAsync();
}
