using System.ComponentModel.DataAnnotations;

namespace HomeDelivery.Order.Data.Dtos.Address;

public class AddressDto
{
    public Guid? AddressId { get; set; } = Guid.Empty;
    public string Street { get; set; }
    
    public string BuildingNumber { get; set; }
    public string ApartmentNumber { get; set; }
    public string City { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string AdditionalInfo { get; set; }
    public string District { get; set; }
}

public class AddressForCourierServiceDto
{
    public Guid? AddressId { get; set; }
    public string Street { get; set; }
    public string BuildingNumber { get; set; }
    public string ApartmentNumber { get; set; }
    public string City { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string AdditionalInfo { get; set; }
    public string District { get; set; }
    public Guid? UserId { get; set; }
}