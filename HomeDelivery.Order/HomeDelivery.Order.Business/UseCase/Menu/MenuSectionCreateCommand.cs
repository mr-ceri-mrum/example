using AutoMapper;
using HomeDelivery.Order.Business.Services;
using HomeDelivery.Order.Core.ResultResponses;
using HomeDelivery.Order.Data.Dtos.MenuSection;
using HomeDelivery.Order.DataAccess.DataAccess;
using HomeDelivery.Order.DataAccess.DbModels;
using MediatR;

namespace HomeDelivery.Order.Business.UseCase.Menu;

public class MenuSectionCreateCommand(MenuSectionCreateDto form) : IRequest<IDataResult<object>>
{
    public MenuSectionCreateDto Form { get; set; } = form;
}

internal class MenuSectionCreateCommandHandler(IMapper mapper, IMenuSectionsDal menuSectionsDal,
    IAuthInformationRepository authInformationRepository) 
    : IRequestHandler<MenuSectionCreateCommand, IDataResult<object>>
{
    public async Task<IDataResult<object>> Handle(MenuSectionCreateCommand request, CancellationToken cancellationToken)
    {
        var menuSection = mapper.Map<MenuSection>(request.Form);
        menuSection.CookId = authInformationRepository.GetUser()!.Id;
        await menuSectionsDal.AddAsync(menuSection);
        return new SuccessDataResult<object>(menuSection, "");
    }
}