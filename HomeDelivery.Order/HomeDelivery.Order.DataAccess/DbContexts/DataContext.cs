using HomeDelivery.Order.DataAccess.DbModels;
using Microsoft.EntityFrameworkCore;

namespace HomeDelivery.Order.DataAccess.DbContexts
{
    public class DataContext : DbContext 
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        public DataContext() { }
        public DbSet<DbModels.Order> Orders { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<MenuSection> MenuSections { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}
