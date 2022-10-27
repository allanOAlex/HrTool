using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSG.Repository.Capability
{
    public interface IIncludable<T, C, K> : IGettable<T, K> 
        where C : class
        where K : class
    {
        IIncludable<T, C, K> Include(C constraint);
    }
}
