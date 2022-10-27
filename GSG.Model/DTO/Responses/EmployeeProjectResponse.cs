using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSG.Model.DTO.Responses
{
    public class EmployeeProjectResponse
    {
        public int EmployeeProjectId { get; set; }
        public int? EmployeeId { get; set; }
        public int? ProjectId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
