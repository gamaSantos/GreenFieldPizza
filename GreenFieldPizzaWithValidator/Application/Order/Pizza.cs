namespace GreenFieldPizza.Application.Orders;

public class Pizza
{
    private Pizza(IEnumerable<Flavor> flavors)
    {
        Flavors = flavors;
    }

    public static ICommandResult<Pizza> Create(IEnumerable<Flavor> flavors)
    {
        var validator = Validator.Create();
        validator.AddRule(flavors.Any(), "A pizza should have at least a flavor");
        validator.AddRule(flavors.Count() <= 2, "A pizza cannot have more than 2 flavors");
        return validator.CreateCommand(new Pizza(flavors));
    }
    public IEnumerable<Flavor> Flavors { get; }

    public Price Price => Flavors.Average(p => p.BasePrice.Amount);
}
