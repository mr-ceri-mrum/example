namespace HomeDelivery.Order.DataAccess.DbModels;

public class Address : BaseEntity
{
    public string Street { get; set; }
    public string BuildingNumber { get; set; }
    public string ApartmentNumber { get; set; }
    public string City { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string AdditionalInfo { get; set; }
    public string District { get; set; }
    public string Name { get; set; }
    public Guid UserId { get; set; }
}