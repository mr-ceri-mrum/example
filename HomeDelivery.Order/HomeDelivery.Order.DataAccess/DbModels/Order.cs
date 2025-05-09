using System.ComponentModel.DataAnnotations.Schema;
using HomeDelivery.Order.Data.Enums;

namespace HomeDelivery.Order.DataAccess.DbModels
{
    public class Order : BaseEntity
    {
        public int StatusId { get; set; } = (int)OrderStatus.Create;
        public string Name { get; set; } 
        public decimal Price { get; set; } 
        public string ImgUrl { get; set; } = string.Empty;
        public string Description { get; set; } 
        public Guid CreatorId { get; set; } 
        public Guid ReceiverId { get; set; } 
        public Guid AddressId { get; set; }
        [ForeignKey("AddressId")]
        public Address? Address { get; set; }
        public Guid? CookId { get; set; } 
        public Guid? CourierId { get; set; } 
        public int Count { get; set; }
        public int CourierCode { get; set; }
        public int UserCode { get; set; }
        
        public Guid? DishId { get; set; }
        [ForeignKey("DishId")]
        public Dish? Dish { get; set; }
    }
}
