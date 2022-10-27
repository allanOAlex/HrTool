using Autofac;
using GSG.Model;
using GSG.Repository.Capability;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using GSG.Service.Interfaces;
using GSG.Shared.Exceptions;

namespace GSG.Tests
{
    public class EmployerTests : BaseServiceTest
    {
        IContainer _container;
        IEmployerManager _employerManager;
        IRepository<Employer> _employerRepository;
        IRepository<EmployeeRole> _employeeRoleRepository;
        IRepository<Employee> _employeeRepository;
        IRepository<Role> _roleRepository;

        protected override void PostSetup(IContainer container) 
        {
            _employerRepository = container.Resolve<IRepository<Employer>>();
            _employerManager = container.Resolve < IEmployerManager>();
            _employeeRoleRepository = container.Resolve<IRepository<EmployeeRole>>();
            _employeeRepository = container.Resolve<IRepository<Employee>>();
            _roleRepository = container.Resolve<IRepository<Role>>();
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

        [TearDown]
        public void CleanUp()
        {
            IEnumerable<Employer> trash = _employerRepository.GetAll();
            foreach (Employer emp in trash)
                _employerRepository.Delete(emp);

            IEnumerable<Employee> trash2 = _employeeRepository.GetAll();
            foreach (Employee emp in trash2)
                _employeeRepository.Delete(emp);

            IEnumerable<EmployeeRole> trash3 = _employeeRoleRepository.GetAll();
            foreach(EmployeeRole role in trash3)
                    _employeeRoleRepository.Delete(role);

            IEnumerable<Role> trash4 = _roleRepository.GetAll();
            foreach(Role role in trash4)
                _roleRepository.Delete(role);

        }

        [Test]
        public void CanCreateEmployer()
        {
            Employer response = _employerManager.CreateEmployer(new Employer { EmployerName = "testName", CreatedBy = "testName2" });
            Assert.True(response is not null);
        }

        [Test]
        public void CanSetCreatedByOnCreateCertificate()
        {
            Employer response = _employerManager.CreateEmployer(
                new Employer { EmployerName = "test7", CreatedBy = GetTestIdentityContext().UserName });

            Assert.AreEqual(GetTestIdentityContext().UserName, response.CreatedBy);
        }

        [Test]
        public void CanDeleteEmployer()
        {
            Employer employer = new Employer { EmployerName = "testName", CreatedBy = "testName2" };
            Employer inserted = _employerRepository.Insert(employer);
            bool exists = _employerManager.DeleteEmployer(inserted.EmployerId);
            Employer test = _employerRepository.GetBy(inserted.EmployerId);
            Assert.IsNull(test);
            Assert.IsTrue(exists);
        }

        [Test]
        public void CanGetAllEmployers()
        {
            _employerRepository.Insert(new Employer { EmployerName = "testName", CreatedBy = "testName2" });
            _employerRepository.Insert(new Employer { EmployerName = "testName3", CreatedBy = "testName4" });

            IEnumerable<Employer> employers = _employerManager.GetEmployers(null);
            Assert.AreEqual(2, employers.Count());
        }

        [Test]
        public void CanGetEmployerByEmployerId ()
        {
            Employer employer = _employerRepository.Insert(new Employer { EmployerName = "testName", CreatedBy = "testName2" });
            Employee employee = _employeeRepository.Insert(GenericEmployee());
            Role role = _roleRepository.Insert(new Role { RoleName = "Tester", CreatedBy = "HR" });
            EmployeeRole employeeRole = _employeeRoleRepository.Insert(new EmployeeRole
            {
                EmployeeId = employee.EmployeeId,
                RoleId = role.RoleId,
                CreatedBy = "HR",
                StartDate = DateTime.Now
            });

            Employer response = _employerManager.GetEmployer(employer.EmployerId);
            Assert.True(response is not null);
        }

        [Test]
        public void CanGetEmployersByEmployerName()
        {
            _employerRepository.Insert(new Employer { EmployerName = "testName", CreatedBy = "testName2" });

            IEnumerable<Employer> employers = _employerManager.GetEmployers(new Model.DTO.Filters.EmployerFilterDTO
            {
                EmployerName = new List<string> { "testName" }.ToArray()
            });
            Assert.IsTrue(employers.Count() != 0);
            Assert.AreEqual(1, employers.Count());
        }

        [Test]
        public void CanGetMultipleEmployersById()
        {
            _employerRepository.Insert(new Employer { EmployerName = "testName", CreatedBy = "testName2" });
            _employerRepository.Insert(new Employer { EmployerName = "testName3", CreatedBy = "testName4" });
            _employerRepository.Insert(new Employer { EmployerName = "testName5", CreatedBy = "testName6" });
            _employerRepository.Insert(new Employer { EmployerName = "testName7", CreatedBy = "testName8" });

            IEnumerable<Employer> employer = _employerRepository.GetAll();
            Employer[] list = employer.ToArray();
            int[] ids = new int[list.Count()];

            for (int i = 0; i < employer.Count(); i++)
            {
                ids[i] = list[i].EmployerId;
            }
            IEnumerable<Employer> response = _employerManager.GetEmployers(
                new Model.DTO.Filters.EmployerFilterDTO { EmployerId = ids });

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Count() is 4);
        }

        [Test]
        public void CannotDeleteEmployerWithInvalidId()
        {
            Employer inserted = _employerRepository.Insert(new Employer { EmployerName = "test", CreatedBy = "test6" });
            Employee employee = _employeeRepository.Insert(GenericEmployee());

            try
            {
                _employerManager.DeleteEmployer(inserted.EmployerId + 1);
            }
            catch (DeletingByInvalidIdException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CannotDeleteEmployerWithEmployeeRolesAttached()
        {
            Role role = _roleRepository.Insert(new Role { RoleName = "test", CreatedBy = "test8" });
            Employee employee = _employeeRepository.Insert(GenericEmployee());
            Employer employer = _employerRepository.Insert(new Employer { EmployerName = "GSG", CreatedBy = "HR" });
            EmployeeRole inserted = _employeeRoleRepository.Insert(new EmployeeRole
            {
                RoleId = role.RoleId,
                EmployeeId = employee.EmployeeId,
                CreatedBy = "test 8",
                EmployerId = employer.EmployerId
            });
            

            try
            {
                _employerManager.DeleteEmployer(employer.EmployerId);
            }
            catch (DeletingEntityWithExistingAttachmentsException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CannotGetEmployerByInvalidEmployerId()
        {
            Employer inserted = new Employer { EmployerName = "test", CreatedBy = "test7" };
            Employee employee = _employeeRepository.Insert(GenericEmployee());
            
            try
            {
                _employerManager.GetEmployer(341);
            }
            catch (GettingByInvalidIdException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CannotInsertDuplicateEmployerNames() 
        {
            _employerRepository.Insert(new Employer { EmployerName = "testName", CreatedBy = "testName2" });
            try 
            {
                _employerManager.CreateEmployer(new Employer { EmployerName = "testName", CreatedBy = "testName2" });
            }
            catch(CreatingDuplicateException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CanUpdateEmployer()
        {
            Employer employer = _employerRepository.Insert(new Employer { EmployerName = "test", CreatedBy = "test8" });
            String target = "Success";
            employer.EmployerName = target;
            _employerManager.UpdateEmployer(employer);
            Assert.True(_employerRepository.GetBy(employer.EmployerId).EmployerName.Equals(target));
        }
    }
}
