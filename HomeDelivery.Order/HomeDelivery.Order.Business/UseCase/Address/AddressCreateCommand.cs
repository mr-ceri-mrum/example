using AutoMapper;
using HomeDelivery.Order.Business.Services;
using HomeDelivery.Order.Core.Responses;
using HomeDelivery.Order.Core.ResultResponses;
using HomeDelivery.Order.Data.Dtos.Address;
using HomeDelivery.Order.DataAccess.DataAccess;
using MediatR;

namespace HomeDelivery.Order.Business.UseCase.Address;

public class AddressCreateCommand(AddressForCourierServiceDto form) :  IRequest<IDataResult<DataAccess.DbModels.Address>>
{
    public AddressForCourierServiceDto Form { get; } = form;
}


public class AddressCreateCommandHandle(IAddressDal addressDal, IMapper mapper, IMessagesRepository messagesRepository, IAuthInformationRepository authInformationRepository) 
    : IRequestHandler<AddressCreateCommand, IDataResult<DataAccess.DbModels.Address>>
{
    public async Task<IDataResult<DataAccess.DbModels.Address>> Handle(AddressCreateCommand request, CancellationToken cancellationToken)
    {
        if (await addressDal.AnyAsync(x => x.Id == request.Form.AddressId) && request.Form.AddressId != null)
        {
            var addressIsExist = await addressDal.GetAsync(x => x.Id == request.Form.AddressId);
            return new SuccessDataResult<DataAccess.DbModels.Address>
                (addressIsExist!, messagesRepository.Response200());
        }
        
        var address = mapper.Map<DataAccess.DbModels.Address>(request.Form);
        address.Name = request.Form.District;
        address.UserId = authInformationRepository.GetUser()!.Id;
        await addressDal.AddAsync(address);
        return new SuccessDataResult<DataAccess.DbModels.Address>(address, messagesRepository.Created("address created"));
    }
}