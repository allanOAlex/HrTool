using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GSG.Repository.Capability
{
    public interface IRepository<T> :
        IUpdatable<T>,
        IDeletable<T>,
        IGettable<T, Expression<Func<T, bool>>>,
        IGettableBy<T, int>,
        IInsertable<T>,

        IIncludable<T, string, Expression<Func<T, bool>>> where T : class
    {
    }
}
