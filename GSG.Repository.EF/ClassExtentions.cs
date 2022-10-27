using Microsoft.EntityFrameworkCore;

namespace GSG.Repository.EF;

public static class ClassExtentions
{
    public static int GetKey<T>(this DbContext context, T entity)
    {
        var keyName = context.Model.FindEntityType(typeof(T)).FindPrimaryKey()
            .Properties.Select(x => x.Name).Single();

        return (int)entity.GetType().GetProperty(keyName).GetValue(entity, null);
    }
}