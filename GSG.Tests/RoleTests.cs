using Autofac;
using GSG.Model;
using GSG.Repository.Capability;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using GSG.Model.DTO.Filters;
using GSG.Shared.Exceptions;
using GSG.Service.Interfaces;

namespace GSG.Tests
{
    public class RoleTests : BaseServiceTest
    {
        IContainer _container;
        IRoleManager _roleManager;
        IRepository<Role> _roleRepository;
        IRepository<EmployeeRole> _employeeRoleRepository;

        protected override void PostSetup(IContainer container)
        {
            _roleRepository = container.Resolve<IRepository<Role>>();
            _roleManager = container.Resolve<IRoleManager>();
            _employeeRoleRepository = container.Resolve<IRepository<EmployeeRole>>();
        }

        [TearDown]
        public void CleanUp()
        {
            IEnumerable<Role> trash = _roleRepository.GetAll();
            foreach(Role role in trash)
                _roleRepository.Delete(role);
            IEnumerable<EmployeeRole> trash2 = _employeeRoleRepository.GetAll();
        }

        [Test]
        public void CanCreateRole()
        {
            Role role = new Role {RoleName = "TestName", CreatedBy = "TestName2"};
            Role response = _roleManager.CreateRole(role);
            Assert.True(response is not null);
            Assert.AreEqual(GetTestIdentityContext().UserName, response.CreatedBy);
        }

        [Test]
        public void CanSetCreatedByOnCreateCertificate()
        {
            Role response = _roleManager.CreateRole(new Role { RoleName = "test7", CreatedBy = "6" });

            Assert.AreEqual(GetTestIdentityContext().UserName, response.CreatedBy);
        }

        [Test]
        public void CanGetRoleByRoleId()
        {
            Role entry = _roleRepository.Insert(new Role { RoleName = "TestName", CreatedBy = "TestName2" });

            var role = _roleManager.GetRole(entry.RoleId);
            Assert.True(role is not null);
        }

        [Test]
        public void CanGetAllRoles()
        {
            _roleRepository.Insert(new Role { RoleName = "TestName", CreatedBy = "TestName2" });
            _roleRepository.Insert(new Role { RoleName = "TestName3", CreatedBy = "TestName4" });

            IEnumerable<Role> roles = _roleManager.GetRoles(null);
            Assert.AreEqual(2, roles.Count());
        }

        [Test]
        public void CannotInsertDuplicateRole()
        {
            try
            {
                _roleRepository.Insert(new Role { RoleName = "TestName", CreatedBy = "TestName2" });
                _roleManager.CreateRole(new Role { RoleName = "TestName", CreatedBy = "TestName2" });
                IEnumerable<Role> roles = _roleRepository.GetAll();
            }
            catch (CreatingDuplicateException)
            {
                Assert.Pass();
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void CanGetRoleByRoleName()
        {
            _roleRepository.Insert(new Role { RoleName = "TestName", CreatedBy = "TestName2" });
            IEnumerable<Role> role = _roleManager.GetRoles(new RoleFilterDTO 
            { RoleName = new string[] { "TestName" } });

            Assert.IsTrue(role is not null);
            Assert.AreEqual(1, role.Count());
        }

        [Test]
        public void CanDeleteRole()
        {
            Role role = _roleRepository.Insert(new Role { RoleName = "TestName", CreatedBy = "TestName2" });
            bool exists = _roleManager.DeleteRole(role.RoleId);
            Role test = _roleRepository.GetBy(role.RoleId);
            Assert.IsNull(test);
            Assert.IsTrue(exists);
        }

        [Test]
        public void CannotDeleteRoleWithInvalidId()
        {
            Role inserted = _roleRepository.Insert(new Role { RoleName = "test", CreatedBy = "test2" });
            try
            {
                _roleManager.DeleteRole(inserted.RoleId + 24);
            }
            catch (DeletingByInvalidIdException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CanGetMultipleRolesById()
        {
            _roleRepository.Insert(new Role { RoleName = "TestName", CreatedBy = "TestName2" });
            _roleRepository.Insert(new Role { RoleName = "TestName3", CreatedBy = "TestName4" });
            _roleRepository.Insert(new Role { RoleName = "TestName5", CreatedBy = "TestName6" });

            IEnumerable<Role> roles = _roleRepository.GetAll();
            Role[] list = roles.ToArray();
            int[] ids = new int[roles.Count()];

            for (int i = 0; i < roles.Count(); i++)
            {
                ids[i] = list[i].RoleId;
            }
            IEnumerable<Role> response = _roleManager.GetRoles(
                new Model.DTO.Filters.RoleFilterDTO { RoleId = ids });

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Count() is 3);
        }

        [Test]
        public void CannotGetRoleByInvalidRoleId()
        {
            Role role = new Role { RoleName = "TestName", CreatedBy = "TestName2" };
            try
            {
                Role response = _roleManager.GetRole(role.RoleId + 1);
            } catch(GettingByInvalidIdException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CannotDeleteRoleWithEmployeeRole()
        {
            Role role = _roleRepository.Insert(new Role { RoleName = "test" , CreatedBy = "HR"});
            _employeeRoleRepository.Insert(new EmployeeRole
            {
                EmployeeId = 1,
                RoleId = role.RoleId,
                CreatedBy = "HR"
            });

            try
            {
                _roleManager.DeleteRole(role.RoleId);
            } catch(DeletingEntityWithExistingAttachmentsException)
            {
                Assert.Pass();
            }
            Assert.Fail(); 
        }

        [Test]
        public void CanUpdateRole()
        {
            Role role = _roleRepository.Insert(new Role { RoleName = "test", CreatedBy = "test8" });
            string target = "Success";
            role.RoleName = target;
            _roleManager.UpdateRole(role);
            Assert.True(_roleRepository.GetBy(role.RoleId).RoleName.Equals(target));
        }
    }
}