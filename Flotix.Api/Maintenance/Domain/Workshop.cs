using Flotix.Api.Shared.Domain;

namespace Flotix.Api.Maintenance.Domain;

public sealed class Workshop : Entity
{
    private Workshop() { }

    public Workshop(string mechanicId, string city, string district, string address, string phone, decimal rating, List<string> tags)
    {
        MechanicId = mechanicId;
        Update(city, district, address, phone, rating, tags);
    }

    public string MechanicId { get; private set; } = null!;

    public string City { get; private set; } = null!;

    public string District { get; private set; } = null!;

    public string Address { get; private set; } = null!;

    public string Phone { get; private set; } = null!;

    public decimal Rating { get; private set; }

    public List<string> Tags { get; private set; } = [];

    public void Update(string city, string district, string address, string phone, decimal rating, List<string> tags)
    {
        City = city;
        District = district;
        Address = address;
        Phone = phone;
        Rating = rating;
        Tags = tags;
    }
}
