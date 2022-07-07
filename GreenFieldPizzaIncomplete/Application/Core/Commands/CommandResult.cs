using System.Diagnostics.CodeAnalysis;

namespace GreenFieldPizza.Application.Core;

public static class CommandResult
{
    public static CommandResult<T> CreateSuccess<T>(T content) where T : class => CommandResult<T>.CreateSucess(content);
}

public class CommandResult<T> : ValidationResult where T : class
{
    private readonly T? _content;
    private CommandResult(T? content, IEnumerable<string> errors) : base(errors)
    {
        _content = content;
    }

    private CommandResult(T content) : this(content, new List<String>()) { }


    public static CommandResult<T> CreateSucess(T content) => new CommandResult<T>(content);
    public static CommandResult<T> CreateWithErrors(string error) => new CommandResult<T>(default, new string[] { error });
    public static CommandResult<T> CreateWithErrors(IEnumerable<string> errors) => new CommandResult<T>(default, errors);

    public bool TryGetContent([NotNullWhen(returnValue: true)] out T? content)
    {
        content = _content;
        return HasErrors == false;
    }


}