using System.Net;
using AutoMapper;
using HomeDelivery.Order.Business.Services;
using HomeDelivery.Order.Core.Responses;
using HomeDelivery.Order.Core.ResultResponses;
using HomeDelivery.Order.Data.Dtos.Rating.RatingRequest;
using HomeDelivery.Order.Data.Enums;
using HomeDelivery.Order.DataAccess.DataAccess;
using MediatR;

namespace HomeDelivery.Order.Business.UseCase.Rating;

public class RatingCreateOrderCommand(RatingCreateRequest form) : IRequest<IDataResult<object>>
{
    public RatingCreateRequest Form { get; set; } = form;
}

public class RatingCreateOrderCommandHandler(
    IRatingDal ratingDal, IMessagesRepository messagesRepository, 
    IAuthInformationRepository authInformationRepository,
    IOrderDal orderDal, IMapper mapper) 
    
    : IRequestHandler<RatingCreateOrderCommand, IDataResult<object>>
{
    public async Task<IDataResult<object>> Handle(RatingCreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderDal.GetId(x => x.Id == request.Form.EntityId);
        
        if (order == null) return new ErrorDataResult<object>("NotFound", HttpStatusCode.NotFound);
        
        var entity = mapper.Map<DataAccess.DbModels.Rating>(request.Form);
            
        entity.UserId = authInformationRepository.GetUserId();
        await ratingDal.AddAsync(entity);
        return new SuccessDataResult<object>("", 
            messagesRepository.Created("Ваша оценка поставленна"));
    }
    
}