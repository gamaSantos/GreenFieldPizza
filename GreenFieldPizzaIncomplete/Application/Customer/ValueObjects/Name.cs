namespace GreenFieldPizza.Application.Customers;

public class Name
{
    public Name(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        FullName = $"{firstName} {lastName}";
    }

    public string FirstName { get; }
    public string LastName { get; }
    public string FullName { get; }

    public bool IsValid() => string.IsNullOrWhiteSpace(FirstName) == false && string.IsNullOrWhiteSpace(LastName) == false;
}