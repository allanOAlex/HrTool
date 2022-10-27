//---------------------------------------------------------------------------------------
//       This code was generated from a template
//       Manual changes to this file may cause unexpected behavior in your application.
//       Manual changes to this file will be overwritten if the code is regenerated.
//---------------------------------------------------------------------------------------


using System;
using System.Collections.Generic;

namespace GSG.Model
{
    public partial class Role
    {
        public Role()
        {
            EmployeeRole = new HashSet<EmployeeRole>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string CreatedBy { get; set; }
        public Guid RowVer { get; set; }

        public virtual ICollection<EmployeeRole> EmployeeRole { get; set; }
    }
}
