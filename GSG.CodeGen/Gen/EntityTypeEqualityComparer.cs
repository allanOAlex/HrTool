using Microsoft.EntityFrameworkCore.Metadata;

namespace GSG.CodeGen.Gen;

public class EntityTypeEqualityComparer : IEqualityComparer<IEntityType>
{
    public bool Equals(IEntityType? x, IEntityType? y)
    {
        return string.Equals(x?.Name, y?.Name);
    }

    public int GetHashCode(IEntityType obj)
    {
        return obj.GetHashCode();
    }
}