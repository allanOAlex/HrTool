using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSG.Model.DTO.Dtos
{
	public class EmployeeCertificateDto
	{
		public int EmployeeCetificateId { get; set; }
		public int CertificateId { get; set; }
		public int EmployeeId { get; set; }
		public DateTime AwarededDate { get; set; }
		public DateTime Created { get; set; }
		public DateTime Updated { get; set; }
		public string CreatedBy { get; set; }
		public Guid RowVer { get; set; }
	}
}
