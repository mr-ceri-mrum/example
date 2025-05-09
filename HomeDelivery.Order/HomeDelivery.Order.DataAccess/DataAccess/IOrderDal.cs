using HomeDelivery.Order.DataAccess.DbContexts;
using HomeDelivery.Order.DataAccess.EfEntityRepositoryBase;

namespace HomeDelivery.Order.DataAccess.DataAccess;

public interface IOrderDal:IEntityRepository<DbModels.Order> { }

public class OrderDal : EfEntityRepositoryBase<DbModels.Order, DataContext>, IOrderDal
{
    public OrderDal(DataContext context) : base(context)
    {
        
    }
}
