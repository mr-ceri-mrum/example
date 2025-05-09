namespace HomeDelivery.Order.DataAccess.DbModels;

public class MenuSection : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public List<Dish> Dishes { get; set; } = new List<Dish>();
    
    public int? DisplayOrder { get; set; }
    public Guid CookId { get; set; }
}