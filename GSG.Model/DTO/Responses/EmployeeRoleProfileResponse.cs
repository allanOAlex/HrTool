using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSG.Model.DTO.Responses
{
    public class EmployeeRoleProfileResponse
    {
        public int EmployeeRoleId { get; set; }
        public int? RoleId { get; set; }
        public int? EmployeeId { get; set; }
        public string? RoleName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string EmployerName { get; set; }
    }
}
