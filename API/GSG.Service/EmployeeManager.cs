using System.Linq.Expressions;
using GSG.Model;
using GSG.Model.DTO.Filters;
using GSG.Repository;
using GSG.Repository.Capability;
using GSG.Service.Interfaces;
using GSG.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace GSG.Service
{
    public class EmployeeManager : IEmployeeManager
    {
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IIdentityContext _context;
        private readonly ModelValidator _modelValidator;
        private readonly IRepository<Skill> _skillRepository;
        private readonly IRepository<Employer> _employerRepository;

        public EmployeeManager(IRepository<Employee> employeeRepository, IRepository<Skill> skillRepository,
            IRepository<Employer> employerRepository, IIdentityContext context, ModelValidator modelValidator)
        {
            _employeeRepository = employeeRepository;
            _skillRepository = skillRepository;
            _employerRepository = employerRepository;
            _context = context;
            _modelValidator = modelValidator;
        }

        public Employee CreateEmployee(Employee employee)
        {
            employee.CreatedBy = _context.UserName;

            var validation = _modelValidator.Validate(employee);
            if (!validation.IsValid)
            {
                throw new InvalidModelException("Model is not valid");
            }
            employee.CreatedBy = _context.UserName;

            IEnumerable<Employee> employees = _employeeRepository.GetAll().Where(row =>
                EF.Functions.Like(row.LastName, employee.LastName) &&
                EF.Functions.Like(row.FirstName, employee.FirstName) &&
                EF.Functions.Like(row.Email, employee.Email));

            if (employees.Any())
                throw new CreatingDuplicateException("Duplicate employee");

            return _employeeRepository.Insert(employee);
        }

        public bool DeleteEmployee(int employeeId)
        {
            Employee employee = _employeeRepository
                .Include($"{nameof(EmployeeProject)}")
                .Include($"{nameof(EmployeeSkill)}")
                .Include($"{nameof(EmployeeRole)}")
                .Include($"{nameof(EmployeeCertificate)}")
                .GetAll(row => row.EmployeeId == employeeId)
                .FirstOrDefault();

            if(employee is null)
                throw new DeletingByInvalidIdException("Employee does not exist");
            
            if (employee.EmployeeProject.Any())
                throw new DeletingEntityWithExistingAttachmentsException("Employee has an assigned Project");
            
            if (employee.EmployeeRole.Any())
                throw new DeletingEntityWithExistingAttachmentsException("Employee has an assigned Role");
            
            if (employee.EmployeeCertificate.Any())
                throw new DeletingEntityWithExistingAttachmentsException("Employee has an assigned Certificate");
            
            if (employee.EmployeeSkill.Any())
                throw new DeletingEntityWithExistingAttachmentsException("Employee has an assigned Skill");
            
            return _employeeRepository.Delete(employee);
        }

        public Employee GetEmployee(int employeeId)
        {
            Employee employee = _employeeRepository.GetBy(employeeId);
            if (employee is not null)
                return employee;
            throw new GettingByInvalidIdException("Invalid employeeId");
        }

        public IEnumerable<Employee> GetEmployees(EmployeeFilterDTO filter)
        {
            IQueryable<Employee> result = _employeeRepository.GetAll().AsQueryable();
            if (filter?.EmployeeId is not null)
            {
                result = result.Where(row => filter.EmployeeId
                .Contains(row.EmployeeId));
            }
            if (filter?.States is not null)
            {
                result = result.Where(row => filter.States
                .Contains(row.EmployeeState));
            }
            if (filter?.LastName is not null)
                result = result.Where(row => filter.LastName
                .Contains(row.LastName));

            return result;
        }
        public Employee UpdateEmployee(Employee employee)
        {
            var validation = _modelValidator.Validate(employee);
            if (!validation.IsValid)
            {
                throw new InvalidModelException("Model is not valid");
            }
            return _employeeRepository.Update(employee);
        }

        public Employee UpdateEmployee(int empId, Employee employee)
        {
            employee.EmployeeId = empId;
            switch (employee.CreatedBy)
            {
                case null:
                    employee.CreatedBy = _context.UserName;
                    break;
                default:
                    break;
            }
            var validation = _modelValidator.Validate(employee);
            if (!validation.IsValid)
            {
                throw new InvalidModelException("Model is not valid");
            }
            return _employeeRepository.Update(employee);
        }

        public IEnumerable<Employee> GetEmployeesFull(EmployeeFilterDTO filter)
        {
            IQueryable<Employee> result = _employeeRepository.Include($"{nameof(EmployeeProject)}.{nameof(Project)}")
                .Include($"{nameof(EmployeeSkill)}.{nameof(Skill)}").Include($"{nameof(EmployeeCertificate)}.{nameof(Certificate)}")
                .Include($"{nameof(EmployeeRole)}.{nameof(Role)}").Include($"{nameof(EmployeeRole)}.{nameof(Employer)}").GetAll().AsQueryable();

            if (filter?.EmployeeId is not null)
            {
                result = result.Where(row => filter.EmployeeId
                .Contains(row.EmployeeId));
            }
            if (filter?.LastName is not null && filter.LastName.Where(row => !string.IsNullOrWhiteSpace(row)).Any())
                result = result.Where(row => filter.LastName.Contains(row.LastName));

            if (filter?.SkillName is not null && filter.SkillName.Where(row => !string.IsNullOrWhiteSpace(row)).Any())
            {
                var skillsList = filter.SkillName.Select(row => row.ToLower()).ToList();
                List<int> skills = new List<int>();
                Expression<Func<Skill, bool>> predicate = (Item) => true;
                foreach (var term in skillsList)
                {
                    predicate = (Item) => true;
                    Expression<Func<Skill, bool>> left = item => item.SkillName.ToLower().Contains(term);
                    Expression right = predicate;
                    predicate = predicate.And(left);
                    List<int> skillsetss = _skillRepository.GetAll(predicate).Select(row => row.SkillId).ToList();
                    if (skillsetss.Any())
                    {
                        skills.AddRange(skillsetss);
                    }
                    predicate = (Item) => false;
                }


                result = result.Where(row => row.EmployeeSkill.Any(empSkill => skills.Any(skill => empSkill.SkillId == skill)));
            }
            if (filter?.EmployerName is not null && filter.EmployerName.Where(row => !string.IsNullOrWhiteSpace(row)).Any())
            {
                IEnumerable<Employer> employers = _employerRepository.GetAll(row => filter.EmployerName.Contains(row.EmployerName));
                result = result.Where(row => row.EmployeeRole.Any(empRole =>
                    employers.Any(emprs => empRole.Employer.EmployerName.Equals(emprs.EmployerName))));
            }


            return result;
        }

    }
}
