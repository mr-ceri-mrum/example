using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using HomeDelivery.Order.Core.Responses;
using HomeDelivery.Order.Core.ResultResponses;
using HomeDelivery.Order.Data.Dtos;
using HomeDelivery.Order.Data.Enums;
using HomeDelivery.Order.DataAccess.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace HomeDelivery.Order.Business.UseCase.Order;

public class OrderCompleteCommand(Guid orderId, int userCode): IRequest<IDataResult<object>>
{
    public Guid OrderId { get; } = orderId;
    public int UserCode { get; } = userCode;
}

public class OrderCompleteCommandHandler(IOrderDal orderDal, IMessagesRepository messagesRepository, 
    IHttpContextAccessor httpContextAccessor, IMediator mediator) 
    
    : IRequestHandler<OrderCompleteCommand, IDataResult<object>>
{
    public async Task<IDataResult<object>> Handle(OrderCompleteCommand request, CancellationToken cancellationToken)
    {
        var order = await orderDal.GetAsync(x => x.Id == request.OrderId);
        if (order == null) return new ErrorDataResult<object>(messagesRepository.NotFound(), HttpStatusCode.NotFound);
        if (order.UserCode != request.UserCode) return new ErrorDataResult<object>(messagesRepository.NotEqual("", "the code"), HttpStatusCode.NotFound);
        
        order.StatusId = (int)OrderStatus.Completed;
        await orderDal.UpdateAsync(order);
        await ChangeStatusForOrderServices(new OrderChangeStatusDto()
        {
            OrderId = order.Id,
            OrderStatusId = order.StatusId
        });
        return new SuccessDataResult<object>(messagesRepository.Response200());
    }
    
    [Authorize]
    private async Task<IDataResult<object>> ChangeStatusForOrderServices(OrderChangeStatusDto orderChangeStatusDto)
    {
        var apiUrl = "http://localhost:5047/CompleteOrderForOrderService";
        
        var token = httpContextAccessor.HttpContext.Request.Headers.Authorization.ToString().Replace("Bearer", "");
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var jsonContent = JsonSerializer.Serialize(orderChangeStatusDto);
        
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        
        var response = await httpClient.PostAsync(apiUrl, content);
            
        if (response.IsSuccessStatusCode)
        {
            // Читаем и десериализуем содержимое ответа
            var responseData = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<object>(responseData); // При необходимости измените тип
            
            // Возвращаем успешный результат
            return new SuccessDataResult<object>(result, "");
        }
        else
        {
            // Если произошла ошибка, возвращаем детали ошибки
            var errorMessage = await response.Content.ReadAsStringAsync();
            return new ErrorDataResult<object>($"Ошибка: {response.StatusCode}, Подробности: {errorMessage}" , HttpStatusCode.InternalServerError);
        }
    }
}