using HomeDelivery.Order.DataAccess.DbContexts;
using HomeDelivery.Order.DataAccess.EfEntityRepositoryBase;

namespace HomeDelivery.Order.DataAccess.DataAccess;

public interface IAddressDal:IEntityRepository<DbModels.Address> { }


public class AddressDal : EfEntityRepositoryBase<DbModels.Address, DataContext>, IAddressDal
{
    public AddressDal(DataContext context) : base(context)
    {
    }
}