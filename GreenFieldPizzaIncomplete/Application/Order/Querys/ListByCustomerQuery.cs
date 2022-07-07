using GreenFieldPizza.Application.Customers;

namespace GreenFieldPizza.Application.Orders.Querys;

public record ListByCustomerQuery(CustomerId customerId, int page);