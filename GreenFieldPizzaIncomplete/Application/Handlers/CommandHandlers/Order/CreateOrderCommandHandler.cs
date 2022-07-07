using GreenFieldPizza.Application.Customers;
using GreenFieldPizza.Application.Customers.Repositories;
using GreenFieldPizza.Application.Orders;
using GreenFieldPizza.Application.Orders.Commands;
using GreenFieldPizza.Application.Orders.Repositories;
using GreenFieldPizza.Application.Stock;
using GreenFieldPizza.Application.Stock.Services.Interfaces;

namespace GreenFieldPizza.Application.Handlers.Orders;

public interface ICreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Order> { }

public class CreateOrderCommandHandler : BaseCommandHandler<CreateOrderCommand, Order>, ICreateOrderCommandHandler
{
    private readonly IStockManager _stockManager;
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;

    public CreateOrderCommandHandler(IStockManager stockManager,
                                     IOrderRepository orderRepository,
                                     ICustomerRepository customerRepository)
    {
        _stockManager = stockManager;
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
    }

    protected override async Task<CommandResult<Order>> Handle(CreateOrderCommand command)
    {
        var buildPizzasResult = await BuildPizzas(command);
        if (buildPizzasResult.TryGetContent(out var pizzas) == false) return buildPizzasResult.ToCommandResultWithErrors<Order>();
        var buildDeliverableResult = await BuildDeliverable(command);
        if (buildDeliverableResult.TryGetContent(out var deliverable) == false) return buildDeliverableResult.ToCommandResultWithErrors<Order>();

        var createOrderResult = Order.Create(pizzas, deliverable);
        if (createOrderResult.TryGetContent(out var order) == false) return createOrderResult;
        await _orderRepository.Insert(order);
        return createOrderResult;
    }

    private async Task<CommandResult<IDeliverable>> BuildDeliverable(CreateOrderCommand command)
    {
        if (command.CustomerId.HasValue) return await GetCustomer(command.CustomerId);
        if (command.TryCreatePhone(out var phone) == false) return CommandResult<IDeliverable>.CreateWithErrors("couldn't recognize the contact information");
        if (command.TryCreateAdress(out var adress) == false) return CommandResult<IDeliverable>.CreateWithErrors("couldn't recognize the adress");
        var createResult = AnonymousCustomer.Create(phone, adress);
        if (createResult.TryGetContent(out var anonymous) == false) return createResult.ToCommandResultWithErrors<IDeliverable>();
        return CommandResult.CreateSuccess((IDeliverable)anonymous);
    }

    private async Task<CommandResult<List<Pizza>>> BuildPizzas(CreateOrderCommand command)
    {
        var pizzas = new List<Pizza>();
        var withdraw = await GetWithdrawFromStock(command);
        if (withdraw.IsCompleted == false) return CommandResult<List<Pizza>>.CreateWithErrors("Some flavors are missing in stock");
        foreach (var createPizzaCommand in command.Pizzas)
        {
            var flavors = createPizzaCommand.Flavors
                    .Select(flavorSku => withdraw.Items.First(pc => pc.Sku == flavorSku))
                    .Select(si => new Flavor(si.Sku, si.BasePrice));

            var pizzaCreateResult = Pizza.Create(flavors);
            if (pizzaCreateResult.TryGetContent(out var pizza) == false) return pizzaCreateResult.ToCommandResultWithErrors<List<Pizza>>();
            pizzas.Add(pizza);
        }
        return CommandResult.CreateSuccess(pizzas);
    }

    private async Task<Withdraw> GetWithdrawFromStock(CreateOrderCommand command)
    {
        var neededSku = command.Pizzas
                            .SelectMany(p => p.Flavors)
                            .GroupBy(i => i)
                            .Select(g => new { sku = g.Key, quantity = g.Count() });
        var withdrawItems = WithdrawItems.CreateEmpty();
        foreach (var item in neededSku)
        {
            withdrawItems.TryAddItem(item.sku, (uint)item.quantity);
        }
        return await _stockManager.WithdrawFromStock(withdrawItems);
    }

    private async Task<CommandResult<IDeliverable>> GetCustomer(CustomerId customerId)
    {
        var customer = await _customerRepository.GetById(customerId);
        if (customer == null) return CommandResult<IDeliverable>.CreateWithErrors("Customer not found");
        return CommandResult.CreateSuccess((IDeliverable)customer);
    }
}