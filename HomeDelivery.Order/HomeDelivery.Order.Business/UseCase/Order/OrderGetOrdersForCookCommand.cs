using HomeDelivery.Order.Business.Services;
using HomeDelivery.Order.Core.Responses;
using HomeDelivery.Order.Core.ResultResponses;
using HomeDelivery.Order.Data.Enums;
using HomeDelivery.Order.DataAccess.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeDelivery.Order.Business.UseCase.Order;

public class OrderGetOrdersForCookCommand(int pageNumber, int pageSize): IRequest<IDataResult<object>>
{
    public int PageNumber { get; set; } = pageNumber;
    public int PageSize { get; set; } = pageSize;
}

public class OrderGetOrdersForCookCommandHandler
    (IOrderDal orderDal, IMessagesRepository messagesRepository
    , IAuthInformationRepository authInformationRepository)
    
    : IRequestHandler<OrderGetOrdersForCookCommand, IDataResult<object>>
{
    public async Task<IDataResult<object>> Handle(OrderGetOrdersForCookCommand request, CancellationToken cancellationToken)
    {
        var order = await orderDal.GetAllAsQueryable(request.PageNumber,request.PageSize,
            x => x.StatusId == (int)OrderStatus.Create).ToListAsync(cancellationToken: cancellationToken);
        return new SuccessDataResult<object>(order, "");
    }
}