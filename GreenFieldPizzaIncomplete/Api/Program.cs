using GreenFieldPizza.Application.Customers.Repositories;
using GreenFieldPizza.Application.Handlers.Orders;
using GreenFieldPizza.Application.Orders.Repositories;
using GreenFieldPizza.Application.Stock.Repositories;
using GreenFieldPizza.Application.Stock.Services;
using GreenFieldPizza.Application.Stock.Services.Interfaces;
using GreenFieldPizza.Data.Repositories.Customers;
using GreenFieldPizza.Data.Repositories.Orders;
using GreenFieldPizza.Data.Repositories.Stock;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddTransient<ICreateOrderCommandHandler, CreateOrderCommandHandler>();
builder.Services.AddTransient<IOrderQueryHandler, OrderQueryHandler>();
builder.Services.AddTransient<IStockManager, StockManager>();

builder.Services.AddTransient<ICustomerRepository, CustomerRepository>();
builder.Services.AddTransient<IStockRepository, StockRepository>();
builder.Services.AddTransient<IOrderReadOnlyRepository, OrderRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();

var app = builder.Build();

app.MapControllers();

app.Run();
