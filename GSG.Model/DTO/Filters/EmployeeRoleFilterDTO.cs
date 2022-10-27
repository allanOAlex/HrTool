using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSG.Model.DTO.Filters
{
    public class EmployeeRoleFilterDTO
    {
        public int [] RoleId { get; set; }
        public int[] EmployeeId { get; set; }
        public int[] EmployerID { get; set; }
        public string[] EmployerName { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string[] RoleName { get; set; }

    }
}
