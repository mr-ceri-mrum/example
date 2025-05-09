using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HomeDelivery.Order.Business.ServiceRegistrations;

public static class JWTServiceRegistrations
{
    public static IServiceCollection AddJWTService(
        this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT" // 
            });
            options.OperationFilter<SecurityRequirementsOperationFilter>();
        });
        
        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(op =>
        {
            op.SaveToken = true;
            op.TokenValidationParameters = new TokenValidationParameters()
            {
                SaveSigninToken = true,
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "home-auth-api",       // Jwt:Issuer - config value 
                ValidAudience = "homeMicroServiceApp",     // Jwt:Issuer - config value 
                IssuerSigningKey = new SymmetricSecurityKey("testMySecret_me_and_fridends_aaaaaaaaaaaaaaaaaaaaaaaa"u8.ToArray())
            };
        });
        return services;
    }
}