//---------------------------------------------------------------------------------------
//       This code was generated from a template
//       Manual changes to this file may cause unexpected behavior in your application.
//       Manual changes to this file will be overwritten if the code is regenerated.
//---------------------------------------------------------------------------------------


using System;
using System.Collections.Generic;

namespace GSG.Model
{
    public partial class Employee
    {
        public Employee()
        {
            EmployeeCertificate = new HashSet<EmployeeCertificate>();
            EmployeeProject = new HashSet<EmployeeProject>();
            EmployeeRole = new HashSet<EmployeeRole>();
            EmployeeSkill = new HashSet<EmployeeSkill>();
        }

        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PictureUrl { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string EmployeeState { get; set; }
        public DateOnly Created { get; set; }
        public DateTime Updated { get; set; }
        public string CreatedBy { get; set; }
        public Guid RowVer { get; set; }

        public virtual ICollection<EmployeeCertificate> EmployeeCertificate { get; set; }
        public virtual ICollection<EmployeeProject> EmployeeProject { get; set; }
        public virtual ICollection<EmployeeRole> EmployeeRole { get; set; }
        public virtual ICollection<EmployeeSkill> EmployeeSkill { get; set; }
    }
}
