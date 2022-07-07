namespace GreenFieldPizza.Application.Core;

public static class CommandResult
{
    public static ICommandResult<T> CreateSuccess<T>(T content) where T : class => new SuccessCommandResult<T>(content);
    public static ICommandResult<T> CreateFailed<T>(string error) => new FailedCommandResult<T>(new string[] { error });
    public static ICommandResult<T> CreateFailed<T>(IEnumerable<string> errors) => new FailedCommandResult<T>(errors);
}

public interface ICommandResult<T>
{
    ICommandResult<R> Do<R>(Func<T, R> func);
    void Do(Action<T> act);
    R Match<R>(Func<IEnumerable<string>, R> matchErrors, Func<T, R> matchContent);
    void Match(Action<IEnumerable<string>> matchErrors, Action<T> matchContent);
}
