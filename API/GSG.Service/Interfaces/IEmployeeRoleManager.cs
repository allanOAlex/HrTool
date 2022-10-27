using GSG.Model;
using GSG.Model.DTO.Filters;

namespace GSG.Service.Interfaces
{
    public interface IEmployeeRoleManager
    {
        EmployeeRole AssignEmployeeRole(EmployeeRole employeeRole);
        bool UnassignEmployeeRole(int employeeRoleId);
        IEnumerable<EmployeeRole> GetEmployeeRoles(EmployeeRoleFilterDTO filter);
        EmployeeRole GetEmployeeRole(int id);
        EmployeeRole GetEmployeeRole(int employeeId,int roleId);
        EmployeeRole UpdateEmployeeRole(EmployeeRole empRole);
    }
}
