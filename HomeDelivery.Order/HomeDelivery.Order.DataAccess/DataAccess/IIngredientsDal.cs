using HomeDelivery.Order.DataAccess.DbContexts;
using HomeDelivery.Order.DataAccess.DbModels;
using HomeDelivery.Order.DataAccess.EfEntityRepositoryBase;

namespace HomeDelivery.Order.DataAccess.DataAccess;

public interface IIngredientsDal : IEntityRepository<Ingredient> {}

public class IngredientsDal : EfEntityRepositoryBase<Ingredient, DataContext>, IIngredientsDal
{
    public IngredientsDal(DataContext context) : base(context)
    {
    }
}