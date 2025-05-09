using System.Net;
using AutoMapper;
using FluentValidation;
using HomeDelivery.Order.Business.Services;
using HomeDelivery.Order.Core.GaneralHelpers;
using HomeDelivery.Order.Core.Responses;
using HomeDelivery.Order.Core.ResultResponses;
using HomeDelivery.Order.Data.Enums;
using HomeDelivery.Order.DataAccess.DataAccess;
using MediatR;

namespace HomeDelivery.Order.Business.UseCase.Order;

public class OrderAcceptCommand(Guid id) : IRequest<IDataResult<object>>
{
    public Guid Id { get; set; } = id;
}

internal class OrderAcceptCommandHandler(IOrderDal orderDal, IMessagesRepository messagesRepository, IMapper mapper, 
    IAuthInformationRepository authInformationRepository, Random random) 
    
    : IRequestHandler<OrderAcceptCommand, IDataResult<object>>
{
    
    public async Task<IDataResult<object>> Handle(OrderAcceptCommand request, CancellationToken cancellationToken)
    {
        var cookId = authInformationRepository.GetUserId();
        var order = await orderDal.GetAsync(x => x.Id == request.Id);
        if (order == null) return new ErrorDataResult<object>(messagesRepository.NotEmpty("Order is null"), HttpStatusCode.BadRequest);
        
        order.StatusId = (int)OrderStatus.InProgress;
        order.CookId = cookId;
        
        await orderDal.UpdateAsync(order);

        return new SuccessDataResult<object>(messagesRepository.Edited("Order of Status"));
    }
}