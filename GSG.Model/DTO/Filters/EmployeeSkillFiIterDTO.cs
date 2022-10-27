using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSG.Model.DTO.Filters
{
    public class EmployeeSkillFilterDTO
    {
        public int[] SkillIds { get; set; }
        public int[] EmployeeId { get; set; }
        public string[] SkillName { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
