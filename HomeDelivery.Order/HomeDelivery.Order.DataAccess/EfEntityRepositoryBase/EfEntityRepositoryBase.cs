using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace HomeDelivery.Order.DataAccess.EfEntityRepositoryBase;

public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
    where TEntity : class, IEntity<Guid>, new()
    where TContext : DbContext, new()
{
    #region DI
    private readonly TContext context;
    private IEntityRepository<TEntity> _entityRepositoryImplementation;

    public EfEntityRepositoryBase(TContext context)
    {
        this.context = context;
    }


    #endregion
    
    public IQueryable<TEntity> GetAllAsQueryable(
        int pageNumber, int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        query = query.AsNoTracking();
        query = includes.Aggregate(query, (current, include) => 
            current.Include(include));
        
        if (filter != null)
        {
            query = query.Where(filter);    
        }
        
        query = query.Where(x => !x.IsDeleted);
        query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        return query.AsQueryable();
    }

    public async Task<IQueryable<TEntity>> GetAllAsQueryable(Expression<Func<TEntity, bool>>? filter = null, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();
        query = query.AsNoTracking();
        query = includes.Aggregate(query, (current, include) => 
            current.Include(include));
        
        if (filter != null)
        {
            query = query.Where(filter);
        }
        
        query = query.Where(x => !x.IsDeleted).AsQueryable();
        return await Task.FromResult(query);
    }

    public List<TEntity> GetAllQuery(ref int totalItems, int Page = 1, int ViewSize = 20, string? Query = null)
    {
        throw new NotImplementedException();
    }

    public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
    {
        throw new NotImplementedException();
    }

    public TEntity? Get(Expression<Func<TEntity, bool>> filter)
    {
        return context.Set<TEntity>().Where(filter).FirstOrDefault();
    }
    public bool Any(Expression<Func<TEntity, bool>> filter) => context.Set<TEntity>().Any(filter);

    public void Add(TEntity entity)
    {
        context.Entry(entity).State = EntityState.Added;
        context.SaveChanges();
    }

    public void Update(TEntity entity)
    {
        context.Entry(entity).State = EntityState.Modified;
        context.SaveChanges();
    }
    
    public void Delete(TEntity entity)
    {
        entity.IsDeleted = true;
        context.Entry(entity).State = EntityState.Modified;
        context.SaveChanges();
    }
    public int Count(Expression<Func<TEntity, bool>>? filter = null)
    {
        return filter == null ? context.Set<TEntity>().Count() : context.Set<TEntity>().Where(filter).Count();
    }
    
    public async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null)
    {
        return filter == null
            ? await context.Set<TEntity>().OrderByDescending(o => o.Id).ToListAsync()
            : await context.Set<TEntity>().Where(filter).OrderByDescending(o => o.Id).ToListAsync();
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> filter)
        => await context.Set<TEntity>().Where(filter).FirstOrDefaultAsync();

    public async Task<Guid?> GetId(Expression<Func<TEntity, bool>> filter)
    {
        return await context.Set<TEntity>()
            .Where(filter)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<TEntity?> GetAsync(
        Expression<Func<TEntity, bool>> filter,
        params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = context.Set<TEntity>();

        // Применяем все includes
        query = includes.Aggregate(query, (current, include) => current.Include(include));

        return await query.FirstOrDefaultAsync(filter);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
        => await context.Set<TEntity>().AnyAsync(filter);

    public async Task AddAsync(TEntity entity)
    {
        if (entity is IEntity<Guid> updatableEntity)
            updatableEntity.ModifiedDate = DateTime.UtcNow;

        context.Entry(entity).State = EntityState.Added;
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        if (entity is IEntity<Guid> updatableEntity)
            updatableEntity.ModifiedDate = DateTime.UtcNow;

        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    #region Delete

    public async Task DeleteAsync(TEntity entity)
    {
        entity.IsDeleted = true;
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync();
    }

    public async Task DeleteRangeAsync(List<TEntity> entities)
    {
        entities.ForEach(x => x.IsDeleted = true);
        context.UpdateRange(entities);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Expression<Func<TEntity, bool>>? filter = null)
    {
        if (filter != null)
        {
            List<TEntity> deleteEntities = await context.Set<TEntity>().Where(filter).ToListAsync();
            context.RemoveRange(deleteEntities);
            await context.SaveChangesAsync();
        }
    }

    #endregion

    public Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null)
    {
        return filter == null ? context.Set<TEntity>().CountAsync() : context.Set<TEntity>().Where(filter).CountAsync();
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await context.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }
}