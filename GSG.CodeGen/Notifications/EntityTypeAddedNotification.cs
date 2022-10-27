using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GSG.CodeGen.Notifications;

public record EntityTypeAddedNotification() : INotification;
