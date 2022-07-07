namespace GreenFieldPizza.Application.Core;

public class FailedCommandResult<T> : ErrorContainer, ICommandResult<T>
{
    internal FailedCommandResult(IEnumerable<string> errors) : base(errors)
    {
    }

    public ICommandResult<R> Do<R>(Func<T, R> func)
    {
        return new FailedCommandResult<R>(Errors);
    }

    public void Do(Action<T> act)
    {
        return;
    }

    public R Match<R>(Func<IEnumerable<string>, R> matchErrors, Func<T, R> matchContent)
    {
        return matchErrors(Errors);
    }

    public void Match(Action<IEnumerable<string>> matchErrors, Action<T> matchContent)
    {
        matchErrors(Errors);
    }
}