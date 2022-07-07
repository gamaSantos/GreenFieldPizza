using GreenFieldPizza.Application.Customers;

namespace GreenFieldPizza.Application.Orders;

public class Order
{
    private Order(OrderId id, IEnumerable<Pizza> pizzas, CustomerId? customerId, IDeliverable deliverable, DateTime createdAt)
    {
        Id = id;
        Pizzas = pizzas;
        CustomerId = customerId;
        Deliverable = deliverable;
        CreatedAt = createdAt;
    }

    public static ICommandResult<Order> Create(IEnumerable<Pizza> pizzas, IDeliverable deliverable)
    {
        var validator = ValidateCreationParamenters(pizzas, deliverable);
        return validator.CreateCommand(new Order(Guid.NewGuid(), pizzas, customerId: null, deliverable: deliverable, createdAt: DateTime.Now));
    }

    public static ICommandResult<Order> CreateWithCustomer(IEnumerable<Pizza> pizzas, Customer customer)
    {
        var deliverable = (IDeliverable)customer;
        var validator = ValidateCreationParamenters(pizzas, deliverable);
        return validator.CreateCommand(new Order(Guid.NewGuid(), pizzas, customerId: customer.Id, deliverable: deliverable, createdAt: DateTime.Now));
    }

    public static ICommandResult<Order> Load(OrderId id, IEnumerable<Pizza> pizzas, CustomerId? customerId, IDeliverable deliverable, DateTime createdAt)
    {
        var validator = ValidateCreationParamenters(pizzas, deliverable);
        validator.AddRule(id.IsValid(), "id should not be empty");
        return validator.CreateCommand(new Order(id, pizzas, customerId, deliverable, createdAt));
    }


    private static Validator ValidateCreationParamenters(IEnumerable<Pizza> pizzas, IDeliverable deliverable)
    {
        var validator = Validator.Create();
        pizzas = pizzas ?? new List<Pizza>();
        validator.AddRule(pizzas.Any(), "An order should have at least one pizza");
        validator.AddRule(pizzas.Count() <= 10, "An order cannot have more than ten pizzas");
        validator.AddRule(deliverable != null, "An order must have a deliverable adress");
        return validator;
    }

    public OrderId Id { get; }
    public IEnumerable<Pizza> Pizzas { get; }
    public CustomerId? CustomerId { get; }
    public IDeliverable Deliverable { get; }

    public DateTime CreatedAt { get; }
}
