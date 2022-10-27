using GSG.CodeGen.Commands;
using GSG.CodeGen.Gen;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GSG.CodeGen.Handlers;

public class AddEntityTypeHandler : IRequestHandler<AddEntityTypeCommand, IEntityType>
{
    private readonly EntityTypeDataStore _entityTypeDataStore;

    public AddEntityTypeHandler(EntityTypeDataStore entityTypeDataStore)
    {
        _entityTypeDataStore = entityTypeDataStore;
    }

    public async Task<IEntityType> Handle(AddEntityTypeCommand request, CancellationToken cancellationToken)
    {
        await _entityTypeDataStore.AddEntity(request.EntityType);
        return request.EntityType;
    }
}