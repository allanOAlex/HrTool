using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GSG.Repository.EF;

public class SqldbContext : ApplicationContext
{
    private readonly string _connectionString;

    public SqldbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void ConfigureContext(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }

    public override IDbContextTransaction GetContextTransaction()
    {
        throw new NotImplementedException();
    }

    protected override void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
       
    }
}