using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GSG.Repository.EF;

public class PostgreSqlContext: ApplicationContext
{
    private readonly string _connectionString;

    public PostgreSqlContext(string connectionString)
    {
        _connectionString = connectionString;
    }
    protected override void ConfigureContext(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }

    public override IDbContextTransaction GetContextTransaction()
    {
        throw new NotImplementedException();
    }

    protected override void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
    }
}