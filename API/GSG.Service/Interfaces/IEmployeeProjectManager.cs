using GSG.Model;
using GSG.Model.DTO.Filters;

namespace GSG.Service.Interfaces
{
    public interface IEmployeeProjectManager
    {
        EmployeeProject AssignEmployeeProject(EmployeeProject employeeProject);
        bool UnassignEmployeeProject(int employeeProjectId);
        IEnumerable<EmployeeProject> GetEmployeeProjects(EmployeeProjectFilterDTO filter);
        EmployeeProject GetEmployeeProject(int id);
        EmployeeProject GetEmployeeProject(int employeeId, int projectId);
        EmployeeProject UpdateEmployeeProject(EmployeeProject project);
    }
}