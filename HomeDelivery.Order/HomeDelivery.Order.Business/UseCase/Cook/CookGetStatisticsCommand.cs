using HomeDelivery.Order.Business.Services;
using HomeDelivery.Order.Core.ResultResponses;
using HomeDelivery.Order.Data.Enums;
using HomeDelivery.Order.DataAccess.DataAccess;
using MediatR;

namespace HomeDelivery.Order.Business.UseCase.Cook;

public class CookGetStatisticsCommand(int filterNumber) : IRequest<IDataResult<object>>
{
    public int FilterNumber { get; set; } = filterNumber;
}

internal class CookGetStatisticsCommandHandler(IAuthInformationRepository authInformationRepository, IOrderDal orderDal) : IRequestHandler<CookGetStatisticsCommand, IDataResult<object>>
{
    public async Task<IDataResult<object>> Handle(CookGetStatisticsCommand request, CancellationToken cancellationToken)
    {
        var userId = authInformationRepository.GetUser()!.Id;

        var orderCount = request.FilterNumber switch
        {
            1 => await orderDal.CountAsync(x =>
                x.CookId == userId && x.StatusId == (int)OrderStatus.Completed &&
                x.DataCreate >= DateTime.Now.AddMonths(-1)),
            2 => await orderDal.CountAsync(x =>
                x.CookId == userId && x.StatusId == (int)OrderStatus.Completed &&
                x.DataCreate >= DateTime.Now.AddMonths(-12)),
            3 => await orderDal.CountAsync(x => 
                x.CookId == userId && 
                x.StatusId == (int)OrderStatus.Completed),
            _ => 0
        };

        return new SuccessDataResult<object>(orderCount, "");
    }
}