using HomeDelivery.Order.Business.UseCase.Rating;
using HomeDelivery.Order.Data.Dtos.Rating.RatingRequest;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeDelivery.Order.API.Controllers;

[Route("api/[controller]/")]
[ApiController]
[Authorize(Roles = "USER")]
public class RatingController(IMediator mediator) : BaseController
{
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromForm] RatingCreateRequest form)
    {
        var result = await mediator.Send(new RatingCreateOrderCommand(form));
        return Return(result);
    }
    
}