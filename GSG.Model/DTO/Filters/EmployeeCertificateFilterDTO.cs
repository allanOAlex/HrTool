using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSG.Model.DTO.Filters
{
    public class EmployeeCertificateFilterDTO
    {
        public int[] EmployeeId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public int[] CertificateId { get; set; }
        public string[] CertificateName { get; set; }

    }
}
