using GSG.CodeGen.Gen;
using GSG.CodeGen.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GSG.CodeGen.Handlers;

public class GetEntityTypeHandler : IRequestHandler<GetEntityTypeQuery, IEnumerable<IEntityType>>
{
    private readonly EntityTypeDataStore _entityTypeDataStore;

    public GetEntityTypeHandler(EntityTypeDataStore entityTypeDataStore)
    {
        _entityTypeDataStore = entityTypeDataStore;
    }
    
    public async Task<IEnumerable<IEntityType>> Handle(GetEntityTypeQuery request, CancellationToken cancellationToken)
    {
        return await _entityTypeDataStore.GetAllEntities();
    }
}