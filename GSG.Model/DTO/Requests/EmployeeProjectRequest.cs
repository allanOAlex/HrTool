using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSG.Model.DTO.Requests
{
    public class EmployeeProjectRequest
    {
        public int? EmployeeId { get; set; }
        public int? ProjectId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
