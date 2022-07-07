using GreenFieldPizza.Application.Customers;
using GreenFieldPizza.Application.Customers.Repositories;
using LiteDB;

namespace GreenFieldPizza.Data.Repositories.Customers;

public class CustomerRepository : ICustomerRepository
{
    private const string DbLocation = @"test.db";
    private const string CollectionName = "customers";

    public Task<Customer?> GetById(CustomerId id)
    {
        using var db = new LiteDatabase(DbLocation);
        var colection = db.GetCollection<CustomerEntity>(CollectionName);
        var entity = colection.FindOne(x => x.Id == id.Value);
        return Task.FromResult(entity?.ToDomain());
    }
}