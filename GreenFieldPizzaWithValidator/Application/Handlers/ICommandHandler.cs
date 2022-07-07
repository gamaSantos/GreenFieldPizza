namespace GreenFieldPizza.Application.Handlers;

public interface ICommandHandler<C, R>
                    where C : ICommand
                    where R : class
{
    Task<ICommandResult<R>> TryHandle(C command);
}