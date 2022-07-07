namespace GreenFieldPizza.Application.Stock;

public record Withdraw
{
    private readonly WithdrawItems _requestedItems;
    private readonly List<StockItem> _items;

    public Withdraw(WithdrawItems requestedItems, IEnumerable<StockItem> items)
    {
        _requestedItems = requestedItems;
        _items = new List<StockItem>(items);
        IsCompleted = requestedItems.GetItems().All(i => _items.Any(si => i.Sku == si.Sku));
    }

    public IReadOnlyList<StockItem> Items => _items;
    public bool IsCompleted { get; }
}