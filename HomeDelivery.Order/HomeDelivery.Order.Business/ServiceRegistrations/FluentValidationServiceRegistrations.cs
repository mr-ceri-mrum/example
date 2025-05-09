using FluentValidation.AspNetCore;
using HomeDelivery.Order.Business.Validator.Order;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HomeDelivery.Order.Business.ServiceRegistrations;

public static class FluentValidationServiceRegistrations
{
    public static IServiceCollection AddFluentValidationServices
        (this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddFluentValidation((fv) =>
            fv.RegisterValidatorsFromAssemblyContaining(typeof(OrderCreateCommandValidator)));
        
        return services;
    }
}