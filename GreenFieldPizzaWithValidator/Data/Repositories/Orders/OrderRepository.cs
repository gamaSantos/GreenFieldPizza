using GreenFieldPizza.Application.Customers;
using GreenFieldPizza.Application.Orders;
using GreenFieldPizza.Application.Orders.Querys;
using GreenFieldPizza.Application.Orders.Repositories;
using LiteDB;

namespace GreenFieldPizza.Data.Repositories.Orders;

public class OrderRepository : IOrderRepository, IOrderReadOnlyRepository
{
    private const string DbLocation = @"test.db";
    private const string CollectionName = "orders";
    private const int PageSize = 10;

    public Task<bool> Insert(Order order)
    {
        var entity = OrderEntity.FromDomain(order);
        using var db = new LiteDatabase(DbLocation);
        var collection = db.GetCollection<OrderEntity>(CollectionName);
        var result = collection.Insert(entity);
        return Task.FromResult(true);
    }

    public Task<IEnumerable<Order>> ListByCustomer(ListByCustomerQuery query)
    {
        using (var db = new LiteDatabase(DbLocation))
        {
            var ordersCollection = db.GetCollection<OrderEntity>(CollectionName);

            var entities = ordersCollection.Find(x => x.CustomerId == query.customerId);
            entities = GetPagedResult(query.page, entities);
            return Task.FromResult(entities.Select(e => e.ToDomain()).ToList().AsEnumerable());
        }
    }

    private IEnumerable<OrderEntity> GetPagedResult(int page, IEnumerable<OrderEntity>? items)
    {
        items = items ?? new List<OrderEntity>();
        var (pageCount, lastPageSize) = Math.DivRem(items.Count(), PageSize);
        page = Math.Clamp(page, 0, pageCount - 1);
        return items.Skip(page * PageSize).Take(PageSize);

    }
}
