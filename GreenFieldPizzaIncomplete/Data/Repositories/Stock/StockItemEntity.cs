using GreenFieldPizza.Application.Stock;

namespace GreenFieldPizza.Data.Repositories.Stock;

class StockItemEntity
{
    public string Sku { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public uint AvailableQuantity { get; set; }

    public StockItem ToDomain() => new(Sku, BasePrice, AvailableQuantity);
}