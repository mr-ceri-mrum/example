using HomeDelivery.Order.Data.Dtos.Address;
using HomeDelivery.Order.DataAccess.DataAccess;
using HomeDelivery.Order.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HomeDelivery.Order.Business.ServiceRegistrations;

public static class DalServiceRegistrations
{
    public static IServiceCollection AddDalServices
        (this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        
        services.AddScoped<IOrderDal, OrderDal>();
        services.AddScoped<IRatingDal, RatingDal>();
        services.AddScoped<IAddressDal, AddressDal>();
        services.AddScoped<IDishesDal, DishesDal>(); 
        services.AddScoped<IMenuSectionsDal, MenuSectionsDal>();
        return services;
    }
}