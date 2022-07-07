namespace GreenFieldPizza.Application.Stock.Services.Interfaces;

public interface IStockManager
{
    Task<Withdraw> WithdrawFromStock(WithdrawItems request);
}