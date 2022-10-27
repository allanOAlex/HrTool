using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSG.Repository.Helper
{
    public class Varience
    {
        public Varience(string memberPath, string value1, string value2)
        {
            MemberPath = memberPath;
            Value1 = value1;
            Value2 = value2;

        }
        public string MemberPath { get; }
        public string Value1 { get; }
        public string Value2 { get; }
    }
}
