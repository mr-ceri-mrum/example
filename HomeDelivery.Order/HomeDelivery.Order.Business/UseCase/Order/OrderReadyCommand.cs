using System.Net;
using System.Text.Json;
using HomeDelivery.Order.Business.Services;
using HomeDelivery.Order.Core.Responses;
using HomeDelivery.Order.Core.ResultResponses;
using HomeDelivery.Order.Data.Constants;
using HomeDelivery.Order.Data.Enums;
using HomeDelivery.Order.DataAccess.DataAccess;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace HomeDelivery.Order.Business.UseCase.Order;

public class OrderReadyCommand(Guid id): IRequest<IDataResult<object>>
{
    public Guid Id { get; set; } = id;
}
    
internal class OrderReadyCommandHandler(IOrderDal orderDal, IConfiguration configuration,
    IMessagesRepository messagesRepository, IAuthInformationRepository authInformationRepository) 
    
    
    : IRequestHandler<OrderReadyCommand, IDataResult<object>>
{
    public async Task<IDataResult<object>> Handle(OrderReadyCommand request, CancellationToken cancellationToken)
    {
        var cookId = authInformationRepository.GetUserId();
        
        var order = await orderDal.GetAsync(x => x.Id == request.Id && x.StatusId == (int)OrderStatus.InProgress, 
            inc => inc.Address!);
        
        if (order == null) 
            return new ErrorDataResult<object>(messagesRepository.Edited("Order in work"), HttpStatusCode.BadRequest);
        if (order.Address == null) 
            return new ErrorDataResult<object>(messagesRepository.Edited("Order in work"), HttpStatusCode.BadRequest);
        
        order.StatusId = (int)OrderStatus.AwaitingCourier;
        order.CookId = cookId;
        
        await orderDal.UpdateAsync(order);

        var orderCreatDto = new 
        {
            order.Id,
            order.StatusId,
            order.AddressId,
            order.Address.Latitude,
            order.Address.Longitude,
            order.CookId,
            order.ReceiverId,
            order.CreatorId,
            order.CourierCode
        };
        
        var json = JsonSerializer.Serialize(orderCreatDto);
        
        using (var producerService = new ProducerOrderService(configuration))
        {
            await producerService.ProduceAsync(Topics.ORDER_CREATE, json);
        }
        
        return new SuccessDataResult<object>(messagesRepository.Edited("Order awaiting Courier"));
    }
    
}
