using GSG.Model;
using GSG.Model.DTO.Requests;
using GSG.Model.DTO.Responses;
using GSG.Shared;

namespace GSG.WebApp.Client.Services.EmployeeService
{
	public interface IEmployeeService
	{
		Task<ResponseBody<List<EmployeeGridResponse>>> GetEmployeesFull();
        Task<ResponseBody<EmployeeResponse>> GetEmployeeById(int Id);
        Task<ResponseBody<EmployeeRequest>> CreateNewEmployee(EmployeeRequest empRequest);
        Task<ResponseBody<EmployeeResponse>> GetEmployeeToUpdate(int Id);
        Task<ResponseBody<EmployeeResponse>> UpdateEmployee(int empId, EmployeeRequest updateEmpRequest);



        Task<ResponseBody<List<CertificateResponse>>> GetCertificates();
        Task<ResponseBody<CertificateResponse>> GetCertificateToAdd(int Id);
        Task<ResponseBody<EmployeeCertificateResponse>> CreateEmployeeCertificate(EmployeeCertificateRequest empCert);
        Task<string> CreateEmpCertificate(EmployeeCertificateRequest empCert);
        Task<ResponseBody<List<EmployeeCertificateProfileResponse>>> GetEmployeeCertificates(string url);
        Task<ResponseBody<EmployeeCertificateResponse>> GetEmpCertToDelete(int certificateId);
        Task<ResponseBody<string>> DeleteEmployeeCertificate(int certId, int empId);


        Task<ResponseBody<List<SkillResponse>>> GetSkills();
        Task<ResponseBody<SkillResponse>> GetSkillToAdd(int Id);
        Task<ResponseBody<EmployeeSkillResponse>> CreateEmployeeSkill(EmployeeSkillRequest empSkill);
        Task<ResponseBody<List<EmployeeSkillProfileResponse>>> GetEmployeeSkills(string url);
        Task<ResponseBody<EmployeeSkillResponse>> GetEmpSkillToDelete(int skillId);
        Task<ResponseBody<string>> DeleteEmployeeSkill(int certId, int empId);


        Task<ResponseBody<List<RoleResponse>>> GetRoles();
        Task<ResponseBody<List<ProjectResponse>>> GetProjects();
        
        
    }
}
