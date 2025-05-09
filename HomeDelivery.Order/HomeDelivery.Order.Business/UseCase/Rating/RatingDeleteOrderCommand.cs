using System.Net;
using HomeDelivery.Order.Business.Services;
using HomeDelivery.Order.Core.Responses;
using HomeDelivery.Order.Core.ResultResponses;
using HomeDelivery.Order.DataAccess.DataAccess;
using MediatR;

namespace HomeDelivery.Order.Business.UseCase.Rating;

public class RatingDeleteOrderCommand(Guid entityId) : IRequest<IDataResult<object>>
{
    public Guid EntityId { get; set; } = entityId;
}

public class RatingDeleteOrderCommandHandler(
    IRatingDal ratingDal, IMessagesRepository messagesRepository) 
    : IRequestHandler<RatingDeleteOrderCommand, IDataResult<object>>
{
    public async Task<IDataResult<object>> Handle(RatingDeleteOrderCommand request, CancellationToken cancellationToken)
    {
        // Retrieve the rating entity by Id
        var entity = await ratingDal.GetAsync(x => x.Id == request.EntityId);
        if (entity == null)
        {
            return new ErrorDataResult<object>(
                messagesRepository.NotFound("Rating not found"),
                HttpStatusCode.NotFound);
        }

        // Check if the entity is eligible for deletion (optional)
        if (entity.ModifiedDate.HasValue && entity.ModifiedDate.Value.AddDays(7) < DateTime.UtcNow)
        {
            return new ErrorDataResult<object>(
                messagesRepository.ValidValueInEnum("","Rating cannot be deleted after 7 days"),
                HttpStatusCode.BadRequest);
        }
        await ratingDal.DeleteAsync(entity);

        return new SuccessDataResult<object>(
            HttpStatusCode.OK,
            messagesRepository.Deleted());
    }
}