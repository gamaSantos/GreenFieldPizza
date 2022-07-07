using GreenFieldPizza.Application.Stock.Repositories;
using GreenFieldPizza.Application.Stock.Services.Interfaces;

namespace GreenFieldPizza.Application.Stock.Services;

public class StockManager : IStockManager
{
    private readonly IStockRepository _stockRepository;

    public StockManager(IStockRepository flavorRepository)
    {
        _stockRepository = flavorRepository;
    }

    private bool IsAvailable(WithdrawItems request, IEnumerable<StockItem> stockItems)
    {
        return request.GetItems().All(requestItem =>
        {
            var stockItem = stockItems.FirstOrDefault(si => si.Sku == requestItem.Sku);
            if (stockItem == null) return false;
            return stockItem.AvailableQuantity >= requestItem.Quantity;
        });
    }

    public async Task<Withdraw> WithdrawFromStock(WithdrawItems request)
    {
        var stockItems = await _stockRepository.List();
        var requestItems = request.GetItems();
        if (IsAvailable(request, stockItems) == false) new Withdraw(request, new List<StockItem>());
        var withdrawded = new List<StockItem>();
        foreach (var requestItem in requestItems)
        {
            var stockItem = stockItems.First(f => f.Sku == requestItem.Sku);
            stockItem.SubstractStock(requestItem.Quantity);
            withdrawded.Add(stockItem);
        }
        await _stockRepository.Update(withdrawded);
        return new(request, withdrawded);
    }
}
