using HomeDelivery.Order.Business.UseCase.Dish;
using HomeDelivery.Order.Business.UseCase.Order;
using HomeDelivery.Order.Data.Dtos;
using HomeDelivery.Order.Data.Dtos.Paginate;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeDelivery.Order.API.Controllers;
[Route("api/[controller]/")]
[ApiController]
[Authorize(Roles = "USER")]
public class UserOrderController(IMediator mediator): BaseController
{
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromForm] OrderCreatRequestDto form, IFormFile? file)
    {
        var result = await mediator.Send(new OrderCreateCommand(form));
        return Return(result);
    }
    
    /// <summary>
    /// получение всех заказов Для юзера который он создавал 
    /// </summary>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>
    /// <returns>Orders</returns>
    [HttpGet("MyOrdersForUser")]
    [Authorize(Roles = "USER")]
    public async Task<IActionResult> MyOrdersForUser([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var result = await mediator.Send(new OrderGetMyOrdersForUserCommand(pageNumber, pageSize)); 
        return Return(result);
    }
    
    /// <summary>
    /// Завершение заказа после того как пользователь пролучил от курьера
    /// </summary>
    /// <param name="orderId"></param>
    /// <param name="userCode"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("CompleteOrder")]
    [Authorize(Roles = "USER")]
    public async Task<IActionResult> CompleteOrder([FromForm] Guid orderId, [FromForm] int userCode, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new OrderCompleteCommand(orderId, userCode), cancellationToken); 
        return Return(result);
    }
    
    /// <summary>
    /// получение блюд для пользователя
    /// </summary>
    /// <param name="form"></param>
    /// <returns></returns>
    [HttpGet("GetDishesForUser")]
    [Authorize(Roles = "USER")]
    public async Task<IActionResult> GetDishesForUser([FromQuery]PaginationDto form)
    {
        var result = await mediator.Send(new DishGetAllForUserCommand(form));
        return Return(result);
    }
    
    [HttpGet("GetDish")]
    [Authorize(Roles = "USER")]
    public async Task<IActionResult> GetDish([FromQuery] Guid dishId)
    {
        var result = await mediator.Send(new DishGetUserCommand(dishId));
        return Return(result);
    }
}