using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSG.Model.DTO.Filters
{
    public class EmployeeFilterDTO
    {
        public int[] EmployeeId { get; set; }
        public string[] States { get; set; }
        public string[] LastName { get; set; }
        public string[] FirstName { get; set; }
        public string[] SkillName { get; set; }
        public string[] EmployerName { get; set; }
    }
}
