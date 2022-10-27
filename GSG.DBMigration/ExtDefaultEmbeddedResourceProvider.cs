using System.Reflection;
using FluentMigrator.Infrastructure;

public class ExtDefaultEmbeddedResourceProvider : IEmbeddedResourceProvider
{
    private readonly IReadOnlyCollection<Assembly> _assemblies;

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultEmbeddedResourceProvider"/> class.
    /// </summary>
    /// <param name="assemblies">The assemblies to be scanned for the embedded resources</param>
    public ExtDefaultEmbeddedResourceProvider(IEnumerable<Assembly> assemblies)
        : this(assemblies.ToArray())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultEmbeddedResourceProvider"/> class.
    /// </summary>
    /// <param name="assemblies">The assemblies to be scanned for the embedded resources</param>
    public ExtDefaultEmbeddedResourceProvider(params Assembly[] assemblies)
    {
        //
        _assemblies = assemblies;
    }

    public IEnumerable<(string name, Assembly assembly)> GetEmbeddedResources()
    {
        ///////
        string sourcePath = @"/Users/joelobando/GSGUS/internalhrtool/DBMigration";
        if (_assemblies == null)
            yield break;

        foreach (var assembly in _assemblies)
        {
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
                    yield return (fileName, assembly);
                    // var destFile = System.IO.Path.Combine(targetPath, fileName);
                    //System.IO.File.Copy(s, destFile, true);
                }
            }
        }
    }
}