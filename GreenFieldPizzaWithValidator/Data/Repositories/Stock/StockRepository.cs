using GreenFieldPizza.Application.Stock;
using GreenFieldPizza.Application.Stock.Repositories;
using LiteDB;

namespace GreenFieldPizza.Data.Repositories.Stock;

public class StockRepository : IStockRepository
{
    private const string DbLocation = @"test.db";
    private const string CollectionName = "StockItems";

    public Task<IEnumerable<StockItem>> List()
    {
        using var db = new LiteDatabase(DbLocation);
        var collection = db.GetCollection<StockItemEntity>(CollectionName);
        return Task.FromResult(collection.FindAll().Select(entity => entity.ToDomain()));
    }

    public Task<bool> Update(IEnumerable<StockItem> stockItem)
    {
        using var db = new LiteDatabase(DbLocation);
        var collection = db.GetCollection<StockItemEntity>(CollectionName);
        var success = true;
        db.BeginTrans();
        foreach (var item in stockItem)
        {
            var entity = collection.FindOne(e => e.Sku == item.Sku.Code);

            entity.AvailableQuantity = item.AvailableQuantity;
            entity.BasePrice = item.BasePrice;

            success = collection.Update(entity);
            if (success == false)
            {
                db.Rollback();
                return Task.FromResult(false);
            }
        }
        success = db.Commit();
        return Task.FromResult(success);
    }
}