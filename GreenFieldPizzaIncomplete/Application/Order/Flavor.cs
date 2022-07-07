namespace GreenFieldPizza.Application.Orders;

public class Flavor
{
    public Flavor(Sku code, Price basePrice)
    {
        Code = code;
        BasePrice = basePrice;
    }

    public CommandResult<Flavor> Load(string code, Price price)
    {
        var errors = new List<string>();
        if (string.IsNullOrWhiteSpace(code)) errors.Add("Flavor must have a code");
        if (price <= 0) errors.Add("Price must be bigger than 0");

        if (errors.Any()) return CommandResult<Flavor>.CreateWithErrors(errors);

        return CommandResult.CreateSuccess(new Flavor(code, price));
    }
    public Sku Code { get; }
    public Price BasePrice { get; }
}
