using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSG.Model.DTO.Filters
{
    public class ProjectFilterDTO
    {
        public string[] ProjectName { get; set; }
        public int[] ProjectId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
