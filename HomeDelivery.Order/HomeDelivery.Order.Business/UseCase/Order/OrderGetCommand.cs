using System.Net;
using HomeDelivery.Order.Core.Responses;
using HomeDelivery.Order.Core.ResultResponses;
using HomeDelivery.Order.DataAccess.DataAccess;
using MediatR;

namespace HomeDelivery.Order.Business.UseCase.Order;

public class OrderGetCommand(Guid id) : IRequest<IDataResult<object>>
{
    public Guid Id { get; set;  } = id;
}


public class OrderGetCommandHandler(IOrderDal orderDal, IMessagesRepository messagesRepository) : IRequestHandler<OrderGetCommand, IDataResult<object>>
{
    
    public async Task<IDataResult<object>> Handle(OrderGetCommand request, CancellationToken cancellationToken)
    {
        var order = await orderDal.GetAsync(x => x.Id == request.Id);
        if (order == null)
        {
            return new ErrorDataResult<object>(messagesRepository.NotFound(), HttpStatusCode.NotFound);
        }
        
        return new SuccessDataResult<object>(order, "");
    }
}