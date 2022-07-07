using GreenFieldPizza.Application.Orders.Querys;

namespace GreenFieldPizza.Application.Orders.Repositories;

public interface IOrderRepository
{
    Task<bool> Insert(Order order);

}

public interface IOrderReadOnlyRepository
{
    Task<IEnumerable<Order>> ListByCustomer(ListByCustomerQuery query);
}