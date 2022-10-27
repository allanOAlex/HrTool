using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GSG.CodeGen.Queries;

public record GetEntityByNameQuery(string name) : IRequest<IEntityType>;
