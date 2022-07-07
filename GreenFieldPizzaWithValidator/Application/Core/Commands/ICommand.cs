namespace GreenFieldPizza.Application.Core;

public interface ICommand
{
    ValidationResult IsValid();
}
