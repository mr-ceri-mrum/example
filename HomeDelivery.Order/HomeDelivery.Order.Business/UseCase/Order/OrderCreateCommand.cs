using System.Net;
using System.Text.Json;
using AutoMapper;
using FluentValidation;
using HomeDelivery.Order.Business.Services;
using HomeDelivery.Order.Business.UseCase.Address;
using HomeDelivery.Order.Core.GaneralHelpers;
using HomeDelivery.Order.Core.Hash;
using HomeDelivery.Order.Core.Responses;
using HomeDelivery.Order.Core.ResultResponses;
using HomeDelivery.Order.Data.Dtos;
using HomeDelivery.Order.DataAccess.DataAccess;
using MediatR;

namespace HomeDelivery.Order.Business.UseCase.Order;

public class OrderCreateCommand(OrderCreatRequestDto form) : IRequest<IDataResult<object>>
{
    public OrderCreatRequestDto Form { get; set; } = form;
}

internal class OrderCreateCommandHandler( IMessagesRepository messagesRepository, IOrderDal orderDal,
    IXssRepository xssRepository, IMapper mapper, IValidator<OrderCreateCommand> validator, IEncoding encoding, 
    IAuthInformationRepository authInformationRepository, IMediator mediator, IStoreService storeService,
    IRedisService redisService) 
    
    : IRequestHandler<OrderCreateCommand, IDataResult<object>>
{
    public async Task<IDataResult<object>> Handle(OrderCreateCommand request, CancellationToken cancellationToken)
    {
        #region Validation
                
        var validationOfForm = await validator.ValidateAsync(request, cancellationToken);
        if (!validationOfForm.IsValid ) return new ErrorDataResult<object?>(messagesRepository.FormValidation(), HttpStatusCode.BadRequest,
                validationOfForm.Errors.Select(e => e.ErrorMessage).ToList())!;
            
        #endregion
        
        var entity = mapper.Map<DataAccess.DbModels.Order>(request.Form);
        
        entity.Description = request.Form.Descriptions;
            
        var userId = authInformationRepository.GetUserId();
            
        if (userId == Guid.Empty)
        {
            return new ErrorDataResult<object?>(messagesRepository.FormValidation(), HttpStatusCode.Forbidden,
                validationOfForm.Errors.Select(e => e.ErrorMessage).ToList())!;
        }
            
        entity.CreatorId = userId;
        if (request.Form.AddressDto != null)
        {
            var address = 
                await mediator.Send(new AddressCreateCommand(request.Form.AddressDto), cancellationToken);
            entity.AddressId = address.Data.Id;
        }
            
        if (request.Form is {  AddressDto: null }) entity.AddressId = request.Form.AddressDto.AddressId.Value;
            
        entity.ReceiverId = userId;
            
        entity.UserCode = await MathHelper.GenerateCodeForOrder();
        entity.CourierCode = await MathHelper.GenerateCodeForOrder();
            
        await orderDal.AddAsync(entity);
        
        await storeService.StorePhotoForOrderRequest(entity.Id);
        
        try
        {
            var redisKey = $"Order:{entity.Id}";
            var redisValue = JsonSerializer.Serialize(entity);
            
            await redisService.SetAsync(redisKey, redisValue, TimeSpan.FromHours(24)); // Сохраняем на 24 часа
            var test = await redisService.GetAsync(redisKey);
            
        }
        catch (Exception ex)
        {
            // Логируем ошибку, но не прерываем выполнение
            
        }
        
        return new SuccessDataResult<object>(entity, messagesRepository.Created("Order created"));
    }
}