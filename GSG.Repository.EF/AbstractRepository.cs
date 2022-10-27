using System.Linq.Expressions;
using GSG.Repository.Capability;
using Microsoft.EntityFrameworkCore;

namespace GSG.Repository.EF;

public abstract class AbstractRepository<T> : UnitOfWorkRepository, IRepository<T> where T : class
{
    protected AbstractRepository(ApplicationContext dbContext) : base(dbContext)
    {
    }

    public T Update(T entity)
    {
        int id = DBContext.GetKey(entity);
        var exisiting = DBContext.Set<T>().Find(id);
        if (exisiting == null)
        {
            return Insert(entity);
        }
        
        DBContext.Entry(exisiting).CurrentValues.SetValues(entity);
        
        DBContext.ChangeTracker.TrackGraph(exisiting, e =>
        {
            if (e.Entry.Entity.GetType() == typeof(T))
            {
                e.Entry.State = EntityState.Modified;
            }
        });
        DBContext.SaveChanges();
        DBContext.ChangeTracker.Clear();
        return entity;
    }

    public bool Delete(T entity)
    {
        DBContext.Entry(entity).State = EntityState.Deleted;
        return DBContext.SaveChanges() > 0 ? true : false;
    }

    public abstract IQueryable<T> GetAll(Expression<Func<T, bool>> criteria = null);

    public T Insert(T entity)
    {
        DBContext.ChangeTracker.TrackGraph(entity, e =>
        {
            if (e.Entry.Entity.GetType() == typeof(T))
            {
                if (e.Entry.IsKeySet && DBContext.GetKey(entity) > 0)
                {
                    throw new Exception("Cannot add entity with a primary key already set");
                }
                e.Entry.State = EntityState.Added;
            }
        });
        DBContext.SaveChanges();
        DBContext.Entry(entity).State = EntityState.Detached;
        DBContext.ChangeTracker.Clear();
        return entity;
    }

    public abstract IIncludable<T, string, Expression<Func<T, bool>>> Include(string constraint);

    public T GetBy(int constraint)
    {
        var entity = DBContext.Set<T>().Find(constraint);
        if (entity != null)
        {
            DBContext.ChangeTracker.Clear();
            return entity;
        }
        DBContext.ChangeTracker.Clear();
        return null;
    }

    public abstract IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
}