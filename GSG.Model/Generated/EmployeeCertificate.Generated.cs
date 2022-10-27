//---------------------------------------------------------------------------------------
//       This code was generated from a template
//       Manual changes to this file may cause unexpected behavior in your application.
//       Manual changes to this file will be overwritten if the code is regenerated.
//---------------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GSG.Model
{
    public partial class EmployeeCertificate
    {
        public int EmployeeCertificateId { get; set; }
        public int CertificateId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime AwardedDate { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string CreatedBy { get; set; }
        public Guid RowVer { get; set; }

        public virtual Certificate Certificate { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
