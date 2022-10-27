using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding;

namespace GSG.CodeGen.Gen;

public class EntityTypeDataStore
{
    private static HashSet<IEntityType> _entities;

    public EntityTypeDataStore()
    {
        IEqualityComparer<IEntityType> comparer = new EntityTypeEqualityComparer();
        _entities = new HashSet<IEntityType>(comparer: comparer);
    }

    public Task AddEntity(IEntityType model)
    {
        _entities.Add(model);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<IEntityType>> GetAllEntities() =>
        await Task.FromResult(_entities);


    public  async Task<IEntityType> GetEntityByName(string name) =>
        await Task.FromResult(_entities.FirstOrDefault(row => row.Name == name));

    public async Task<ScaffoldedFile> EventOccured(
            IEntityType entityType, 
            IEnityTypeCodeGenerator action, 
            string @namespace,
            string file,
            string[] usings) => 
        await Task.FromResult(new ScaffoldedFile { Path = file, Code = action.WriteCode(entityType, @namespace, usings) });
    
    public async Task<ScaffoldedFile> EventOccured(
        IEnumerable<IEntityType> entityTypes, 
        IEnityTypeCollectionCodeGenerator action, 
        string @namespace,
        string file,
        string[] usings) => 
        await Task.FromResult(new ScaffoldedFile { Path = file, Code = action.WriteCode(entityTypes, @namespace, usings) });
    
}