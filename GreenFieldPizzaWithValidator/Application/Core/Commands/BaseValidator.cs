namespace GreenFieldPizza.Application.Core;

public abstract class BaseValidator
{
    public abstract ValidationResult Validate();

    public ICommandResult<T> CreateCommand<T>(T content) where T : class
    {
        var validationResult = Validate();
        return validationResult.Match<ICommandResult<T>>(
            onError: errors => validationResult.ToCommandResultWithErrors<T>(),
            onSuccess: () => CommandResult.CreateSuccess(content));
    }
}
