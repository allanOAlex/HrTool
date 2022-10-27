//---------------------------------------------------------------------------------------
//       This code was generated from a template
//       Manual changes to this file may cause unexpected behavior in your application.
//       Manual changes to this file will be overwritten if the code is regenerated.
//---------------------------------------------------------------------------------------


using System;
using System.Collections.Generic;

namespace GSG.Model
{
    public partial class EmployeeRole
    {
        public int EmployeeRoleId { get; set; }
        public int EmployeeId { get; set; }
        public int RoleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? EmployerId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string CreatedBy { get; set; }
        public Guid RowVer { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Employer Employer { get; set; }
        public virtual Role Role { get; set; }
    }
}
