namespace GreenFieldPizza.Application.Handlers;

public interface ICommandHandler<C, R>
                    where C : ICommand
                    where R : class
{
    Task<CommandResult<R>> TryHandle(C command);
}