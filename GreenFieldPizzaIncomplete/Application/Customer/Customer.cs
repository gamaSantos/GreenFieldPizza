namespace GreenFieldPizza.Application.Customers;

public class Customer : BaseCustomer
{
    private Customer(CustomerId id, Telephone phone, Adress adress, Name name) : base(phone, adress)
    {
        Id = id;
        Name = name;
    }

    public CustomerId Id { get; }
    public Name Name { get; }

    public static CommandResult<Customer> Create(Name name, Telephone phone, Adress adress)
    {
        var errors = new List<string>();
        if (!phone.IsValid()) errors.Add("Customer must have a valid phone");
        if (!adress.IsValid()) errors.Add("Customer must have a valid phone");
        if (!name.IsValid()) errors.Add("Customer must have a valid name");
        if (errors.Any()) return CommandResult<Customer>.CreateWithErrors(errors);

        return CommandResult.CreateSuccess(new Customer(Guid.NewGuid(), phone, adress, name));
    }

    public static CommandResult<Customer> Load(CustomerId id, Name name, Telephone phone, Adress adress)
    {
        var errors = new List<string>();
        if (!id.IsValid()) errors.Add("Invalid id");
        if (!phone.IsValid()) errors.Add("Customer must have a valid phone");
        if (!adress.IsValid()) errors.Add("Customer must have a valid phone");
        if (!name.IsValid()) errors.Add("Customer must have a valid name");
        if (errors.Any()) return CommandResult<Customer>.CreateWithErrors(errors);

        return CommandResult.CreateSuccess(new Customer(id, phone, adress, name));
    }
}
