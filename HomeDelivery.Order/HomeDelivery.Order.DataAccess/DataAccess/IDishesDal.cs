using HomeDelivery.Order.DataAccess.DbContexts;
using HomeDelivery.Order.DataAccess.EfEntityRepositoryBase;

namespace HomeDelivery.Order.DataAccess.DataAccess;

public interface IDishesDal:IEntityRepository<DbModels.Dish> { }

public class DishesDal : EfEntityRepositoryBase<DbModels.Dish, DataContext>, IDishesDal
{
    public DishesDal(DataContext context) : base(context)
    {
    }
}