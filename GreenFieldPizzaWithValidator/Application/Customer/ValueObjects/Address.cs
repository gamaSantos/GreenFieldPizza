namespace GreenFieldPizza.Application.Customers;

public class Address
{
    public Address(string streetName, string number, string? observation = null)
    {
        StreetName = streetName;
        Number = number;
        Observation = observation;
    }

    public string StreetName { get; }
    public string Number { get; }
    public string? Observation { get; }

    public override string ToString()
    {
        return $"{StreetName}, {Number}{(string.IsNullOrWhiteSpace(Observation) ? string.Empty : " " + Observation)}";
    }

    internal bool IsValid() => string.IsNullOrWhiteSpace(StreetName) == false && string.IsNullOrWhiteSpace(Number) == false;
}