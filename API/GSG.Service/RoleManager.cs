using GSG.Model;
using GSG.Model.DTO.Filters;
using GSG.Repository;
using GSG.Repository.Capability;
using GSG.Service.Interfaces;
using GSG.Shared.Exceptions;

namespace GSG.Service
{
    public class RoleManager : IRoleManager
    {
        public IRepository<Role> _roleRepository;
        private readonly IIdentityContext _context;
        private readonly ModelValidator _modelValidator;

        public RoleManager(IRepository<Role> roleRepository, IIdentityContext context, ModelValidator modelValidator)
        {
            _roleRepository = roleRepository;
            _context = context;
            _modelValidator = modelValidator;
        }

        public Role CreateRole(Role role)
        {
            var validation = _modelValidator.Validate(role);
            if (!validation.IsValid)
            {
                throw new InvalidModelException("Model is not valid");
            }
            role.CreatedBy = _context.UserName;
            Role checker = _roleRepository.GetAll(row => row.RoleName.Equals(role.RoleName)).FirstOrDefault();
            if (checker is null)
                return _roleRepository.Insert(role);

            throw new CreatingDuplicateException("Role already exists");
        }

        public bool DeleteRole(int roleId)
        {
            Role existingRole = _roleRepository.Include($"{nameof(EmployeeRole)}").GetAll(row => row.RoleId == roleId).FirstOrDefault();

            if (existingRole is not null)
            {
                if (existingRole.EmployeeRole.Any())
                    throw new DeletingEntityWithExistingAttachmentsException("Role has attached EmployeeRoles");
                else
                    return _roleRepository.Delete(existingRole);
            }
            else
                throw new DeletingByInvalidIdException("RoleId does not exist");
        }

        public Role GetRole(int roleId)
        {
            Role results = _roleRepository.GetBy(roleId);
            if (results is not null)
                return results;
            else
                throw new GettingByInvalidIdException("Invalid RoleId");
        }

        public IEnumerable<Role> GetRoles(RoleFilterDTO filter)
        {
            if (filter?.RoleName is not null)
                return _roleRepository.GetAll(row => filter.RoleName.
                    Contains(row.RoleName));

            if (filter?.RoleId is not null)
                return _roleRepository.GetAll(row => filter.RoleId
                    .Contains(row.RoleId));

            return _roleRepository.GetAll();
        }

        public Role UpdateRole(Role role)
        {
            var validation = _modelValidator.Validate(role);
            if (!validation.IsValid)
            {
                throw new InvalidModelException("Model is not valid");
            }
            return _roleRepository.Update(role);
        }
    }
}