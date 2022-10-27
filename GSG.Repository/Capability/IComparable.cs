using GSG.Repository.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSG.Repository.Capability
{
    public interface IComparable<T>
    {
        IEnumerable<Varience> Compare(T sideA, T sideB);
    }
}
