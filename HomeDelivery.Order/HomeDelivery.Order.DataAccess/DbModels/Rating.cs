

using System.ComponentModel.DataAnnotations;
using HomeDelivery.Order.Data.Enums;

namespace HomeDelivery.Order.DataAccess.DbModels;

public class Rating: BaseEntity
{
    [Range(1, 5)] 
    public int? RatingValue { get; set; }
    public Guid UserId { get; set; }
    [MaxLength(15000)]
    public string Comment { get; set; } = string.Empty;
    public Guid EntityId { get; set; } 
    public EntityType EntityType { get; set; }
}