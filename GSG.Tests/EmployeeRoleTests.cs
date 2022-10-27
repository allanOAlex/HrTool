using Autofac;
using GSG.Model;
using GSG.Repository.Capability;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using GSG.Model.DTO.Filters;
using GSG.Shared.Exceptions;
using GSG.Service.Interfaces;

namespace GSG.Tests
{
    public class EmployeeRoleTests : BaseServiceTest
    {

        IContainer _container;
        IEmployeeRoleManager _employeeRoleManager;
        IRepository<Role> _roleRepository;
        IRepository<EmployeeRole> _employeeRoleRepository;
        IRepository<Employee> _employeeRepository;

        protected override void PostSetup(IContainer container)
        {
            // employee role objects
            _employeeRoleRepository = container.Resolve<IRepository<EmployeeRole>>();
            _employeeRoleManager = container.Resolve<IEmployeeRoleManager>();

            // extra repositories
            _employeeRepository = container.Resolve<IRepository<Employee>>();
            _roleRepository = container.Resolve<IRepository<Role>>();
        }
        [TearDown]
        public void CleanUp()
        {
            IEnumerable<EmployeeRole> trash1 = _employeeRoleRepository.GetAll();
            foreach (EmployeeRole empCert in trash1)
                _employeeRoleRepository.Delete(empCert);

            IEnumerable<Employee> trash2 = _employeeRepository.GetAll();
            foreach (Employee emp in trash2)
                _employeeRepository.Delete(emp);

            IEnumerable<Role> trash3 = _roleRepository.GetAll();
            foreach (Role cert in trash3)
                _roleRepository.Delete(cert);
        }

        public Employee GenericEmployee()
        {
            Employee response = new Employee
            {
                FirstName = "John",
                LastName = "Smith",
                Address = "Nunya",
                Address2 = "Bee's Knees",
                City = "Mars Vegas",
                Email = "gmail",
                EmployeeState = "Georgia",
                PictureUrl = "image",
                PhoneNumber = "911",
                Zip = "it",
                CreatedBy = "a fool"
            };
            return response;
        }

        public Role GenericRole()
        {
            Role response = new Role { RoleName = "participation", CreatedBy = "a fool" };
            return response;
        }

        [Test]
        public void CanAssignRoleToEmployee()
        {
            EmployeeRole employeeRole = new EmployeeRole
            {
                EmployeeId = 1,
                RoleId = 1,
                CreatedBy = "a fool"
            };
            EmployeeRole returnValue = _employeeRoleManager.AssignEmployeeRole(employeeRole);
            EmployeeRole response = _employeeRoleRepository.GetAll().FirstOrDefault();
            Assert.True(response is not null);
        }

        [Test]
        public void CanSetCreatedByOnEmployeeRole()
        {
            EmployeeRole employeeRole = new EmployeeRole
            {
                EmployeeId = 1,
                RoleId = 1,
                CreatedBy = GetTestIdentityContext().UserName
            };
            EmployeeRole returnValue = _employeeRoleManager.AssignEmployeeRole(employeeRole);
            EmployeeRole response = _employeeRoleRepository.GetAll().FirstOrDefault();
            Assert.AreEqual(GetTestIdentityContext().UserName, response.CreatedBy);
        }

        [Test]
        public void CanUnassignRoleFromEmployee()
        {
            EmployeeRole employeeRole = new EmployeeRole
            {
                EmployeeId = 1,
                RoleId = 1,
                CreatedBy = "a second fool"
            };
            EmployeeRole inserted = _employeeRoleRepository.Insert(employeeRole);
            _employeeRoleManager.UnassignEmployeeRole(inserted.EmployeeRoleId);
            IEnumerable<EmployeeRole> employeeRoles = _employeeRoleRepository.GetAll();
            Assert.True(employeeRoles.Count() == 0);
        }
        [Test]
        public void CannotUnassignInvalidEmployeeRoleFromEmployee()
        {
            EmployeeRole employeeRole = new EmployeeRole
            {
                EmployeeId = 1,
                RoleId = 1,
                CreatedBy = "a third fool"
            };

            EmployeeRole response = _employeeRoleRepository.Insert(employeeRole);
            try
            {
                _employeeRoleManager.UnassignEmployeeRole(response.EmployeeRoleId + 1);
            }
            catch (DeletingByInvalidIdException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CanGetEmployeeRoleByEmployeeRoleId()
        {
            EmployeeRole employeeRole = new EmployeeRole
            {
                EmployeeId = 1,
                RoleId = 1,
                CreatedBy = "a fourth fool"
            };
            EmployeeRole inserted = _employeeRoleRepository.Insert(employeeRole);
            EmployeeRole response = _employeeRoleManager.GetEmployeeRole(inserted.EmployeeRoleId);
            Assert.IsNotNull(response);
        }

        [Test]
        public void CannotGetByInvalidEmployeeRoleId()
        {
            EmployeeRole employeeRole = new EmployeeRole
            {
                EmployeeId = 1,
                RoleId = 1,
                CreatedBy = "a fifth fool"
            };
            EmployeeRole inserted = _employeeRoleRepository.Insert(employeeRole);
            try
            {
                _employeeRoleManager.GetEmployeeRole(inserted.EmployeeRoleId + 1);
            }
            catch (GettingByInvalidIdException)
            {
                Assert.Pass();
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void CannotAssignDuplicateRoleToEmployee()
        {
            EmployeeRole entry1 = new EmployeeRole
            {
                EmployeeId = 1,
                RoleId = 1,
                CreatedBy = "a sixth fool"
            };
            EmployeeRole entry2 = new EmployeeRole
            {
                EmployeeId = 1,
                RoleId = 1,
                CreatedBy = "a sixth fool"
            };
            _employeeRoleRepository.Insert(entry1);
            try
            {
                _employeeRoleManager.AssignEmployeeRole(entry2);
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
        [Test]
        public void CanGetEmployeeRoleByEmployeeIdRoleIdandEmployerId()
        {
            EmployeeRole entry = _employeeRoleRepository.Insert(new EmployeeRole
            {
                EmployeeId = 1,
                RoleId = 1,
                EmployerId = 1,
                CreatedBy = "a seventh fool"
            });

            EmployeeRole Response = _employeeRoleManager.GetEmployeeRole(entry.EmployeeId, entry.RoleId);
            Assert.True(
                Response.EmployeeId == entry.EmployeeId &&
                Response.RoleId == entry.RoleId &&
                Response.EmployeeRoleId == entry.EmployeeRoleId &&
                Response.CreatedBy == entry.CreatedBy
                );
        }

        [Test]
        public void CanGetMultipleEmployeeRolesByRoleId()
        {
            for (int i = 0; i < 3; i++)
            {
                _employeeRepository.Insert(GenericEmployee());
                _roleRepository.Insert(GenericRole());
            }
            for (int i = 1; i <= 4; i++)
            {
                _employeeRoleRepository.Insert(new EmployeeRole
                {
                    EmployeeId = i,
                    RoleId = i % 2,
                    CreatedBy = "an eighth fool"
                });
            }

            EmployeeRoleFilterDTO filter = new EmployeeRoleFilterDTO { RoleId = new int[] { 1 } };
            IEnumerable<EmployeeRole> response = _employeeRoleManager.GetEmployeeRoles(filter);
            Assert.True(response.Count() == 2);
        }

        [Test]
        public void CanGetMultipleEmployeeRolesByMultipleRoleIds()
        {
            for (int i = 0; i < 3; i++)
            {
                _employeeRepository.Insert(GenericEmployee());
            }
            _roleRepository.Insert(GenericRole());
            _roleRepository.Insert(GenericRole());
            for (int i = 1; i <= 4; i++)
            {
                _employeeRoleRepository.Insert(new EmployeeRole
                {
                    EmployeeId = i,
                    RoleId = 1 + i % 2,
                    CreatedBy = "an eighth fool"
                });
            }

            EmployeeRoleFilterDTO filter = new EmployeeRoleFilterDTO { RoleId = new int[] { 1, 2 } };
            IEnumerable<EmployeeRole> response = _employeeRoleManager.GetEmployeeRoles(filter);
            int count = response.Count();
            var all = _employeeRoleRepository.GetAll();
            var roles = _roleRepository.GetAll();
            if (count is not 4)
            {
                Assert.Fail();
            }
            Assert.True(response.Count() == 4);

        }

        [Test]
        public void CanGetMultipleEmployeeRolesByMultipleEmployeeIds()
        {
            for (int i = 0; i < 3; i++)
                _employeeRepository.Insert(GenericEmployee());
            for (int i = 1; i <= 4; i++)
            {
                _employeeRoleRepository.Insert(new EmployeeRole
                {
                    EmployeeId = i % 2 + 27,
                    RoleId = 1,
                    CreatedBy = "a fool"
                });
            }
            _roleRepository.Insert(GenericRole());
            _roleRepository.Insert(GenericRole());
            _employeeRoleRepository.Insert(new EmployeeRole { EmployeeId = 26, RoleId = 2, CreatedBy = "debug" });
            _employeeRoleRepository.Insert(new EmployeeRole { EmployeeId = 29, RoleId = 3, CreatedBy = "debug" });

            EmployeeRoleFilterDTO filter = new EmployeeRoleFilterDTO { EmployeeId = new int[] { 27, 28 } };
            IEnumerable<EmployeeRole> response = _employeeRoleManager.GetEmployeeRoles(filter);
            int count = response.Count();

            if (response.Any(row => row.RoleId == 2))
                Assert.Fail("miss by -1");
            if (response.Any(row => row.RoleId == 3))
                Assert.Fail("miss by +1");

            if (response.Count() == 4)
                Assert.Pass();
            else
                Assert.Fail("got " + response.Count() + " responses");
        }
        [Test]
        public void CanGetMultipleRolesByRoleName()
        {
            _employeeRepository.Insert(GenericEmployee());
            IEnumerable<Employee> employees = _employeeRepository.GetAll();
            _roleRepository.Insert(GenericRole());
            Role target = _roleRepository.Insert(new Role { RoleName = "target", CreatedBy = "a debugger" });

            IEnumerable<Role> roles = _roleRepository.GetAll();
            foreach (Role cert in roles)
                for (int i = 1; i < 4; i++)
                    foreach (Employee employee in employees)
                        _employeeRoleRepository.Insert(
                            new EmployeeRole
                            {
                                EmployeeId = employee.EmployeeId,
                                RoleId = cert.RoleId,
                                CreatedBy = "HR"
                            });

            EmployeeRoleFilterDTO filter = new EmployeeRoleFilterDTO { RoleName = new string[] { "target" } };
            IEnumerable<EmployeeRole> response = _employeeRoleManager.GetEmployeeRoles(filter);
            if (response.All(row => row.RoleId == target.RoleId))
                Assert.Pass();
            else
                Assert.Fail("got " + response.Where(row => row.RoleId != target.RoleId).Count() + " incorrect responses");
        }


        public Employee GenericEmployee2()
        {
            Employee response = new Employee
            {
                FirstName = "George",
                LastName = "Brown",
                Address = "Nunya",
                Address2 = "Bee's Knees",
                City = "Mars Vegas",
                Email = "gmail",
                EmployeeState = "Texas",
                PictureUrl = "image",
                PhoneNumber = "911",
                Zip = "it",
                CreatedBy = "a fool"
            };
            return response;
        }
        public Employee GenericEmployee3()
        {
            Employee response = new Employee
            {
                FirstName = "Charles",
                LastName = "Schwab",
                Address = "Nunya",
                Address2 = "Bee's Knees",
                City = "Mars Vegas",
                Email = "gmail",
                EmployeeState = "Florida",
                PictureUrl = "image",
                PhoneNumber = "911",
                Zip = "it",
                CreatedBy = "a fool"
            };
            return response;
        }
        [Test]
        public void CanFilterMultipleParameters()
        {
            Role cert1 = _roleRepository.Insert(GenericRole());

            Role target = GenericRole();
            target.RoleName = "target";

            Role cert2 = _roleRepository.Insert(target);
            Role cert3 = _roleRepository.Insert(GenericRole());
            Employee emp1 = _employeeRepository.Insert(GenericEmployee());
            Employee emp2 = _employeeRepository.Insert(GenericEmployee2());
            Employee emp3 = _employeeRepository.Insert(GenericEmployee3());
            _employeeRoleRepository.Insert(new EmployeeRole
            {
                EmployeeId = emp1.EmployeeId
                    ,
                RoleId = cert1.RoleId,
                CreatedBy = "debug"
            });
            _employeeRoleRepository.Insert(new EmployeeRole
            {
                EmployeeId = emp1.EmployeeId
                    ,
                RoleId = cert2.RoleId,
                CreatedBy = "debug"
            });
            _employeeRoleRepository.Insert(new EmployeeRole
            {
                EmployeeId = emp3.EmployeeId
                    ,
                RoleId = cert3.RoleId,
                CreatedBy = "debug"
            });

            EmployeeRoleFilterDTO filter = new EmployeeRoleFilterDTO
            {
                RoleName = new string[] { "target" }
            ,
                RoleId = new int[] { 2, 3 }
            ,
                EmployeeId = new int[] { 1 }
            };
            IEnumerable<EmployeeRole> response = _employeeRoleManager.GetEmployeeRoles(filter);

            if (response.Count() == 1)
                Assert.Pass();
            Assert.Fail();
        }

        [Test]
        public void CanUpdateEmployeeRole()
        {
            EmployeeRole employeeRole = _employeeRoleRepository.Insert(new EmployeeRole { RoleId = 1, EmployeeId = 1, CreatedBy = "test8" });
            String target = "Success";
            employeeRole.CreatedBy = target;
            _employeeRoleManager.UpdateEmployeeRole(employeeRole);
            Assert.True(_employeeRoleRepository.GetBy(employeeRole.EmployeeRoleId).CreatedBy.Equals(target));
        }
    }
}
