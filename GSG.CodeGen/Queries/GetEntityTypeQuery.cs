using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GSG.CodeGen.Queries;

public class GetEntityTypeQuery : IRequest<IEnumerable<IEntityType>>
{
    
}