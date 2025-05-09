using HomeDelivery.Order.Data.Dtos.Address;

namespace HomeDelivery.Order.Data.Dtos;

public class OrderCreatDto
{
    public Guid? Id { get; set; }
    public required string Name { get; set; } 
    public int Count { get; set; } 
    public required string ImgUrl { get; set; } 
    public int OrderStatusId { get; set; }
    public required string Descriptions { get; set; }
    public AddressForCourierServiceDto? AddressDto { get; set; }
    public Guid CreatorId { get; set; }
    public Guid ReceiverId { get; set; }
    public int? CourierCode { get; set; }
    public int? UserCode { get; set; }
}

public class OrderCreateWorkerProducerDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; } 
    public int Count { get; set; } 
    public required string ImgUrl { get; set; } 
    
    public required string Descriptions { get; set; }
    public AddressDto? AddressDto { get; set; }
    public int OrderStatusId { get; set; }
    public Guid CreatorId { get; set; }
    public Guid ReceiverId { get; set; }
    public int? CourierCode { get; set; }
    public int? UserCode { get; set; }
    public Guid CookId { get; set; }
}

public class OrderCreatRequestDto
{
    public required string Name { get; set; } 
    public int Count { get; set; } 
    public required string ImgUrl { get; set; } 
    public required string Descriptions { get; set; }
    public AddressForCourierServiceDto? AddressDto { get; set; }
    public decimal Price { get; set; }
}


public class OrderChangeStatusDto
{
    public Guid OrderId { get; set; }
    public int OrderStatusId { get; set; }
}
