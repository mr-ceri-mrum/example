using HomeDelivery.Order.Business.UseCase.Cook;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeDelivery.Order.API.Controllers;
[Route("api/[controller]/")]
[ApiController]
[Authorize(Roles = "COOK")]
public class CookController(IMediator mediator) : BaseController
{
    [HttpGet("statistics")]
    public async Task<IActionResult> StatisticsOfCook([FromForm] int filterNumber)
    {
        var result = await mediator.Send(new CookGetStatisticsCommand(filterNumber));
        return Return(result);
    }
}