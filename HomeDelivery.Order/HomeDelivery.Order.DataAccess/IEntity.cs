using System.ComponentModel.DataAnnotations;

namespace HomeDelivery.Order.DataAccess;

public interface IEntity<TKey>
{
     [Key]
     public TKey Id { get; set; }

     public DateTime? ModifiedDate { get; set; }
     public bool IsDeleted { get; set; } 
}