namespace GreenFieldPizza.Application.Core;
public class Validator : BaseValidator
{
    public static Validator<CT> Create<CT>(CT sut) => new(sut);
    public static Validator Create() => new();

    private readonly List<ValidationResult> _appendedValidations;
    private readonly List<Rule> _rules;

    internal Validator()
    {
        _appendedValidations = new();
        _rules = new();
    }

    public void AddRule(bool condition, string errorMessage)
    {
        _rules.Add(new Rule(condition, errorMessage));
    }

    public void Append(ValidationResult validationResult) => _appendedValidations.Add(validationResult);

    public override ValidationResult Validate()
    {
        var validationResult = new ValidationResult();
        foreach (var validation in _appendedValidations) validationResult.Add(validation);
        foreach (var rule in _rules) rule.Test(message => validationResult.Add(message));

        return validationResult;
    }

    private class Rule
    {
        public Rule(bool condition, string errorMessage)
        {
            this.condition = condition;
            ErrorMessage = errorMessage;
        }

        public bool condition { get; }
        public string ErrorMessage { get; }

        public void Test(Action<string> onFail)
        {
            if (condition == false) onFail(ErrorMessage);
        }
    }
}

public class Validator<T> : BaseValidator
{
    private readonly T _sut;
    private readonly List<ValidationResult> _appendedValidations;
    private readonly List<Rule<T>> _rules;

    internal Validator(T sut)
    {
        _sut = sut;
        _rules = new List<Rule<T>>();
        _appendedValidations = new List<ValidationResult>();
    }


    public void AddRule(Func<T, bool> condition, string errorMessage)
    {
        _rules.Add(new Rule<T>(condition, errorMessage));
    }

    public void Append(ValidationResult validationResult) => _appendedValidations.Add(validationResult);


    public override ValidationResult Validate()
    {
        var validationResult = new ValidationResult();
        foreach (var validation in _appendedValidations) validationResult.Add(validation);
        foreach (var rule in _rules) rule.Test(_sut, message => validationResult.Add(message));

        return validationResult;
    }

    private class Rule<RT>
    {
        public Rule(Func<RT, bool> condition, string errorMessage)
        {
            this.condition = condition;
            ErrorMessage = errorMessage;
        }

        public Func<RT, bool> condition { get; }
        public string ErrorMessage { get; }

        public void Test(RT sut, Action<string> onFail)
        {
            if (condition(sut) == false) onFail(ErrorMessage);
        }
    }
}
