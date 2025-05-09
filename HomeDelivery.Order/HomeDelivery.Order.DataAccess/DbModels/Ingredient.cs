namespace HomeDelivery.Order.DataAccess.DbModels;

public class Ingredient: BaseEntity
{
    public string Name { get; set; }

    // Количество ингредиента, например, 100 (грамм)
    public decimal Quantity { get; set; }

    // Единица измерения, например, "грамм", "мл", "штук"
    public string Unit { get; set; }
    
    // Ссылка на блюдо, которому принадлежи ингредиент
    public int? DishId { get; set; }
    public Dish? Dish { get; set; }
}