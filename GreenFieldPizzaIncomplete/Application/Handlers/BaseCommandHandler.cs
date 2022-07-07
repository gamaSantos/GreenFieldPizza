namespace GreenFieldPizza.Application.Handlers;

public abstract class BaseCommandHandler<C, R> : ICommandHandler<C, R>
                        where C : ICommand
                        where R : class
{
    public async Task<CommandResult<R>> TryHandle(C command)
    {
        var validationResult = command.IsValid();
        if (validationResult.HasErrors) return validationResult.ToCommandResultWithErrors<R>();

        return await Handle(command);
    }

    protected abstract Task<CommandResult<R>> Handle(C command);
}