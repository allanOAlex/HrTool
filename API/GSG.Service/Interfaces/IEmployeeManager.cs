using GSG.Model;
using GSG.Model.DTO.Filters;

namespace GSG.Service.Interfaces
{
    public interface IEmployeeManager
    {
        Employee CreateEmployee(Employee employee);
        bool DeleteEmployee(int employeeId);
        IEnumerable<Employee> GetEmployees(EmployeeFilterDTO filter);
        Employee GetEmployee(int employeeId);
        IEnumerable<Employee> GetEmployeesFull(EmployeeFilterDTO filter);
        public Employee UpdateEmployee(Employee employee);
        public Employee UpdateEmployee(int empId, Employee employee);
    }
}
