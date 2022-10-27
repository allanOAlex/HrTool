using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentMigrator;
using FluentMigrator.Expressions;
using FluentMigrator.Infrastructure;

public sealed class ExtExecuteEmbeddedSqlScriptExpression : ExecuteEmbeddedSqlScriptExpressionBase
{
    private readonly IReadOnlyCollection<IEmbeddedResourceProvider> _embeddedResourceProviders;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecuteEmbeddedSqlScriptExpression"/> class.
    /// </summary>
    [Obsolete]
    public ExtExecuteEmbeddedSqlScriptExpression()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecuteEmbeddedSqlScriptExpression"/> class.
    /// </summary>
    /// <param name="embeddedResourceProviders">The embedded resource providers</param>
    public ExtExecuteEmbeddedSqlScriptExpression(
        [NotNull] IEnumerable<IEmbeddedResourceProvider> embeddedResourceProviders)
    {
        _embeddedResourceProviders = embeddedResourceProviders.ToList();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExecuteEmbeddedSqlScriptExpression"/> class.
    /// </summary>
    /// <param name="assemblyCollection">The collection of assemblies to be searched for the resources</param>
    [Obsolete]
    public ExtExecuteEmbeddedSqlScriptExpression([NotNull] IAssemblyCollection assemblyCollection)
    {
        MigrationAssemblies = assemblyCollection;
        _embeddedResourceProviders = new IEmbeddedResourceProvider[]
        {
            new DefaultEmbeddedResourceProvider(assemblyCollection),
        };
    }

    /// <summary>
    /// Gets or sets the SQL script name
    /// </summary>
    [Required(
        ErrorMessageResourceType = typeof(ErrorMessages),
        ErrorMessageResourceName = nameof(ErrorMessages.SqlScriptCannotBeNullOrEmpty))]
    public string SqlScript { get; set; }

    /// <summary>
    /// Gets or sets the migration assemblies
    /// </summary>
    [Obsolete()]
    public IAssemblyCollection MigrationAssemblies { get; set; }

    /// <inheritdoc />
    public override void ExecuteWith(IMigrationProcessor processor)
    {
        List<(string name, Assembly assembly)> resourceNames;
#pragma warning disable 612
        if (MigrationAssemblies != null)
        {
            resourceNames = MigrationAssemblies.GetManifestResourceNames()
                .Select(item => (name: item.Name, assembly: item.Assembly))
                .ToList();
#pragma warning restore 612
        }
        else if (_embeddedResourceProviders != null)
        {
            resourceNames = _embeddedResourceProviders
                .SelectMany(x => x.GetEmbeddedResources())
                .Distinct()
                .ToList();
        }
        else
        {
#pragma warning disable 612
            throw new InvalidOperationException(
                $"The caller forgot to set the {nameof(MigrationAssemblies)} property.");
#pragma warning restore 612
        }

        var embeddedResourceNameWithAssembly = GetQualifiedResourcePath(resourceNames, SqlScript);
        string sqlText;

        string sourcePath = @"/Users/joelobando/GSGUS/internalhrtool/DBMigration";


        //foreach (var resourceName in assembly.GetManifestResourceNames())
        //{
        //    yield return (resourceName, assembly);
        //}
        if (System.IO.Directory.Exists(sourcePath))
        {
            string[] files = System.IO.Directory.GetFiles(sourcePath);

            // Copy the files and overwrite destination files if they already exist.
            foreach (string s in files)
            {
                // Use static Path methods to extract only the file name from the path.
                var fileName = System.IO.Path.GetFileName(s);
                var destFile = System.IO.Path.Combine(sourcePath, fileName);
                var stream = File.Open(destFile, FileMode.Open);
                if (stream == null)
                {
                    throw new InvalidOperationException(
                        $"The resource {embeddedResourceNameWithAssembly.name} couldn't be found in {embeddedResourceNameWithAssembly.assembly.FullName}");
                }

                using (stream)
                using (var reader = new StreamReader(stream))
                {
                    sqlText = reader.ReadToEnd();
                }

                Execute(processor, sqlText);
            }
        }
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return base.ToString() + SqlScript;
    }
}