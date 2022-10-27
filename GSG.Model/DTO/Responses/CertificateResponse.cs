using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSG.Model.DTO.Responses
{
    public class CertificateResponse
    {
        public int CertificateId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CertificateName { get; set; }
    }
}
