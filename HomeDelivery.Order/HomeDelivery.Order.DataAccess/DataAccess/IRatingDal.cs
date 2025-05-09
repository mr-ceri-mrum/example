using HomeDelivery.Order.DataAccess.DbContexts;
using HomeDelivery.Order.DataAccess.EfEntityRepositoryBase;

namespace HomeDelivery.Order.DataAccess.DataAccess;

public interface IRatingDal : IEntityRepository<DbModels.Rating> { }

public class RatingDal : EfEntityRepositoryBase<DbModels.Rating, DataContext>, IRatingDal
{
    public RatingDal(DataContext context) : base(context)
    {
        
    }
}
