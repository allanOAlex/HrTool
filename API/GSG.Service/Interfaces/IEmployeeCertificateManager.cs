using GSG.Model;
using GSG.Model.DTO.Filters;

namespace GSG.Service.Interfaces
{
    public interface IEmployeeCertificateManager
    {
        EmployeeCertificate AssignEmployeeCertificate(EmployeeCertificate employeeCertificate);
        bool UnassignEmployeeCertificate(int employeeCertificateId);
        bool UnassignEmployeeCertificate(int certId, int empId);
        IEnumerable<EmployeeCertificate> GetEmployeeCertificates(EmployeeCertificateFilterDTO filter);
        EmployeeCertificate GetEmployeeCertificate(int id);
        EmployeeCertificate GetEmployeeCertificate(int employeeId, int certificateId);
        EmployeeCertificate UpdateEmployeeCertificate(EmployeeCertificate employeeCertificate);
        EmployeeCertificate CreateEmployeeCertificate(EmployeeCertificate employeeCertificate);
    }
}
