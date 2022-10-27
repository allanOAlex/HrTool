using System.Linq.Expressions;
using GSG.Repository.Capability;
using Microsoft.EntityFrameworkCore;

namespace GSG.Repository.EF;

public class EFRepository<T> : AbstractRepository<T> where T : class
{
    private readonly List<string> _includes;

    private EFRepository(ApplicationContext context, List<string> includes) : base(context)
    {
        _includes = includes;
    }

    public EFRepository(ApplicationContext dbContext) : base(dbContext)
    {
        _includes = new List<string>();
    }

    public override IQueryable<T> GetAll(Expression<Func<T, bool>> criteria = null)
    {
        var set = DBContext.Set<T>().AsQueryable().AsNoTracking();
        if (criteria != null)
        {
            set = set.Where(criteria);
        }

        if (_includes is not null)
        {
            foreach (var include in _includes)
            {
                set = set.Include(include);
            }
        }

        return set;
    }

    public override IIncludable<T, string, Expression<Func<T, bool>>> Include(string constraint)
    {
        _includes.Add(constraint);
        return new EFRepository<T>(DBContext, _includes);
    }

    public override IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
    {
        return DBContext.Set<T>().Where(expression).AsNoTracking();
    }


}