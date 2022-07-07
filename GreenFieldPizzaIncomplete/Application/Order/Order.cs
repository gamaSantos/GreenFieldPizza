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

    public static CommandResult<Order> Create(IEnumerable<Pizza> pizzas, IDeliverable deliverable)
    {
        var validationResult = ValidateCreationParamenters(pizzas, deliverable);
        if (validationResult.HasErrors) return validationResult.ToCommandResultWithErrors<Order>();
        return CommandResult.CreateSuccess(new Order(Guid.NewGuid(), pizzas, customerId: null, deliverable: deliverable, createdAt: DateTime.Now));
    }

    public static CommandResult<Order> CreateWithCustomer(IEnumerable<Pizza> pizzas, Customer customer)
    {
        var deliverable = (IDeliverable)customer;
        var validationResult = ValidateCreationParamenters(pizzas, deliverable);
        if (validationResult.HasErrors) return validationResult.ToCommandResultWithErrors<Order>();
        return CommandResult.CreateSuccess(new Order(Guid.NewGuid(), pizzas, customerId: customer.Id, deliverable: deliverable, createdAt: DateTime.Now));
    }

    public static CommandResult<Order> Load(OrderId id, IEnumerable<Pizza> pizzas, CustomerId? customerId, IDeliverable deliverable, DateTime createdAt)
    {
        var validationResult = ValidateCreationParamenters(pizzas, deliverable);
        if (!id.IsValid()) validationResult.Add("id should not be empty");
        if (validationResult.HasErrors) return validationResult.ToCommandResultWithErrors<Order>();
        return CommandResult.CreateSuccess(new Order(id, pizzas, customerId, deliverable, createdAt));
    }


    private static ValidationResult ValidateCreationParamenters(IEnumerable<Pizza> pizzas, IDeliverable deliverable)
    {
        pizzas = pizzas ?? new List<Pizza>();
        var errors = new List<string>();
        if (!pizzas.Any()) errors.Add("An order should have at least one pizza");
        if (pizzas.Count() > 10) errors.Add("An order cannot have more than ten pizzas");
        if (deliverable == null) errors.Add("An order must have a deliverable adress");
        return new ValidationResult(errors);
    }

    public OrderId Id { get; }
    public IEnumerable<Pizza> Pizzas { get; }
    public CustomerId? CustomerId { get; }
    public IDeliverable Deliverable { get; }

    public DateTime CreatedAt { get; }
}
