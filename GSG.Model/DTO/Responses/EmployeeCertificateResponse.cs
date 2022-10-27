using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSG.Model.DTO.Responses
{
    public class EmployeeCertificateResponse
    {
        public int EmployeeCertificateId { get; set; }
        public int CertificateId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime AwardedDate { get; set; }
    }
}
