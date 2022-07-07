namespace GreenFieldPizza.Application.Core;

public class SuccessCommandResult<T> : ICommandResult<T>
{
    private readonly T _content;

    internal SuccessCommandResult(T content)
    {
        _content = content;
    }

    public ICommandResult<R> Do<R>(Func<T, R> func)
    {
        var result = func(_content);
        return new SuccessCommandResult<R>(result);
    }

    public void Do(Action<T> act)
    {
        act(_content);
    }

    public R Match<R>(Func<IEnumerable<string>, R> matchErrors, Func<T, R> matchContent)
    {
        return matchContent(_content);
    }

    public void Match(Action<IEnumerable<string>> matchErrors, Action<T> matchContent)
    {
        matchContent(_content);
    }
}
