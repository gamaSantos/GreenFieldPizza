namespace GreenFieldPizza.Application.Customers;

public class CustomerId : BaseId
{
    public CustomerId(Guid value) : base(value)
    {
    }
    public static implicit operator Guid(CustomerId id) => id.Value;
    public static implicit operator CustomerId(Guid id) => new CustomerId(id);

}