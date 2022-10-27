using Autofac;
using FluentValidation.Results;

namespace GSG.Repository;

public class ModelValidator
{
    private readonly ILifetimeScope _container;


    public ModelValidator(ILifetimeScope container)
    {
        _container = container;
    }
    
    public ValidationResult Validate<T>(T model)
    {
        var validation = _container.Resolve<AbstractModelValidator<T>>();
        return validation.Validate(model);
    }
}