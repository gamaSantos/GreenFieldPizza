namespace GreenFieldPizza.Application.Core;

public record Telephone
{
    public Telephone(string phoneNumber, string regionCode)
    {
        PhoneNumber = phoneNumber;
        RegionCode = regionCode.PadLeft(3, '0');
    }
    public string RegionCode { get; }
    public string PhoneNumber { get; }

    public override string ToString()
    {
        return $"{RegionCode}{PhoneNumber}";
    }

    internal bool IsValid()
    {
        return
            RegionCode.Length == 3
            && RegionCode != "000"
            && RegionCode.All(c => Char.IsDigit(c))
            && (PhoneNumber.Length == 8 || PhoneNumber.Length == 9)
            && PhoneNumber.All(c => Char.IsDigit(c));
    }
}