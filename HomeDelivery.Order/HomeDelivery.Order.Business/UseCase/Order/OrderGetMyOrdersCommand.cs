using System.Net;
using HomeDelivery.Order.Business.Services;
using HomeDelivery.Order.Core.Responses;
using HomeDelivery.Order.Core.ResultResponses;
using HomeDelivery.Order.DataAccess.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeDelivery.Order.Business.UseCase.Order;

public class OrderGetMyOrdersForUserCommand(int pageNumber = 1, int pageSize = 10) 
    : IRequest<IDataResult<object>>
{
    public int PageNumber { get; } = pageNumber > 0 ? pageNumber : 1;
    public int PageSize { get; } = pageSize > 0 ? pageSize : 10;
}

public class OrderGetMyOrdersCommandHandle(IOrderDal orderDal, IMessagesRepository messagesRepository, IAuthInformationRepository authInformationRepository) 
    : IRequestHandler<OrderGetMyOrdersForUserCommand, IDataResult<object>>
{
    public async Task<IDataResult<object>> Handle(OrderGetMyOrdersForUserCommand request, CancellationToken cancellationToken)
    {
        var userId = authInformationRepository.GetUser()!.Id;
        
        var order = await orderDal.GetAllAsQueryable(request.PageNumber,request.PageSize,
            x => x.CreatorId == userId)
            .ToListAsync(cancellationToken: cancellationToken);
        
        return new SuccessDataResult<object>(order, "");
    }
}