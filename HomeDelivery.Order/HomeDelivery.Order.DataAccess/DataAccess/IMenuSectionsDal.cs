using HomeDelivery.Order.DataAccess.DbContexts;
using HomeDelivery.Order.DataAccess.EfEntityRepositoryBase;

namespace HomeDelivery.Order.DataAccess.DataAccess;

public interface IMenuSectionsDal : IEntityRepository<DbModels.MenuSection> { }

public class MenuSectionsDal :  EfEntityRepositoryBase<DbModels.MenuSection, DataContext>, IMenuSectionsDal
{
    public MenuSectionsDal(DataContext context) : base(context)
    {
    }
}