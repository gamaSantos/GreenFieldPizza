namespace GreenFieldPizza.Application.Customers.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetById(CustomerId id);
}