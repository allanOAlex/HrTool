using Autofac;

namespace GSG.Repository.Validation;
public partial class ModelValidationModule
{
    partial void AddToBuilder(ContainerBuilder builder)
    {
        builder.RegisterType<ModelValidator>().SingleInstance();
    }
}