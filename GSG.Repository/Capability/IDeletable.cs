using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSG.Repository.Capability
{
    public interface IDeletable<T>
    {
        bool Delete(T entity);
    }
}
