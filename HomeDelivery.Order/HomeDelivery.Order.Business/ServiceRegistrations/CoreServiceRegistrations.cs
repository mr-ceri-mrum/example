using HomeDelivery.Order.Business.Services;
using HomeDelivery.Order.Core.GaneralHelpers;
using HomeDelivery.Order.Core.Hash;
using HomeDelivery.Order.Core.Responses;
using HomeDelivery.Order.Core.ResultResponses;
using HomeDelivery.Order.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HomeDelivery.Order.Business.ServiceRegistrations;

public static class CoreServiceRegistrations
{
    public static IServiceCollection AddCoreServices
        (this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        
        #region DbContexts
        var conn = "DefaultConnection";
        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString(conn),
                b =>
                    b.MigrationsAssembly("HomeDelivery.Order.API")
            );
        });
        #endregion
        
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        
        services.AddAuthorization();
        
        services.AddScoped<IHashRepository, HashRepository>();
        services.AddScoped<IEncoding, EncodingService>();
        services.AddTransient<IMessagesRepository, MessagesRepository>();
        services.AddScoped<IXssRepository, XssRepository>();
        services.AddScoped<IStoreService, StoreService>();
        services.AddScoped<IAuthInformationRepository, AuthInformationRepository>();
        services.AddScoped<IRedisService, RedisService>();
        
        #region Cors
        
        services.AddCors(options =>
        {
            options.AddPolicy(name: configuration.GetSection("CorsLabel").Value!,
                builder =>
                {
                    builder.WithMethods(
                        configuration.GetSection("Methods").GetChildren().Select(x => x.Value).ToArray()!);
                    builder.AllowAnyHeader();
                    builder.AllowCredentials();
                    builder.WithOrigins(
                        configuration.GetSection("Origins").GetChildren().Select(x => x.Value).ToArray()!);
                    builder.Build();
                });
        });
        #endregion
        return services;
    }
}