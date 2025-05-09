using HomeDelivery.Order.Business.UseCase.Order;
using HomeDelivery.Order.Data.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeDelivery.Order.API.Controllers;
[Authorize]
[Route("api/[controller]/")]
[ApiController]
public class OrderForCourierServiceController(IMediator mediator) : BaseController
{ 
    [HttpPost("TakeTheOrder")]
    [Authorize(Roles = "COURIER")]
    public async Task<IActionResult> TakeTheOrder([FromBody] OrderChangeStatusDto form)
    {
        var result = await mediator.Send(new OrderChangeStatusCommand(form));
        return Return(result);
    }
}