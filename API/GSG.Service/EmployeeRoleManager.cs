using GSG.Model;
using GSG.Model.DTO.Filters;
using GSG.Repository;
using GSG.Repository.Capability;
using GSG.Shared.Exceptions;
using GSG.Service.Interfaces;

namespace GSG.Service
{
    public class EmployeeRoleManager : IEmployeeRoleManager
    {
        public IRepository<EmployeeRole> _employeeRoleRepository;
        public IRepository<Role> _roleRepository;
        private readonly IIdentityContext _context;
        private readonly ModelValidator _modelValidator;

        public EmployeeRoleManager(IRepository<EmployeeRole> employeeRoleRepository, IRepository<Role> roleRepository
            , IIdentityContext context, ModelValidator modelValidator)
        {
            _employeeRoleRepository = employeeRoleRepository;
            _roleRepository = roleRepository;
            _context = context;
            _modelValidator = modelValidator;
        }

        public EmployeeRole AssignEmployeeRole(EmployeeRole employeeRole)
        {
            var validation = _modelValidator.Validate(employeeRole);
            if (!validation.IsValid)
            {
                throw new InvalidModelException("Model is not valid");
            }
            employeeRole.CreatedBy = _context.UserName;
            EmployeeRole existingRole = _employeeRoleRepository.GetAll(
                    row => row.RoleId == employeeRole.RoleId &&
                    row.EmployeeId == employeeRole.EmployeeId).FirstOrDefault();
            if (existingRole is not null)
                throw new CreatingDuplicateException("EmployeeRole already exists");

            return _employeeRoleRepository.Insert(employeeRole);
        }

        public EmployeeRole GetEmployeeRole(int id)
        {
            EmployeeRole response = _employeeRoleRepository.GetBy(id);
            if (response is null)
                throw new GettingByInvalidIdException("EmployeeRoleId does not exist");
            return response;
        }

        public EmployeeRole GetEmployeeRole(int employeeId, int roleId)
        {
            EmployeeRole existingEmployeeRole = _employeeRoleRepository.GetAll(row =>
                row.EmployeeId == employeeId && row.RoleId == roleId).FirstOrDefault();
            if (existingEmployeeRole is not null)
                return existingEmployeeRole;
            throw new GettingByInvalidIdException("EmployeeRole does not exist for the given Employee and Role Id's");
        }

        public IEnumerable<EmployeeRole> GetEmployeeRoles(EmployeeRoleFilterDTO filter)
        {
            IQueryable<EmployeeRole> result = _employeeRoleRepository.Include($"{nameof(Role)}").Include($"{nameof(Employer)}")
                .GetAll().AsQueryable();

            if (filter?.EmployeeId is not null)
                result = result.Where(row => filter.EmployeeId.Any(id => id ==row.EmployeeId));

            if (filter?.RoleId is not null)
                result = result.Where(row => filter.RoleId.Any(id => id == row.RoleId));

            if (filter?.RoleName is not null)
            {
                IEnumerable<Role> roles =
                    _roleRepository
                        .Include($"{nameof(EmployeeRole)}.{nameof(Employee)}")
                        .GetAll(row => filter.RoleName.Contains(row.RoleName));

                result = result.Where(row => roles.Any(rol => rol.RoleId == row.RoleId));
            }
            if (filter?.From is not null)
                result = result.Where(row => DateTime.Compare(filter.From.Value, row.StartDate) < 0);

            if (filter?.To is not null)
                result = result.Where(row => row.EndDate.HasValue && filter.To.Value.CompareTo(row.EndDate) > 0);

            return result;
        }

        public bool UnassignEmployeeRole(int employeeRoleId)
        {
            EmployeeRole existingEmployeeRole = _employeeRoleRepository.GetAll().
                Where<EmployeeRole>(record => record.EmployeeRoleId == employeeRoleId).FirstOrDefault();

            if (existingEmployeeRole is not null)
                return _employeeRoleRepository.Delete(existingEmployeeRole);
            else throw new DeletingByInvalidIdException("EmployeeRoleID does not exist");
        }

        public EmployeeRole UpdateEmployeeRole(EmployeeRole empRole)
        {
            var validation = _modelValidator.Validate(empRole);
            if (!validation.IsValid)
            {
                throw new InvalidModelException("Model is not valid");
            }
            return _employeeRoleRepository.Update(empRole);
        }
    }
}
