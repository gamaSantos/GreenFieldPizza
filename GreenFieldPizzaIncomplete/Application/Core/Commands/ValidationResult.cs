namespace GreenFieldPizza.Application.Core;

public class ValidationResult
{
    private readonly List<string> _errors;
    public ValidationResult()
    {
        _errors = new List<string>();
    }
    public ValidationResult(IEnumerable<string> errors)
    {
        _errors = new List<string>(errors);
    }

    public ValidationResult(string error)
    {
        _errors = new List<string>
        {
            error
        };
    }

    public IEnumerable<string> Errors => _errors;

    public void Add(string error) => _errors.Add(error);
    public void Add(ValidationResult validationResult) => _errors.AddRange(validationResult.Errors);
    public bool Any() => _errors.Any();
    public bool Success => _errors.Any() == false;
    public bool HasErrors => _errors.Any();
    public CommandResult<T> ToCommandResultWithErrors<T>() where T : class => CommandResult<T>.CreateWithErrors(_errors);
}