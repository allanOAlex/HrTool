using GSG.Model;
using GSG.Model.DTO.Filters;


namespace GSG.Service.Interfaces
{
    public interface IEmployeeSkillManager
    {
        EmployeeSkill AssignEmployeeSkill(EmployeeSkill employeeSkill);
        bool UnassignEmployeeSkill(int employeeSkillId);
        bool UnassignEmployeeSkill(int employeeId, int skillId);
        IEnumerable<EmployeeSkill> GetEmployeeSkills(EmployeeSkillFilterDTO filter);
        EmployeeSkill CreateEmployeeSkill(EmployeeSkill employeeSkill);
        EmployeeSkill GetEmployeeSkill(int id);
        EmployeeSkill GetEmployeeSkill(int employeeId, int skillId);
        EmployeeSkill UpdateEmployeeSkill(EmployeeSkill empSkill);
        bool DeleteEmployeeSkill(int skillId, int empId);
    }
}