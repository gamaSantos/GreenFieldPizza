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

    public ICommandResult<Telephone> TryCreatePhone()
    {
        var number = PhoneNumber ?? string.Empty;
        var region = PhoneRegion ?? string.Empty;
        var phone = new Telephone(number, region);
        var validation = Validator.Create(phone);
        validation.AddRule(x => x.IsValid(), "couldn't recognize the contact information");

        return validation.CreateCommand(phone);
    }

    public ICommandResult<Address> TryCreateAddress()
    {
        var streetName = StreetName ?? string.Empty;
        var number = StreetNumber ?? string.Empty;
        var adress = new Address(streetName, number, AdressObservation);
        var validation = Validator.Create(adress);
        validation.AddRule(x => x.IsValid(), "couldn't recognize the address");
        return validation.CreateCommand(adress);
    }

    public ValidationResult IsValid()
    {
        var validator = Validator.Create(this);
        validator.AddRule(x => x.Pizzas.Any(), "An order must have at least one pizza");
        foreach (var pizza in Pizzas) validator.Append(pizza.IsValid());
        if (CustomerId != Guid.Empty) validator.Append(ValidateAnonymousCustomer());
        return validator.Validate();
    }

    private ValidationResult ValidateAnonymousCustomer()
    {
        var validator = Validator.Create(this);
        validator.AddRule(x => string.IsNullOrWhiteSpace(x.StreetName) == false, "A street name is necessary to create an anonymous order");
        validator.AddRule(x => string.IsNullOrWhiteSpace(x.StreetNumber) == false, "A street number is necessary to create an anonymous order");
        validator.AddRule(x => string.IsNullOrWhiteSpace(x.PhoneRegion) == false, "A phone region is necessary to create an anonymous order");
        validator.AddRule(x => string.IsNullOrWhiteSpace(x.PhoneNumber) == false, "A phone number is necessary to create an anonymous order");
        return validator.Validate();
    }
}

public class CreatePizzaCommand : ICommand
{
    public List<string> Flavors { get; set; } = new List<string>();

    public ValidationResult IsValid()
    {
        var valiadtionBuilder = Validator.Create(this);
        valiadtionBuilder.AddRule(x => x.Flavors.Any(), "A pizza must have at least one flavor");
        return new ValidationResult();
    }
}