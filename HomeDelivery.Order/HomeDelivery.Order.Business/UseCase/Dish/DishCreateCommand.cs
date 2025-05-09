using AutoMapper;
using HomeDelivery.Order.Business.Services;
using HomeDelivery.Order.Core.ResultResponses;
using HomeDelivery.Order.Data.Dtos.Dish;
using HomeDelivery.Order.DataAccess.DataAccess;
using MediatR;

namespace HomeDelivery.Order.Business.UseCase.Dish;

public class DishCreateCommand(DishCreateDto form) : IRequest<IDataResult<object>>
{
    public DishCreateDto Form { get; set; } = form;
}

internal class DishCreateCommandHandler(IDishesDal dishesDal, IMapper mapper, IStoreService storeService) 
    : IRequestHandler<DishCreateCommand, IDataResult<object>>
{
    public async Task<IDataResult<object>> Handle(DishCreateCommand request, CancellationToken cancellationToken)
    {
        var dish = mapper.Map<DataAccess.DbModels.Dish>(request.Form);
        await dishesDal.AddAsync(dish);
        bool IsSaved = await storeService.StorePhotoForDish(dish.Id);
        
        return new SuccessDataResult<object>(dish, $"{IsSaved}");
    }
}