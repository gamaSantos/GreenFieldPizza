namespace GreenFieldPizza.Application.Core;

public record Sku
{
    public Sku(string value)
    {
        this.Code = value;
    }

    public string Code { get; }

    public static implicit operator string(Sku code) => code.Code;
    public static implicit operator Sku(string value) => new(value);

    public override string ToString()
    {
        return Code;
    }
}