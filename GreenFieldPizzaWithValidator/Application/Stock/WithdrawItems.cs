namespace GreenFieldPizza.Application.Stock;

public class WithdrawItems
{
    private readonly Dictionary<Sku, uint> _items;

    private WithdrawItems(Dictionary<Sku, uint> items)
    {
        _items = items ?? new Dictionary<Sku, uint>();
    }

    public static WithdrawItems Create(Dictionary<Sku, uint> items) => new(items);
    public static WithdrawItems CreateEmpty() => new(new Dictionary<Sku, uint>());

    public bool TryAddItem(Sku sku, uint quantity)
    {
        return _items.TryAdd(sku, quantity);
    }

    public IEnumerable<(Sku Sku, uint Quantity)> GetItems() => _items.Select(kv => (kv.Key, kv.Value));
}