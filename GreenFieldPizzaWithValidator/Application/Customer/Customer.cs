namespace GreenFieldPizza.Application.Customers;

public class Customer : BaseCustomer
{
    private Customer(CustomerId id, Telephone phone, Address adress, Name name) : base(phone, adress)
    {
        Id = id;
        Name = name;
    }

    public CustomerId Id { get; }
    public Name Name { get; }

    public static ICommandResult<Customer> Create(Name name, Telephone phone, Address adress)
    {
        var validator = Validator.Create();
        validator.AddRule(phone.IsValid(), "Customer must have a valid phone");
        validator.AddRule(adress.IsValid(), "Customer must have a valid phone");
        validator.AddRule(name.IsValid(), "Customer must have a valid name");

        return validator.CreateCommand(new Customer(Guid.NewGuid(), phone, adress, name));
    }

    public static ICommandResult<Customer> Load(CustomerId id, Name name, Telephone phone, Address adress)
    {
        var validator = Validator.Create();
        validator.AddRule(id.IsValid(), "Invalid id");
        validator.AddRule(phone.IsValid(), "Customer must have a valid phone");
        validator.AddRule(adress.IsValid(), "Customer must have a valid phone");
        validator.AddRule(name.IsValid(), "Customer must have a valid name");

        return validator.CreateCommand(new Customer(id, phone, adress, name));
    }
}
