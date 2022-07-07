namespace GreenFieldPizza.Application.Orders;

public class Flavor
{
    public Flavor(Sku code, Price basePrice)
    {
        Code = code;
        BasePrice = basePrice;
    }

    public ICommandResult<Flavor> Load(string code, Price price)
    {
        var errors = new List<string>();
        var validtor = Validator.Create();
        validtor.AddRule(string.IsNullOrWhiteSpace(code) == false, "Flavor must have a code");
        validtor.AddRule(price > 0, "Price must be bigger than 0");
        return validtor.CreateCommand(new Flavor(code, price));
    }
    public Sku Code { get; }
    public Price BasePrice { get; }
}
