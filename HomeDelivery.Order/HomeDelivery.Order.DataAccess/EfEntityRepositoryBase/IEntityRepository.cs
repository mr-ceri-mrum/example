using System.Linq.Expressions;

namespace HomeDelivery.Order.DataAccess.EfEntityRepositoryBase;

public interface IEntityRepository<T> where T : class,  new()
{
    T? Get(Expression<Func<T, bool>> filter);

    IQueryable<T> GetAllAsQueryable(
        int pageNumber, int pageSize,
        Expression<Func<T, bool>>? filter = null,
        params Expression<Func<T, object>>[] includes);
    
    Task<IQueryable<T>> GetAllAsQueryable(
        Expression<Func<T, bool>>? filter = null,
        params Expression<Func<T, object>>[] includes);
    
    List<T> GetAllQuery(ref int totalItems, int Page = 1, int ViewSize = 20, string? Query = null);
    List<T> GetAll(Expression<Func<T, bool>> filter = null);
    bool Any(Expression<Func<T, bool>> filter);
    void Add(T entity);    
    void Update(T entity);
    void Delete(T entity);
    int Count(Expression<Func<T, bool>> filter = null);

    // async
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
    Task<T?> GetAsync(Expression<Func<T, bool>> filter);
    Task<Guid?> GetId(Expression<Func<T, bool>> filter);
    
    Task<T?> GetAsync(
        Expression<Func<T, bool>> filter,
        params Expression<Func<T, object>>[] includes);
    Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task DeleteRangeAsync(List<T> entities);
    Task DeleteAsync(Expression<Func<T, bool>> filter = null);
    Task<int> CountAsync(Expression<Func<T, bool>> filter = null);
    
}

