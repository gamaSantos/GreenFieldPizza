namespace GreenFieldPizza.Application.Core;

public record Price
{
    public Price(decimal value)
    {
        Amount = value;
    }
    public decimal Amount { get; }

    public static implicit operator decimal(Price d) => d.Amount;
    public static implicit operator Price(decimal d) => new Price(d);

    public override string ToString()
    {
        return Math.Round(Amount, 2).ToString("c");
    }
}
