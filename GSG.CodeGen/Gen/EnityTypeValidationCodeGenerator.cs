using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GSG.CodeGen.Gen;

public class EnityTypeValidationCodeGenerator : IValidationEnityTypeCodeGenerator
{
    public string WriteCode(IEntityType entityType, string @namespace, string[] usings)
    {
        IndentedStringBuilder sb = new IndentedStringBuilder();

        sb.AppendWarning();
        sb.AppendLine("using FluentValidation;");
        foreach (var @using in usings)
        {
            sb.AppendLine($"using {@using};");
        }

        sb.AppendLine();
        sb.AppendLine("namespace " + @namespace + ";");

        using (sb.Indent())
        {
            sb.AppendLine(
                $"public partial class {entityType.Name}Validation : AbstractModelValidator<{entityType.Name}>");
            sb.AppendLine("{");
            using (sb.Indent())
            {
                sb.AppendLine($"public {entityType.Name}Validation()");
                sb.AppendLine("{");
                using (sb.Indent())
                {
                    foreach (var property in entityType.GetProperties())
                    {
                        List<string> lines = new List<string>();
                        sb.Append($"RuleFor(x => x.{property.Name})");

                        if (!property.IsNullable
                            && property.ClrType.IsNullableType()
                            && !property.IsPrimaryKey())
                        {
                            lines.Add("NotEmpty()");
                            lines.Add("NotNull()");
                            // lines.Add($".{nameof(PropertyBuilder.IsRequired)}()");
                        }

                        var maxLength = property.GetMaxLength();
                        if (maxLength.HasValue)
                        {
                            lines.Add($"MaximumLength({maxLength})");
                        }

                        if (lines.Any())
                        {
                            using (sb.Indent())
                            {
                                sb.Append($".{string.Join(".", lines)}");
                            }
                        }

                        sb.Append(";");
                        sb.AppendLine();
                    }
                }

                sb.AppendLine("}");
                ;
            }

            sb.AppendLine("}");
        }


        return sb.ToString();
    }
}