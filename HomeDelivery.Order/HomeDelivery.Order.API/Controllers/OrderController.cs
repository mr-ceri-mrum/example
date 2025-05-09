using System.Text.Json;
using Confluent.Kafka;
using HomeDelivery.Order.Business.Services;
using HomeDelivery.Order.Business.UseCase.Dish;
using HomeDelivery.Order.Business.UseCase.Menu;
using HomeDelivery.Order.Business.UseCase.Order;
using HomeDelivery.Order.Data.Dtos;
using HomeDelivery.Order.Data.Dtos.Dish;
using HomeDelivery.Order.Data.Dtos.MenuSection;
using HomeDelivery.Order.DataAccess.DataAccess;
using HomeDelivery.Order.DataAccess.DbModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HomeDelivery.Order.API.Controllers;
[Route("api/[controller]/")]
[ApiController]
[Authorize]
public class OrderController(IMediator mediator, 
    IMenuSectionsDal menuSectionsDal, IAuthInformationRepository authInformationRepository) 
    
    : BaseController
{
    [HttpPost("Create")]
    [Authorize(Roles = "USER, COOK")]
    public async Task<IActionResult> Create([FromForm] OrderCreatRequestDto form)
    {
        var result = await mediator.Send(new OrderCreateCommand(form));
        return Return(result);
    }
    
    [HttpPost("Accept")]
    [Authorize(Roles = "COOK")]
    public async Task<IActionResult> AcceptOrder([FromForm] Guid id)
    {
        var result = await mediator.Send(new OrderAcceptCommand(id));
        return Return(result);
    } 
    
    [HttpPost("Ready")]
    [Authorize(Roles = "COOK")]
    public async Task<IActionResult> MarkOrderAsReady([FromForm] Guid id)
    {
        var result = await mediator.Send(new OrderReadyCommand(id)); 
        return Return(result);
    }
    
    [HttpGet("Get")]
    [Authorize]
    public async Task<IActionResult> Get([FromQuery] Guid orderId)
    {
        var result = await mediator.Send(new OrderGetCommand(orderId)); 
        return Return(result);
    }
    
    [HttpGet("OrdersForCook")]
    [Authorize(Roles = "COOK")]
    public async Task<IActionResult> OrdersForCook([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        var result = await mediator.Send(new OrderGetOrdersForCookCommand(pageNumber, pageSize)); 
        return Return(result);
    }
    
    /// <summary>
    /// Создание Меню для повара
    /// </summary>
    /// <param name="form"></param>
    /// <returns></returns>
    [HttpPost("createMenuForCook")]
    [Authorize(Roles = "COOK")]
    public async Task<IActionResult> CreateMenuForCook([FromBody] MenuSectionCreateDto form)
    {
        var result = await mediator.Send(new MenuSectionCreateCommand(form)); 
        return Return(result);
    }
    
    [HttpPost("createDishForCook")]
    [Authorize(Roles = "COOK")]
    public async Task<IActionResult> CreateDishForCook([FromForm] DishCreateDto form, IFormFile file)
    {
        var result = await mediator.Send(new DishCreateCommand(form)); 
        return Return(result);
    }
    
    [HttpPost("getMyMenu")]
    [Authorize(Roles = "COOK")]
    public async Task<IActionResult> GetMyMenu()
    {
        var userId = authInformationRepository.GetUser()!.Id;
        
        var menuSections = await menuSectionsDal
            .GetAllAsQueryable(1,10,x => x.CookId == userId, 
                inc => inc.Dishes).ToListAsync();
        
        foreach (var val in menuSections.ToList().SelectMany(section => section.Dishes.ToList()))
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), val.ImgUrl ?? "");
            if (!System.IO.File.Exists(path)) continue;
            
            var bytes = await System.IO.File.ReadAllBytesAsync(path);
            var avatarBase64 = Convert.ToBase64String(bytes);
            val.ImgUrl = avatarBase64;
        }
        
        return Ok(menuSections);
    }
    
    [HttpPost("EditDishForCook")]
    [Authorize(Roles = "COOK")]
    public async Task<IActionResult> EditDishForCook([FromBody] DishEditDto form)
    {
        var result = await mediator.Send(new DishEditCommand(form)); 
        return Return(result);
    }
    
}

