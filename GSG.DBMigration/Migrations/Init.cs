using FluentMigrator;
using FluentMigrator.Builders.Execute;
using FluentMigrator.Infrastructure;

namespace GSG.DBMigration.Migrations;

[Migration(0000000000, "Init")]
public class Init0000000000 : GSGMigration
{
    internal IMigrationContext _context;
    private readonly object _mutex = new object();

    public override void Up()
    {
        Execute.EmbeddedScript("0000000000.sql");
    }

    public override void Down()
    {
    }

    /// <summary>
    /// Gets the starting point for SQL execution
    /// </summary>
    public new IExecuteExpressionRoot Execute
    {
        get { return new ExtExecuteExpressionRoot(_context); }
    }

    /// <inheritdoc />
    public override void GetUpExpressions(IMigrationContext context)
    {
        lock (_mutex)
        {
            _context = context;
            base.GetUpExpressions(context);
            _context = null;
        }
    }

    /// <inheritdoc />
    public override void GetDownExpressions(IMigrationContext context)
    {
        lock (_mutex)
        {
            _context = context;
            base.GetDownExpressions(context);
            _context = null;
        }
    }
}

public abstract class GSGMigration : Migration
{
    internal IMigrationContext _context;
    private readonly object _mutex = new object();


    /// <summary>
    /// Gets the starting point for SQL execution
    /// </summary>
    public new IExecuteExpressionRoot Execute
    {
        get { return new ExtExecuteExpressionRoot(_context); }
    }

    /// <inheritdoc />
    public override void GetUpExpressions(IMigrationContext context)
    {
        lock (_mutex)
        {
            _context = context;
            base.GetUpExpressions(context);
            _context = null;
        }
    }

    /// <inheritdoc />
    public override void GetDownExpressions(IMigrationContext context)
    {
        lock (_mutex)
        {
            _context = context;
            base.GetDownExpressions(context);
            _context = null;
        }
    }
}