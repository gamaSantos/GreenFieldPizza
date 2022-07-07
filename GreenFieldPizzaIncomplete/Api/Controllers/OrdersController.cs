using GreenFieldPizza.Application.Handlers.Orders;
using GreenFieldPizza.Application.Orders;
using GreenFieldPizza.Application.Orders.Commands;
using GreenFieldPizza.Application.Orders.Querys;
using Microsoft.AspNetCore.Mvc;

namespace GreenfieldPizza.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController
{
    private readonly ICreateOrderCommandHandler _createHandler;
    private readonly IOrderQueryHandler _orderQueryHandler;
    public OrdersController(
        ICreateOrderCommandHandler createHandler, IOrderQueryHandler orderQueryHandler)
    {
        _createHandler = createHandler;
        _orderQueryHandler = orderQueryHandler;
    }

    [HttpPost("")]
    public async Task<CommandResult<Order>> CreateOrder(CreateOrderCommand command)
    {
        return await _createHandler.TryHandle(command);
    }

    [HttpGet]
    public string Health() => "ok";

    [HttpGet("{customerId:Guid}/list/{page:int?}")]
    public async Task<IEnumerable<Order>> List(Guid customerId, int page = 0)
    {
        return await _orderQueryHandler.ListByCustomer(new ListByCustomerQuery(customerId, page));
    }
}