using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GSG.Repository.EF;

public class InMemoryContext : ApplicationContext
{
    private readonly string _dbName;

    public InMemoryContext(string dbName)
    {
        _dbName = dbName;
    }

    protected override void ConfigureContext(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: _dbName);
    }

    public override IDbContextTransaction GetContextTransaction()
    {
        throw new NotImplementedException();
    }

    protected override void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
    }
}