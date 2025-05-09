using System.Net;
using HomeDelivery.Order.Core.ResultResponses;
using HomeDelivery.Order.DataAccess.DataAccess;
using MediatR;

namespace HomeDelivery.Order.Business.UseCase.Dish;

public class DishGetUserCommand(Guid dishId) : IRequest<IDataResult<object>>
{
    public Guid DishId { get; set; } = dishId;
}

internal class DishGetUserCommandHandler(IDishesDal dishesDal) : IRequestHandler<DishGetUserCommand, IDataResult<object>>
{
    public async Task<IDataResult<object>> Handle(DishGetUserCommand request, CancellationToken cancellationToken)
    {
        var dish = await dishesDal.GetAsync(x => x.Id == request.DishId);
        if (dish == null)
        {
            return new ErrorDataResult<object>("Not found", HttpStatusCode.NotFound);
        }
        string? avatarBase64 = null;
        
        var path = Path.Combine(Directory.GetCurrentDirectory(), dish.ImgUrl);
        
        if (File.Exists(path))
        {
            var bytes = await System.IO.File.ReadAllBytesAsync(path, cancellationToken);
            avatarBase64 = Convert.ToBase64String(bytes); // Конвертируем в Base64
        }
        
        var result = new
        {
            Dish = dish,
            AvatarBase64 = avatarBase64
        };
        
        return new SuccessDataResult<object>(result, "");
    }
}