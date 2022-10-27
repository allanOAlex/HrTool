using GSG.Repository.Capability;
using Microsoft.EntityFrameworkCore.Storage;

namespace GSG.Repository.EF;

public abstract class BaseAbstractRepository : IUnitOfWork
{
    protected readonly ApplicationContext DBContext;
    protected readonly IDbContextTransaction DBContextTransaction;

    protected BaseAbstractRepository(ApplicationContext dbContext,
        IDbContextTransaction dbContextTransaction) : this(dbContext)
    {
        DBContext = dbContext;
        DBContextTransaction = dbContextTransaction;
    }

    protected BaseAbstractRepository(ApplicationContext dbContext)
    {
        DBContext = dbContext;
    }

    public void Dispose()
    {
        if (DBContextTransaction != null)
        {
            try
            {
                DBContextTransaction.Commit();
            }
            catch (Exception ex)
            {
                DBContextTransaction.Rollback();
            }

            DBContextTransaction.Dispose();
        }

        GC.SuppressFinalize(this);
    }

    public IUnitOfWork StartUnitOfWork()
    {
        return GetNewInstance(DBContext, DBContext.GetContextTransaction());
    }


    protected abstract BaseAbstractRepository GetNewInstance(ApplicationContext context,
        IDbContextTransaction contextTransaction);
}