namespace GreenFieldPizza.Application.Customers;

public abstract class BaseCustomer : IDeliverable
{
    protected BaseCustomer(Telephone phone, Adress adress)
    {
        Phone = phone;
        Adress = adress;
    }

    public Telephone Phone { get; }
    public Adress Adress { get; }
    public string GetAdressLine() => Adress.ToString();

    public IDeliverable AsDeliverable() => (IDeliverable)this;
}
