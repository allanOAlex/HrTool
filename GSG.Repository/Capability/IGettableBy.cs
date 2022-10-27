using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSG.Repository.Capability
{
    public interface IGettableBy<T, C>
    {
        T GetBy(C constraint);
    }
}
