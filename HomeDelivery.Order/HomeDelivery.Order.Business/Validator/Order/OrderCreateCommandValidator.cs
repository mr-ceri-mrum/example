using FluentValidation;
using HomeDelivery.Order.Business.UseCase.Order;

namespace HomeDelivery.Order.Business.Validator.Order;

public class OrderCreateCommandValidator: AbstractValidator<OrderCreateCommand>
{
    public OrderCreateCommandValidator()
    {
        
    }
}