using Microsoft.EntityFrameworkCore.Metadata;

namespace GSG.CodeGen.Gen;

public interface IEnityTypeCollectionCodeGenerator
{
    string WriteCode(IEnumerable<IEntityType> entityType, string @namespace, string[] usings);
}