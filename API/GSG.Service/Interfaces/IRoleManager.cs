using GSG.Model;
using GSG.Model.DTO.Filters;

namespace GSG.Service.Interfaces
{
    public interface IRoleManager
    {
        Role CreateRole(Role role);
        bool DeleteRole(int roleId);
        IEnumerable<Role> GetRoles(RoleFilterDTO filter);
        Role GetRole(int roleId);
        Role UpdateRole(Role Role);
    }
}