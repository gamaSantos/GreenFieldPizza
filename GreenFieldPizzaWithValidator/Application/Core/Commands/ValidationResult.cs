namespace GreenFieldPizza.Application.Core;

public class ValidationResult : ErrorContainer
{
    public ValidationResult() : base()
    {
    }

    public ValidationResult(IEnumerable<string> errors) : base(errors)
    {
    }

    public ValidationResult(string error) : base(error)
    {
    }

    public void Add(ValidationResult validationResult) => _errors.AddRange(validationResult.Errors);

    public T Match<T>(Func<IEnumerable<string>, T> onError, Func<T> onSuccess) where T : class
    {
        return _errors.Any() ? onError(_errors) : onSuccess();
    }

    public ICommandResult<T> ToCommandResultWithErrors<T>() where T : class => CommandResult.CreateFailed<T>(_errors);
}


