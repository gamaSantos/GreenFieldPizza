using System.Diagnostics.CodeAnalysis;
using GreenFieldPizza.Application.Customers;

namespace GreenFieldPizza.Application.Orders.Commands;

public class CreateOrderCommand : ICommand
{
    public Guid? CustomerId { get; set; }
    public string? StreetName { get; set; }
    public string? StreetNumber { get; set; }
    public string? AdressObservation { get; set; }
    public string? PhoneRegion { get; set; }
    public string? PhoneNumber { get; set; }

    public List<CreatePizzaCommand> Pizzas { get; set; } = new List<CreatePizzaCommand>();

    public bool TryCreatePhone([NotNullWhen(returnValue: true)] out Telephone? phone)
    {
        phone = null;
        if (string.IsNullOrWhiteSpace(PhoneRegion) || string.IsNullOrWhiteSpace(PhoneNumber)) return false;
        phone = new Telephone(PhoneNumber, PhoneRegion);
        return true;
    }

    public bool TryCreateAdress([NotNullWhen(true)] out Adress? adress)
    {
        adress = null;
        if (string.IsNullOrWhiteSpace(StreetName) || string.IsNullOrWhiteSpace(StreetNumber)) return false;
        adress = new Adress(StreetName, StreetNumber, AdressObservation);
        return true;
    }

    public ValidationResult IsValid()
    {
        var result = new ValidationResult();
        if (Pizzas == null || Pizzas.Any() == false)
        {
            result.Add("An order must have at least one pizza");
        }
        foreach (var pizza in Pizzas!) result.Add(pizza.IsValid());

        if (CustomerId != Guid.Empty) return result;

        if (string.IsNullOrWhiteSpace(StreetName)) result.Add("A street name is necessary to create an anonymous order");
        if (string.IsNullOrWhiteSpace(StreetNumber)) result.Add("A street number is necessary to create an anonymous order");

        if (string.IsNullOrWhiteSpace(PhoneRegion)) result.Add("A phone region is necessary to create an anonymous order");
        if (string.IsNullOrWhiteSpace(PhoneNumber)) result.Add("A phone number is necessary to create an anonymous order");

        return result;
    }
}

public class CreatePizzaCommand : ICommand
{
    public List<string> Flavors { get; set; } = new List<string>();

    public ValidationResult IsValid()
    {
        if (Flavors.Any() == false) return new ValidationResult("A pizza must have at least one flavor");
        return new ValidationResult();
    }
}