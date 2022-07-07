namespace GreenFieldPizza.Application.Handlers;

public abstract class BaseCommandHandler<C, R> : ICommandHandler<C, R>
                        where C : ICommand
                        where R : class
{
    public async Task<ICommandResult<R>> TryHandle(C command)
    {
        var validationResult = command.IsValid();
        return await validationResult.Match<Task<ICommandResult<R>>>(
            onError: errors => Task.FromResult(CommandResult.CreateFailed<R>(errors)),
            onSuccess: () => Handle(command)
        );
    }

    protected abstract Task<ICommandResult<R>> Handle(C command);
}