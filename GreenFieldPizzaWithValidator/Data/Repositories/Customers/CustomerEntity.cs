using GreenFieldPizza.Application.Customers;

namespace GreenFieldPizza.Data.Repositories.Customers;

class CustomerEntity
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string StreetName { get; set; } = string.Empty;
    public string StreetNumber { get; set; } = string.Empty;
    public string? AdressObservation { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public string PhoneRegion { get; set; } = string.Empty;

    public Customer ToDomain()
    {
        var adress = new Address(StreetName, StreetNumber, AdressObservation);
        var phone = new Telephone(PhoneNumber, PhoneRegion);
        var name = new Name(FirstName, LastName);
        var loadResult = Customer.Load(Id, name, phone, adress);
        return loadResult.Match(
            errors => { throw new InvalidDataException(string.Join(',', errors)); },
            customer => customer
        );
    }

    public static CustomerEntity FromDomain(Customer domain) => new()
    {
        Id = domain.Id,
        FirstName = domain.Name.FirstName,
        LastName = domain.Name.LastName,
        AdressObservation = domain.Adress.Observation,
        StreetName = domain.Adress.StreetName,
        StreetNumber = domain.Adress.Number,
        PhoneNumber = domain.Phone.PhoneNumber,
        PhoneRegion = domain.Phone.RegionCode
    };
}