using GreenFieldPizza.Application.Customers;

namespace GreenFieldPizza.Application.Core;

public interface IDeliverable
{
    public string GetAdressLine();
    public Telephone Phone { get; }
    public Adress Adress { get; }
}