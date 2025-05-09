using HomeDelivery.Order.Core.ResultResponses;
using HomeDelivery.Order.Data.Dtos.Paginate;
using HomeDelivery.Order.DataAccess.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeDelivery.Order.Business.UseCase.Dish;

public class DishGetAllForUserCommand : IRequest<IDataResult<object>>
{
    public DishGetAllForUserCommand(PaginationDto paginationDto)
    {
        PaginationDto = paginationDto;
    }

    public PaginationDto PaginationDto { get; set; }
}

internal class DishGetAllForUserCommandHandler(IDishesDal dishesDal) : IRequestHandler<DishGetAllForUserCommand, IDataResult<object>>
{
    public async Task<IDataResult<object>> Handle(DishGetAllForUserCommand request, CancellationToken cancellationToken)
    {
        var dishes = await dishesDal
            .GetAllAsQueryable(request.PaginationDto.PageNumber, request.PaginationDto.PageSize, dish => dish.IsAvailable)
            .ToListAsync(cancellationToken);
        
        var result = new List<object>();
            
        foreach (var dish in dishes)
        {
            string? avatarBase64 = null;
            if (!string.IsNullOrEmpty(dish.ImgUrl))
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), dish.ImgUrl);

                if (File.Exists(path))
                {
                    var bytes = await System.IO.File.ReadAllBytesAsync(path, cancellationToken);
                    avatarBase64 = Convert.ToBase64String(bytes); // Конвертируем в Base64
                }
            }

            result.Add(new
            {
                dishId = dish.Id,
                dish.Name,
                dish.Price,
                Grade = 4.7,
                ImgBase64 = avatarBase64
            });
        }

        return new SuccessDataResult<object>(result, "");
        
    }
}