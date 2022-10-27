using GSG.CodeGen.Gen;
using GSG.CodeGen.Notifications;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Scaffolding;

namespace GSG.CodeGen.Handlers;

public class EntityTypeValidationHandler : INotificationHandler<EntityTypeAddedNotification>
{
    private readonly IComplexReverseEngineerScaffolder _complexReverseEngineerScaffolder;
    private readonly EntityTypeDataStore _entityTypeDataStore;
    private readonly ApplicationSettings _applicationSettings;
    private readonly IValidationEnityTypeCodeGenerator _entityTypeCodeGenerator;
    private readonly IValidationEnityTypeCollectionCodeGenerator _collectionCodeGenerator;

    public EntityTypeValidationHandler(EntityTypeDataStore entityTypeDataStore,
        ApplicationSettings applicationSettings,
        IValidationEnityTypeCodeGenerator entityTypeCodeGenerator,
        IValidationEnityTypeCollectionCodeGenerator collectionCodeGenerator,
        IComplexReverseEngineerScaffolder complexReverseEngineerScaffolder)
    {
        _entityTypeDataStore = entityTypeDataStore;
        _applicationSettings = applicationSettings;
        _entityTypeCodeGenerator = entityTypeCodeGenerator;
        _collectionCodeGenerator = collectionCodeGenerator;
        _complexReverseEngineerScaffolder = complexReverseEngineerScaffolder;
    }

    public async Task Handle(EntityTypeAddedNotification notification, CancellationToken cancellationToken)
    {
        if (Directory.Exists( _applicationSettings.ValidationDir))
        {
            Directory.Delete( _applicationSettings.ValidationDir,true); 
        }

        IEnumerable<IEntityType> entityTypes = await _entityTypeDataStore.GetAllEntities();
       
        foreach (var entity in entityTypes)
        {
            ScaffoldedFile file = await _entityTypeDataStore.EventOccured(
                entityType: entity,
                action: _entityTypeCodeGenerator,
                @namespace: _applicationSettings.ValidationNamespace,
                file: entity.Name + "Validation.Generated.cs",
                usings: _applicationSettings.ValidationUsings);

            _complexReverseEngineerScaffolder.Save(file, _applicationSettings.ValidationDir, true);
        }
        
        ScaffoldedFile builderFile = await _entityTypeDataStore.EventOccured(
            entityTypes: entityTypes,
            action: _collectionCodeGenerator,
            @namespace: _applicationSettings.ValidationNamespace,
            file:  "Validation.Generated.cs",
            usings: _applicationSettings.ValidationUsings);

        _complexReverseEngineerScaffolder.Save(builderFile, _applicationSettings.ValidationDir, true);
    }
}