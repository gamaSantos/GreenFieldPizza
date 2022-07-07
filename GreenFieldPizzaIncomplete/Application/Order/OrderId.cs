namespace GreenFieldPizza.Application.Orders;

public class OrderId : BaseId
{
    public OrderId(Guid value) : base(value)
    {
    }
    public static implicit operator Guid(OrderId id) => id.Value;
    public static implicit operator OrderId(Guid id) => new(id);
}