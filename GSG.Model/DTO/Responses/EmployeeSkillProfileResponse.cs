using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSG.Model.DTO.Responses
{
    public class EmployeeSkillProfileResponse
    {
        public int EmployeeSkillId { get; set; }
        public int SkillId { get; set; }
        public int? EmployeeId { get; set; }
        public string? SkillName { get; set; }
    }
}
