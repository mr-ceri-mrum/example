using HomeDelivery.Order.Business.UseCase.Address;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeDelivery.Order.API.Controllers;
[Route("api/[controller]/")]
[ApiController]
[Authorize]
public class AddressController(IMediator mediator) : BaseController
{
    [HttpPost("address")]
    [Authorize]
    public async Task<IActionResult> Address()
    {
        var result = await mediator.Send(new AddressGetCommand());
        return Return(result);
    }
}