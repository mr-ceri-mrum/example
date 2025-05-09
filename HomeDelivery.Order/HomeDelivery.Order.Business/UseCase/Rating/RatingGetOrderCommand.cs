using System.Net;
using AutoMapper;
using HomeDelivery.Order.Core.Responses;
using HomeDelivery.Order.Core.ResultResponses;
using HomeDelivery.Order.DataAccess.DataAccess;
using MediatR;

namespace HomeDelivery.Order.Business.UseCase.Rating;

public class RatingGetOrderCommand(Guid entityId) : IRequest<IDataResult<object>>
{
    public Guid EntityId { get; set; } = entityId;
}

public class RatingGetOrderCommandHandler(
    IRatingDal ratingDal, IMessagesRepository messagesRepository, IMapper mapper) 
    : IRequestHandler<RatingGetOrderCommand, IDataResult<object>>
{
    public async Task<IDataResult<object>> Handle(RatingGetOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await ratingDal.GetAsync(x => x.Id == request.EntityId);
        if (entity == null)
        {
            return new ErrorDataResult<object?>(
                messagesRepository.NotFound("Rating not found"),
                HttpStatusCode.NotFound)!;
        }
        
        return new SuccessDataResult<object?>(
            entity,
            "Rating retrieved successfully")!;
    }
}