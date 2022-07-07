namespace GreenFieldPizza.Application.Orders;

public class Pizza
{
    private Pizza(IEnumerable<Flavor> flavors)
    {
        Flavors = flavors;
    }

    public static CommandResult<Pizza> Create(IEnumerable<Flavor> flavors)
    {
        if (flavors == null || flavors.Any() == false) return CommandResult<Pizza>.CreateWithErrors("A pizza should have at least a flavor");
        if (flavors.Count() > 2) return CommandResult<Pizza>.CreateWithErrors("A pizza cannot have more than 2 flavors");
        return CommandResult.CreateSuccess(new Pizza(flavors));
    }
    public IEnumerable<Flavor> Flavors { get; }

    public Price Price => Flavors.Average(p => p.BasePrice.Amount);
}
