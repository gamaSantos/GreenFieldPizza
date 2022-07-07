namespace GreenFieldPizza.Application.Customers;

public class AnonymousCustomer : BaseCustomer
{
    private AnonymousCustomer(Telephone phone, Adress adress) : base(phone, adress)
    {
    }

    public static CommandResult<AnonymousCustomer> Create(Telephone phone, Adress adress)
    {
        var errors = new List<string>();
        if (!phone.IsValid()) errors.Add("Customer must have a valid phone");
        if (!adress.IsValid()) errors.Add("Customer must have a valid phone");
        if (errors.Any()) return CommandResult<AnonymousCustomer>.CreateWithErrors(errors);

        return CommandResult.CreateSuccess(new AnonymousCustomer(phone, adress));
    }


}