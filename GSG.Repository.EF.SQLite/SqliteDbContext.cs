using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GSG.Repository.EF.SQLite;

public class SqliteDbContext: ApplicationContext
{
    private readonly string _connectionString;

    public SqliteDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    protected override void ConfigureContext(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_connectionString);
    }

    public override IDbContextTransaction GetContextTransaction()
    {
        throw new NotImplementedException();
    }

    protected override void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
    }
}