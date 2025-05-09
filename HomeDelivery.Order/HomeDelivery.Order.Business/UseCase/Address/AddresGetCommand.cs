using HomeDelivery.Order.Business.Services;
using HomeDelivery.Order.Core.ResultResponses;
using HomeDelivery.Order.DataAccess.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HomeDelivery.Order.Business.UseCase.Address;

public class AddressGetCommand:  IRequest<IDataResult<object>>
{
    
}

public class AddressGetCommandHandler(IAuthInformationRepository authInformationRepository, IAddressDal addressDal) : IRequestHandler<AddressGetCommand, IDataResult<object>>
{
    public async Task<IDataResult<object>> Handle(AddressGetCommand request, CancellationToken cancellationToken)
    {
        var address = await addressDal
            .GetAllAsQueryable(10, 30, x => x.UserId == authInformationRepository.GetUser()!.Id).ToListAsync(cancellationToken: cancellationToken);
        return new SuccessDataResult<object>(address, "");
    }
}