namespace GreenFieldPizza.Application.Customers;

public abstract class BaseCustomer : IDeliverable
{
    protected BaseCustomer(Telephone phone, Address adress)
    {
        Phone = phone;
        Adress = adress;
    }

    public Telephone Phone { get; }
    public Address Adress { get; }
    public string GetAdressLine() => Adress.ToString();

    public IDeliverable AsDeliverable() => (IDeliverable)this;
}
