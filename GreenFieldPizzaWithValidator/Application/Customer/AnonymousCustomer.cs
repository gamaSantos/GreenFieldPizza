namespace GreenFieldPizza.Application.Customers;

public class AnonymousCustomer : BaseCustomer
{
    private AnonymousCustomer(Telephone phone, Address adress) : base(phone, adress)
    {
    }

    public static ICommandResult<AnonymousCustomer> Create(Telephone phone, Address address)
    {
        var validator = Validator.Create();
        validator.AddRule(phone.IsValid(), "Customer must have a valid phone");
        validator.AddRule(address.IsValid(), "Customer must have a valid phone");

        return validator.CreateCommand(new AnonymousCustomer(phone, address));
    }


}