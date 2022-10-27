using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GSG.Repository.Capability
{
    public interface IGettable<T, C> where C : class
    {
        IQueryable<T> GetAll(C criteria = null);
    }
    public interface IGettable<T> : IGettable<T, Expression<Func<T, bool>>>
    {

    }
}
