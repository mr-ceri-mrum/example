using AutoMapper;
using HomeDelivery.Order.Data.Dtos;
using HomeDelivery.Order.Data.Dtos.Address;
using HomeDelivery.Order.Data.Dtos.Dish;
using HomeDelivery.Order.Data.Dtos.MenuSection;
using HomeDelivery.Order.DataAccess.DbModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HomeDelivery.Order.Business.ServiceRegistrations;

public static class AutoMapServiceRegistrations
{
    public static IServiceCollection AddAutoMapServices
        (this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddSingleton(new MapperConfiguration(config =>
        {
            #region orders
            config.CreateMap<OrderCreatDto, DataAccess.DbModels.Order>();
            config.CreateMap<OrderCreatRequestDto, DataAccess.DbModels.Order>();
            config.CreateMap<DataAccess.DbModels.Order, OrderCreatRequestDto>(); 
            config.CreateMap<DataAccess.DbModels.Order, OrderCreatDto>();
            #endregion
           
            
            #region addres
            config.CreateMap<AddressDto, Address>().ForMember(dest => dest.Id, 
                opt => 
                    opt.MapFrom(src => src.AddressId));
            
            
            config.CreateMap<Address, AddressForCourierServiceDto>() // Маппинг для Address -> AddressForCourierServiceDto
                .ForMember(dest => dest.AddressId, opt => opt.MapFrom(src => src.Id)); // Маппим Id в AddressId

            config.CreateMap<AddressForCourierServiceDto, Address>() // Обратный маппинг
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AddressId)); // Маппим AddressId в Id

            config.CreateMap<Address, AddressDto>();

            config.CreateMap<AddressForCourierServiceDto, Address>();

            #endregion

            #region MenuSection

            config.CreateMap<MenuSection, MenuSectionCreateDto>();
            config.CreateMap<MenuSectionCreateDto, MenuSection>();

            #endregion

            #region Dish

            config.CreateMap<Dish, DishCreateDto>();
            config.CreateMap<Dish, DishEditDto>();
            config.CreateMap<DishCreateDto, Dish>();
            #endregion
        }).CreateMapper());

       
        return services;
    }
}