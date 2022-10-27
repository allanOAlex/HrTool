using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GSG.CodeGen.Commands;

public record  AddEntityTypeCommand(IEntityType EntityType) : IRequest<IEntityType>
{
    
}