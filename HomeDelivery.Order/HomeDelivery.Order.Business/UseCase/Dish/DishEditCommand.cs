using System.Net;
using AutoMapper;
using HomeDelivery.Order.Business.Services;
using HomeDelivery.Order.Core.ResultResponses;
using HomeDelivery.Order.Data.Dtos.Dish;
using HomeDelivery.Order.DataAccess.DataAccess;
using MediatR;

namespace HomeDelivery.Order.Business.UseCase.Dish;

public class DishEditCommand(DishEditDto form) : IRequest<IDataResult<object>>
{
    public DishEditDto Form { get; set; } = form;
}

public class DishEditCommandHandler(IDishesDal dishesDal, IMapper mapper, IStoreService storeService) 
    : IRequestHandler<DishEditCommand, IDataResult<object>>
{
    public async Task<IDataResult<object>> Handle(DishEditCommand request, CancellationToken cancellationToken)
    {
        var dish = await dishesDal.GetAsync(x => x.Id == request.Form.DishId);
        if (dish == null) return new ErrorDataResult<object>("", HttpStatusCode.NotFound);
        mapper.Map(request.Form, dish);

        // Обновление блюда в базе данных
        await dishesDal.UpdateAsync(dish);
        var isSave = await storeService.StorePhotoForDish(dish.Id);
        
        return new SuccessDataResult<object>(dish, $"{isSave}");
    }
}