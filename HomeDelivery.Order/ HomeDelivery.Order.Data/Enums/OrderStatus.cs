namespace HomeDelivery.Order.Data.Enums;

public enum OrderStatus
{
    Create = 1,
    InProgress  = 2,
    AwaitingCourier = 3,
    InTransit = 4,
    Cancelled = 5,    
    Completed = 6,    
}