using GSG.CodeGen.Gen;
using GSG.CodeGen.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GSG.CodeGen.Handlers;
 
public class GetEntityTypeByNameHandler : IRequestHandler<GetEntityByNameQuery,IEntityType>
{
    private readonly EntityTypeDataStore _entityTypeDataStore;

    public GetEntityTypeByNameHandler(EntityTypeDataStore entityTypeDataStore)
    {
        _entityTypeDataStore = entityTypeDataStore;
    }

    public async Task<IEntityType> Handle(GetEntityByNameQuery request, CancellationToken cancellationToken)
        => await _entityTypeDataStore.GetEntityByName(request.name);
}