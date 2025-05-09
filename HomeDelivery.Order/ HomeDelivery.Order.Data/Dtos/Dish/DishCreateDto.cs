namespace HomeDelivery.Order.Data.Dtos.Dish;

public class DishCreateDto
{
    public required string Name { get; set; }

    // Описание блюда
    public required string Description { get; set; }

    // Цена блюда
    public decimal Price { get; set; }
    
    // Время приготовления блюда в минутах
    public int? PreparationTimeInMinutes { get; set; }

    // Калории
    public int? Calories { get; set; }

    // Категория блюда
    public string? Category { get; set; }

    // Доступность блюда
    public bool IsAvailable { get; set; } = true;

    // Идентификатор раздела меню, к которому относится блюдо
    public required Guid MenuSectionId { get; set; } 

    // Список идентификаторов ингредиентов для блюда
    public List<Guid?> IngredientIds { get; set; } = new ();
}

public class DishEditDto
{
    public required Guid DishId { get; set; }
    public required string Name { get; set; }

    // Описание блюда
    public required string Description { get; set; }

    // Цена блюда
    public decimal Price { get; set; }

    // URL изображения блюда
    public string? ImgUrl { get; set; }

    // Время приготовления блюда в минутах
    public int? PreparationTimeInMinutes { get; set; }

    // Калории
    public int? Calories { get; set; }

    // Категория блюда
    public string? Category { get; set; }

    // Доступность блюда
    public bool IsAvailable { get; set; } = true;
    
    // Список идентификаторов ингредиентов для блюда
    public List<Guid?> IngredientIds { get; set; } = new ();
}