namespace GreenFieldPizza.Application.Stock;

public class StockItem
{
    public StockItem(Sku sku, Price basePrice, uint availableQuantity)
    {
        Sku = sku;
        BasePrice = basePrice;
        AvailableQuantity = availableQuantity;
    }

    public Sku Sku { get; }
    public Price BasePrice { get; }
    public uint AvailableQuantity { get; private set; }

    public void SubstractStock(uint quantity)
    {
        AvailableQuantity = Math.Clamp(AvailableQuantity - quantity, 0, AvailableQuantity);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Sku.Code, BasePrice.Amount, AvailableQuantity);
    }

    public override string ToString()
    {
        return $"Flavor-{Sku}-{BasePrice}-{AvailableQuantity}";
    }
}