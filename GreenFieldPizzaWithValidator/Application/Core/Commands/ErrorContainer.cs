namespace GreenFieldPizza.Application.Core;

public abstract class ErrorContainer
{
    protected readonly List<string> _errors;

    public ErrorContainer()
    {
        _errors = new List<string>();
    }
    public ErrorContainer(IEnumerable<string> errors)
    {
        _errors = new List<string>(errors);
    }
    public ErrorContainer(string error)
    {
        _errors = new List<string>
        {
            error
        };
    }

    public IEnumerable<string> Errors => _errors;

    public void Add(string error) => _errors.Add(error);
}
