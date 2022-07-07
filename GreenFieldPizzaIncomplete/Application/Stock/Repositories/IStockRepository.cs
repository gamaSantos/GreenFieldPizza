namespace GreenFieldPizza.Application.Stock.Repositories
{
    public interface IStockRepository
    {
        Task<IEnumerable<StockItem>> List();
        Task<bool> Update(IEnumerable<StockItem> stockItem);
    }
}