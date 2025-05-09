using System.ComponentModel.DataAnnotations;
using HomeDelivery.Order.Data.Enums;

namespace HomeDelivery.Order.Data.Dtos.Rating.RatingRequest;

public class RatingCreateRequest
{
    [Range(1, 5)] 
    public int? RatingValue { get; set; }
    [MaxLength(15000)]
    public string Comment { get; set; } = string.Empty;
    public required Guid EntityId { get; set; } 
    public required EntityType EntityType { get; set; }
}


public class RatingUpdateRequest
{
    [Range(1, 5)] 
    public int? RatingValue { get; set; }
    [MaxLength(15000)]
    public string Comment { get; set; } = string.Empty;
    public required Guid EntityId { get; set; } 
    public required EntityType EntityType { get; set; }
}