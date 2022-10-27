using Microsoft.EntityFrameworkCore.Metadata;

namespace GSG.CodeGen.Gen;

public interface IEnityTypeCodeGenerator
{
    string WriteCode(IEntityType entityType, string @namespace, string[] usings);
}