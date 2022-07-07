namespace GreenFieldPizza.Application.Core;

public class CommandResultCollection<T>
{
    private readonly List<ICommandResult<T>> items;
    public CommandResultCollection()
    {
        items = new List<ICommandResult<T>>();
    }

    public void Add(ICommandResult<T> item) => items.Add(item);

    public ICommandResult<IEnumerable<T>> Flatten()
    {
        var result = new List<T>();
        var errors = new List<string>();
        foreach (var item in items)
        {
            item.Match(e => errors.AddRange(e), p => result.Add(p));
        }
        if (errors.Any()) return CommandResult.CreateFailed<IEnumerable<T>>(errors);
        return CommandResult.CreateSuccess<IEnumerable<T>>(result);
    }
}
