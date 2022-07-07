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

    protected override async Task<ICommandResult<Order>> Handle(CreateOrderCommand command)
    {
        var buildDeliverableResult = await BuildDeliverable(command);
        var buildPizzasResult = await BuildPizzas(command);
        return buildPizzasResult.Match(
            errors => CommandResult.CreateFailed<Order>(errors),
            pizzas =>
            {
                return buildDeliverableResult.Match(
                    errors => CommandResult.CreateFailed<Order>(errors),
                    deliverable =>
                    {
                        var createOrderResult = Order.Create(pizzas, deliverable);
                        createOrderResult.Do(async order => await _orderRepository.Insert(order));
                        return createOrderResult;
                    }
                );
            }
        );
    }

    private async Task<ICommandResult<IDeliverable>> BuildDeliverable(CreateOrderCommand command)
    {
        if (command.CustomerId.HasValue) return await GetCustomer(command.CustomerId);
        return CreateDeliverable(command);
    }

    private static ICommandResult<IDeliverable> CreateDeliverable(CreateOrderCommand command)
    {
        var phoneCreate = command.TryCreatePhone();
        var addressCreate = command.TryCreateAddress();
        return phoneCreate.Match(
            errors => CommandResult.CreateFailed<IDeliverable>(errors),
            phone => addressCreate.Match(
                adressErrors => CommandResult.CreateFailed<IDeliverable>(adressErrors),
                address =>
                {
                    var createResult = AnonymousCustomer.Create(phone, address);
                    return createResult.Match(
                        errors => CommandResult.CreateFailed<IDeliverable>(errors),
                        anonymous => CommandResult.CreateSuccess((IDeliverable)anonymous)
                    );
                }
            )
        );
    }

    private async Task<ICommandResult<IEnumerable<Pizza>>> BuildPizzas(CreateOrderCommand command)
    {
        var resultCollection = new CommandResultCollection<Pizza>();
        var withdraw = await GetWithdrawFromStock(command);
        if (withdraw.IsCompleted == false) return CommandResult.CreateFailed<IEnumerable<Pizza>>("Some flavors are missing in stock");
        foreach (var createPizzaCommand in command.Pizzas)
        {
            var flavors = createPizzaCommand.Flavors
                    .Select(flavorSku => withdraw.Items.First(pc => pc.Sku == flavorSku))
                    .Select(si => new Flavor(si.Sku, si.BasePrice));
            resultCollection.Add(Pizza.Create(flavors));
        }
        return resultCollection.Flatten();
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

    private async Task<ICommandResult<IDeliverable>> GetCustomer(CustomerId customerId)
    {
        var customer = await _customerRepository.GetById(customerId);
        if (customer == null) return CommandResult.CreateFailed<IDeliverable>("Customer not found");
        return CommandResult.CreateSuccess((IDeliverable)customer);
    }
}