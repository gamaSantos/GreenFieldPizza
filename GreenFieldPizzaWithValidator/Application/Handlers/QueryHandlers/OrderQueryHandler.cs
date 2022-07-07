using GreenFieldPizza.Application.Orders;
using GreenFieldPizza.Application.Orders.Querys;
using GreenFieldPizza.Application.Orders.Repositories;

namespace GreenFieldPizza.Application.Handlers.Orders;

public interface IOrderQueryHandler
{
    Task<IEnumerable<Order>> ListByCustomer(ListByCustomerQuery query);
}

public class OrderQueryHandler : IOrderQueryHandler
{
    private readonly IOrderReadOnlyRepository _orderReadOnlyRepository;

    public OrderQueryHandler(IOrderReadOnlyRepository orderReadOnlyRepository)
    {
        _orderReadOnlyRepository = orderReadOnlyRepository;
    }

    public Task<IEnumerable<Order>> ListByCustomer(ListByCustomerQuery query)
    {
        return _orderReadOnlyRepository.ListByCustomer(query);
    }
}