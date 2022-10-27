using Microsoft.EntityFrameworkCore.Storage;

namespace GSG.Repository.EF;

public class UnitOfWorkRepository : BaseAbstractRepository
{
    private UnitOfWorkRepository(
        ApplicationContext dbContext,
        IDbContextTransaction dbContextTransaction) : base(dbContext, dbContextTransaction)
    {
    }

    public UnitOfWorkRepository(ApplicationContext dbContext) : base(dbContext)
    {
    }

    protected override BaseAbstractRepository GetNewInstance(
        ApplicationContext context,
        IDbContextTransaction contextTransaction)
    {
        return new UnitOfWorkRepository(DBContext, DBContextTransaction);
    }
}