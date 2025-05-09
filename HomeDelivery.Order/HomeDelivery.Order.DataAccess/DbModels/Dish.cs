using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeDelivery.Order.DataAccess.DbModels;

public class Dish : BaseEntity
{
    // Ссылка на категорию или секцию меню, к которой относится блюдо
    public Guid? MenuSectionId { get; set; }
    
    public string Name { get; set; }

    // Описание блюда
    public string Description { get; set; }

    // Цена блюда
    public decimal Price { get; set; }
    
    // URL изображения блюда
    public string? ImgUrl { get; set; }

    // Время приготовления блюда в минутах
    public int? PreparationTimeInMinutes { get; set; }
    
    public int? Calories { get; set; }
    
    public string? Category { get; set; }

    // Информация о наличии
    public bool IsAvailable { get; set; } = false;
    
    // Список ингредиентов блюда
    public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    
}