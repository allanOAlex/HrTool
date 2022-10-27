using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GSG.CodeGen.Gen;

public class EnityTypeValidationRegistrationCodeGenerator : IValidationEnityTypeCollectionCodeGenerator
{
    public string WriteCode(IEnumerable<IEntityType> entityTypes, string @namespace, string[] usings)
    {
        IndentedStringBuilder sb = new IndentedStringBuilder();
        sb.AppendWarning();
        sb.AppendLine("using Autofac;");
        sb.AppendLine("using FluentValidation;");
        foreach (var @using in usings)
        {
            sb.AppendLine($"using {@using};");
        }

        sb.AppendLine();
        sb.AppendLine("namespace " + @namespace + ";");

        sb.AppendLine(
            $"public partial class ModelValidationModule : Module");
        sb.AppendLine("{");
        using (sb.Indent())
        {
            sb.AppendLine("protected override void Load(ContainerBuilder builder)");
            sb.AppendLine("{");
            using (sb.Indent())
            {
                foreach (IEntityType entityType in entityTypes)
                {
                    sb.AppendLine($"builder.RegisterType<{entityType.Name}Validation>().As(typeof(AbstractModelValidator<{entityType.Name}>));");
                  
                }
                sb.AppendLine("AddToBuilder(builder);");
            }
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine("partial void AddToBuilder(ContainerBuilder builder);");
        }

        sb.AppendLine("}");


        //public class RepositoryModule : Module
        //{
        //protected override void Load(ContainerBuilder builder)
        // {
        // builder.RegisterGeneric(typeof(AbstractValidator<>)).As(typeof(AbstractBaseValidator<>));
        //   }
        //}

        return sb.ToString();
    }
}