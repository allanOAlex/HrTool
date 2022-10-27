//---------------------------------------------------------------------------------------
//       This code was generated from a template
//       Manual changes to this file may cause unexpected behavior in your application.
//       Manual changes to this file will be overwritten if the code is regenerated.
//---------------------------------------------------------------------------------------


using System;
using System.Collections.Generic;

namespace GSG.Model
{
    public partial class Skill
    {
        public Skill()
        {
            EmployeeSkill = new HashSet<EmployeeSkill>();
        }

        public int SkillId { get; set; }
        public string SkillName { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string CreatedBy { get; set; }
        public Guid RowVer { get; set; }

        public virtual ICollection<EmployeeSkill> EmployeeSkill { get; set; }
    }
}
