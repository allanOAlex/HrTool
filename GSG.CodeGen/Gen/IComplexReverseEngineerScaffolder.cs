using Microsoft.EntityFrameworkCore.Scaffolding;

namespace GSG.CodeGen.Gen;

public interface IComplexReverseEngineerScaffolder 
{
    void Save(
        ScaffoldedFile code,
        string outputDir,
        bool overwriteFiles);
}