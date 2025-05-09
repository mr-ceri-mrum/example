using System.ComponentModel.DataAnnotations;

namespace HomeDelivery.Order.Data.Dtos.MenuSection;

public class MenuSectionCreateDto
{
    [MaxLength(300)]
    public required string Name { get; set; }

    // Описание раздела меню
    public required string Description { get; set; }

    // Доступность раздела меню
    public bool IsActive { get; set; } = true;
    
}