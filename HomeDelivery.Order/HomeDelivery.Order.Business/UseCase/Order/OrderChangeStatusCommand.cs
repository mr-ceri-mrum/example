using System.Net;
using HomeDelivery.Order.Core.Responses;
using HomeDelivery.Order.Core.ResultResponses;
using HomeDelivery.Order.Data.Dtos;
using HomeDelivery.Order.DataAccess.DataAccess;
using MediatR;

namespace HomeDelivery.Order.Business.UseCase.Order;

public class OrderChangeStatusCommand(OrderChangeStatusDto form) : IRequest<IDataResult<object>>
{
    public OrderChangeStatusDto Form { get; } = form;
    
}

public class OrderChangeStatusCommandHandler(
    IOrderDal orderDal, 
    IMessagesRepository messagesRepository) 
    : IRequestHandler<OrderChangeStatusCommand, IDataResult<object>>
{
    
    public async Task<IDataResult<object>> Handle(OrderChangeStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await orderDal.GetAsync(x => x.Id == request.Form.OrderId);
        if (order == null)
        {
            return new ErrorDataResult<object>(messagesRepository.NotFound(), HttpStatusCode.NotFound);
        }
        
        order.StatusId = request.Form.OrderStatusId;
        await orderDal.UpdateAsync(order);
        return new SuccessDataResult<object>(order, messagesRepository.Edited("status of order"));
    }
}