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
    public class EmployeeTests : BaseServiceTest
    {
        IContainer _container;
        IEmployeeManager _employeeManager;
        IRepository<Employee> _employeeRepository;

        protected override void PostSetup(IContainer container)
        {
            _employeeRepository = container.Resolve<IRepository<Employee>>();
            _employeeManager = container.Resolve<IEmployeeManager>();
        }

        [TearDown]
        public void CleanUp()
        {
            IEnumerable<Employee> trash = _employeeRepository.GetAll();
            foreach (Employee emp in trash)
                _employeeRepository.Delete(emp);
        }

        public Employee GenericEmployee()
        {
            Employee response = new Employee
            {
                Address = "Holy",
                Address2 = "Guacamole",
                City = "Mars Vegas",
                Created = DateOnly.FromDateTime(DateTime.Now),
                CreatedBy = "a fool",
                Email = "Wrong.Number@hotmail.org",
                EmployeeState = "Ga",
                FirstName = "John",
                LastName = "Smith",
                PhoneNumber = "911",
                PictureUrl = "images.google.com",
                RowVer = new Guid(),
                Updated = DateTime.Now,
                Zip = "404"
            };
            return response;
        }

        [Test]
        public void CanCreateEmployee()
        {
            Employee employee = GenericEmployee();
            _employeeManager.CreateEmployee(employee);
            IEnumerable<Employee> all = _employeeRepository.GetAll();
            Assert.AreEqual(1,all.Count());
        }

        [Test]
        public void CanSetCreatedByOnCreateCertificate()
        {
            Employee employee = GenericEmployee();
            employee.CreatedBy = GetTestIdentityContext().UserName;
            Employee response = _employeeManager.CreateEmployee(employee);

            Assert.AreEqual(GetTestIdentityContext().UserName, response.CreatedBy);
        }

        [Test]
        public void CanDeleteEmployee()
        {
            Employee inserted = _employeeRepository.Insert(GenericEmployee());
            _employeeRepository.Insert(GenericEmployee());
            Assert.IsTrue(_employeeManager.DeleteEmployee(inserted.EmployeeId));
        }

        [Test]
        public void CanGetAllEmployees()
        {
            for (int i = 0; i < 10; i++)
                _employeeRepository.Insert(GenericEmployee());
            IEnumerable<Employee> all = _employeeManager.GetEmployees(null);
            Assert.AreEqual(10,all.Count());
        }

        [Test]
        public void CanGetEmployeeByEmployeeId()
        {
            Employee employee = _employeeRepository.Insert(GenericEmployee());
            Employee response = _employeeManager.GetEmployee(employee.EmployeeId);
            Assert.NotNull(response);
        }

        [Test]
        public void CanGetMultipleEmployeesByEmployeeId()
        {
            Employee temp = GenericEmployee();
            for (int i = 0; i < 10; i++)
                temp = _employeeRepository.Insert(GenericEmployee());
            int target = temp.EmployeeId - 5;
            EmployeeFilterDTO filter = new EmployeeFilterDTO { EmployeeId = new int[] { target,target+1,target+2 } };
            
            IEnumerable<Employee> response = _employeeManager.GetEmployees(filter);
            Assert.AreEqual(3, response.Count());
        }

        [Test]
        public void CanGetEmployeeByLastname()
        {
            _employeeRepository.Insert(GenericEmployee());

            var employee = _employeeManager.GetEmployees(new Model.DTO.Filters.EmployeeFilterDTO
            {
                LastName = new string[] { "Smith" }
            });
            Assert.IsTrue(employee is not null);
        }

        [Test]
        public void CanGetMultipleEmployeesByEmployeeState()
        {
            for(int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                {
                    var temp = GenericEmployee();
                    temp.EmployeeState = "Kansas";
                    _employeeRepository.Insert(temp);
                }
                else
                    _employeeRepository.Insert(GenericEmployee());
            }
            EmployeeFilterDTO filter = new EmployeeFilterDTO { States = new string[] { "Kansas" } };
            IEnumerable<Employee> response = _employeeManager.GetEmployees(filter);
            Assert.AreEqual(5, response.Count());
        }

        [Test]
        public void CannotDeleteEmployeeWithInvalidId()
        {
            try
            {
                _employeeManager.DeleteEmployee(60);
            } catch(DeletingByInvalidIdException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CannotGetEmployeeWithInvalidId()
        {
            Employee inserted = GenericEmployee();
            try 
            {
                _employeeManager.GetEmployee(inserted.EmployeeId + 1);
            } catch(GettingByInvalidIdException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CannotGetEmployeeWithInvalidLastname()
        {
            _employeeRepository.Insert(GenericEmployee());
            IEnumerable<Employee> employee = _employeeManager.GetEmployees(new Model.DTO.Filters.EmployeeFilterDTO
            {
                LastName = new string[] { "Doe" }
            });

            Assert.AreEqual(0,employee.Count());
        }

        [Test]
        public void CannotGetEmployeeWithInvalidState()
        {
            for (int i = 0; i < 10; i++)
            {
                if (i % 2 == 0)
                {
                    var temp = GenericEmployee();
                    temp.EmployeeState = "Kansas";
                    _employeeRepository.Insert(temp);
                }
                else
                    _employeeRepository.Insert(GenericEmployee());
            }
            EmployeeFilterDTO filter = new EmployeeFilterDTO { States = new string[] { "Nebraska" } };
            IEnumerable<Employee> response = _employeeManager.GetEmployees(filter);
            Assert.AreEqual(0, response.Count());
        }

        [Test]
        public void CannotCreateDuplicateEmployees()
        {
            _employeeRepository.Insert(GenericEmployee());
            try
            {
                _employeeManager.CreateEmployee(GenericEmployee());
                IEnumerable<Employee> employees = _employeeManager.GetEmployees(null);
            } catch(CreatingDuplicateException)
            {
                Assert.Pass();
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void CanFilterByMultipleParameters()
        {
            Employee emp1 = _employeeRepository.Insert(new Employee
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
            });
            Employee emp2 = _employeeRepository.Insert(new Employee
            {
                FirstName = "Jane",
                LastName = "Smith",
                Address = "Nunya",
                Address2 = "Bee's Knees",
                City = "Mars Vegas",
                Email = "gmail",
                EmployeeState = "Texas",
                PictureUrl = "image",
                PhoneNumber = "911",
                Zip = "it",
                CreatedBy = "a fool"
            });
            Employee emp3 = _employeeRepository.Insert(new Employee
            {
                FirstName = "Craig",
                LastName = "Jones",
                Address = "Nunya",
                Address2 = "Bee's Knees",
                City = "Mars Vegas",
                Email = "gmail",
                EmployeeState = "Georgia",
                PictureUrl = "image",
                PhoneNumber = "911",
                Zip = "it",
                CreatedBy = "a fool"
            });
            Employee emp4 = _employeeRepository.Insert(new Employee
            {
                FirstName = "John",
                LastName = "Dun",
                Address = "Nunya",
                Address2 = "Bee's Knees",
                City = "Mars Vegas",
                Email = "gmail",
                EmployeeState = "Georgia",
                PictureUrl = "image",
                PhoneNumber = "911",
                Zip = "it",
                CreatedBy = "a fool"
            });

            EmployeeFilterDTO filter = new EmployeeFilterDTO
            {
                LastName = new string[] { "Smith", "Jones" }
                ,
                States = new string[] { "Georgia" }
            };

            IEnumerable<Employee> employees = _employeeManager.GetEmployees(filter);
            Assert.AreEqual(2, employees.Count());
        }

        [Test]
        public void CanUpdateEmployeeRole()
        {
            Employee employee = _employeeRepository.Insert(GenericEmployee());
            String target = "Success";
            employee.FirstName = target;
            _employeeManager.UpdateEmployee(employee);
            Assert.True(_employeeRepository.GetBy(employee.EmployeeId).FirstName.Equals(target));
        }
    }
}
