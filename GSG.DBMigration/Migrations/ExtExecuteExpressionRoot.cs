using System.Diagnostics;
using FluentMigrator.Builders.Execute;
using FluentMigrator.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace GSG.DBMigration.Migrations;

public class ExtExecuteExpressionRoot : ExecuteExpressionRoot, IExecuteExpressionRoot
{
    private readonly IMigrationContext _context;
    /// <inheritdoc />
    public ExtExecuteExpressionRoot(IMigrationContext context) : base(context)
    {
        _context = context;
    }
    public new void EmbeddedScript(string embeddedSqlScriptName)
    {
        var embeddedResourceProviders = _context.ServiceProvider.GetService<IEnumerable<IEmbeddedResourceProvider>>();
        if (embeddedResourceProviders == null)
        {
#pragma warning disable 612
            Debug.Assert(_context.MigrationAssemblies != null, "_context.MigrationAssemblies != null");
            var expression = new ExtExecuteEmbeddedSqlScriptExpression(_context.MigrationAssemblies) { SqlScript = embeddedSqlScriptName };
#pragma warning restore 612
            _context.Expressions.Add(expression);
        }
        else
        {
            var expression = new ExtExecuteEmbeddedSqlScriptExpression(embeddedResourceProviders) { SqlScript = embeddedSqlScriptName };
            _context.Expressions.Add(expression);
        }
    }
}