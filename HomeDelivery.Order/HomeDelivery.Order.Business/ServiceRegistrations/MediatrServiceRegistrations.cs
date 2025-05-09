using HomeDelivery.Order.Business.UseCase.Dish;
using HomeDelivery.Order.Business.UseCase.Menu;
using HomeDelivery.Order.Business.UseCase.Order;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HomeDelivery.Order.Business.ServiceRegistrations
{
    public static class MediatrServiceRegistrations
    {
        public static IServiceCollection AddMediatrServices(
            this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            #region order

            services.AddMediatR(cnf => cnf.RegisterServicesFromAssemblies(typeof(OrderCreateCommand).Assembly));
            
            services.AddMediatR(cnf => cnf.RegisterServicesFromAssemblies(typeof(OrderAcceptCommand).Assembly));
            services.AddMediatR(cnf => cnf.RegisterServicesFromAssemblies(typeof(OrderGetCommand).Assembly));
            services.AddMediatR(cnf => cnf.RegisterServicesFromAssemblies(typeof(OrderReadyCommand).Assembly));
            services.AddMediatR(cnf => cnf.RegisterServicesFromAssemblies(typeof(OrderGetMyOrdersForUserCommand).Assembly));
            services.AddMediatR(
                cnf => cnf.RegisterServicesFromAssemblies(typeof(OrderGetOrdersForCookCommand).Assembly));
            services.AddMediatR(cnf => 
                cnf.RegisterServicesFromAssemblies(typeof(OrderChangeStatusCommand).Assembly));

            #endregion

            #region Dish

            services.AddMediatR(cnf => 
                cnf.RegisterServicesFromAssemblies(typeof(DishCreateCommand).Assembly));
            services.AddMediatR(cnf => 
                cnf.RegisterServicesFromAssemblies(typeof(DishEditCommand).Assembly));

            #endregion

            #region MenuSection

            services.AddMediatR(cnf => 
                cnf.RegisterServicesFromAssemblies(typeof(MenuSectionCreateCommand).Assembly));

            #endregion
            
            return services;
        }
    }
}
