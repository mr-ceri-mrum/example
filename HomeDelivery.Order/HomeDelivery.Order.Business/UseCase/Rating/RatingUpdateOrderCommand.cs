using System.Net;
using AutoMapper;
using HomeDelivery.Order.Business.Services;
using HomeDelivery.Order.Core.Responses;
using HomeDelivery.Order.Core.ResultResponses;
using HomeDelivery.Order.Data.Dtos.Rating.RatingRequest;
using HomeDelivery.Order.DataAccess.DataAccess;
using MediatR;

namespace HomeDelivery.Order.Business.UseCase.Rating;

public class RatingUpdateOrderCommand(RatingUpdateRequest form) : IRequest<IDataResult<object>>
{
    public RatingUpdateRequest Form { get; set; } = form;
}

public class RatingUpdateOrderCommandHandler(
    IRatingDal ratingDal, IMessagesRepository messagesRepository, 
    IAuthInformationRepository authInformationRepository, IMapper mapper) 
    
    : IRequestHandler<RatingUpdateOrderCommand, IDataResult<object>>
{
    public async Task<IDataResult<object>> Handle(RatingUpdateOrderCommand request, CancellationToken cancellationToken)
    {
        // Check if the rating entity exists
        var entity = await ratingDal.GetAsync(x => x.Id == request.Form.EntityId);
        if (entity == null || (entity.ModifiedDate.HasValue && entity.ModifiedDate.Value.AddDays(7) < DateTime.UtcNow))
        {
            return new ErrorDataResult<object>(
                messagesRepository.NotFound("Rating not found"),
                HttpStatusCode.NotFound);
        }
        
        mapper.Map(request.Form, entity);
        
        entity.UserId = authInformationRepository.GetUserId();
        entity.ModifiedDate = DateTime.UtcNow;
        
        await ratingDal.UpdateAsync(entity);

        return new SuccessDataResult<object>(
            entity,
            messagesRepository.Edited("Rating successfully updated"));
    }
}