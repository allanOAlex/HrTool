using GSG.CodeGen.Commands;
using GSG.CodeGen.Notifications;
using GSG.CodeGen.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;

namespace GSG.CodeGen.Gen;

public class CSharpModelGenerator : ModelCodeGenerator
{
    private readonly ISender _mediatorSender;
    private readonly IPublisher _mediatorPublisher;
    public virtual ICSharpDbContextGenerator CSharpDbContextGenerator { get; }

    public virtual ICSharpEntityTypeGenerator CSharpEntityTypeGenerator { get; }

    /// <summary>
    ///     This is an internal API that supports the Entity Framework Core infrastructure and not subject to
    ///     the same compatibility standards as public APIs. It may be changed or removed without notice in
    ///     any release. You should only use it directly in your code with extreme caution and knowing that
    ///     doing so can result in application failures when updating to a new Entity Framework Core release.
    /// </summary>
    public CSharpModelGenerator(
        ModelCodeGeneratorDependencies dependencies,
        ICSharpDbContextGenerator cSharpDbContextGenerator,
        ICSharpEntityTypeGenerator cSharpEntityTypeGenerator,
        ISender mediatorSender, IPublisher mediatorPublisher)
        : base(dependencies)
    {
        Check.NotNull(cSharpDbContextGenerator, nameof(cSharpDbContextGenerator));
        Check.NotNull(cSharpEntityTypeGenerator, nameof(cSharpEntityTypeGenerator));
        Check.NotNull(mediatorSender, nameof(mediatorSender));
        Check.NotNull(mediatorPublisher, nameof(mediatorPublisher));

        CSharpDbContextGenerator = cSharpDbContextGenerator;
        CSharpEntityTypeGenerator = cSharpEntityTypeGenerator;
        _mediatorSender = mediatorSender;
        _mediatorPublisher = mediatorPublisher;
    }

    private const string FileExtension = ".cs";

    public override string Language => "C#";

    public override ScaffoldedModel GenerateModel(
        IModel model,
        ModelCodeGenerationOptions options)
    {
        Check.NotNull(model, nameof(model));
        Check.NotNull(options, nameof(options));

        if (options.ContextName == null)
        {
            throw new ArgumentException(
                CoreStrings.ArgumentPropertyNull(nameof(options.ContextName), nameof(options)), nameof(options));
        }

        if (options.ConnectionString == null)
        {
            throw new ArgumentException(
                CoreStrings.ArgumentPropertyNull(nameof(options.ConnectionString), nameof(options)), nameof(options));
        }

        var generatedCode = CSharpDbContextGenerator.WriteCode(
            model,
            options.ContextName,
            options.ConnectionString,
            options.ContextNamespace,
            options.ModelNamespace,
            options.UseDataAnnotations,
            options.UseNullableReferenceTypes,
            options.SuppressConnectionStringWarning,
            options.SuppressOnConfiguring);

        // output DbContext .cs file
        var dbContextFileName = options.ContextName +".Generated"+ FileExtension;
        var resultingFiles = new ScaffoldedModel
        {
            ContextFile = new ScaffoldedFile
            {
                Path = options.ContextDir != null
                    ? Path.Combine(options.ContextDir, dbContextFileName)
                    : dbContextFileName,
                Code = generatedCode
            }
        };

        foreach (var entityType in model.GetEntityTypes())
        {
            string entityName = entityType.Name;

            if (SharedTypeExtensions.IsManyToManyJoinEntityType(entityType))
            {
                continue;
            }

            Task.Run(() => _mediatorSender.Send(new AddEntityTypeCommand(entityType))).Wait();
           
            
            generatedCode = CSharpEntityTypeGenerator.WriteCode(
                entityType,
                options.ModelNamespace,
                options.UseDataAnnotations,
                options.UseNullableReferenceTypes);

            // output EntityType poco .cs file
            var entityTypeFileName = entityName +".Generated"+ FileExtension;
            resultingFiles.AdditionalFiles.Add(
                new ScaffoldedFile { Path = entityTypeFileName, Code = generatedCode });
        }
 Task.Run(() => _mediatorPublisher.Publish(new EntityTypeAddedNotification())).Wait();
        return resultingFiles;
    }
}