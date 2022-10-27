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
    public class EmployeeCertificateTests : BaseServiceTest
    {

        IContainer _container;
        IEmployeeCertificateManager _employeeCertificateManager;
        IRepository<Certificate> _certificateRepository;
        IRepository<EmployeeCertificate> _employeeCertificateRepository;
        IRepository<Employee> _employeeRepository;

        protected override void PostSetup(IContainer container)
        {
            // employee certificate objects
            _employeeCertificateRepository = container.Resolve<IRepository<EmployeeCertificate>>();
            _employeeCertificateManager = container.Resolve<IEmployeeCertificateManager>();

            // extra repositories
            _employeeRepository = container.Resolve<IRepository<Employee>>();
            _certificateRepository = container.Resolve<IRepository<Certificate>>();
        }

        [TearDown]
        public void CleanUp()
        {
            IEnumerable<EmployeeCertificate> trash1 = _employeeCertificateRepository.GetAll();
            foreach (EmployeeCertificate empCert in trash1)
                _employeeCertificateRepository.Delete(empCert);

            IEnumerable<Employee> trash2 = _employeeRepository.GetAll();
            foreach (Employee emp in trash2)
                _employeeRepository.Delete(emp);

            IEnumerable<Certificate> trash3 = _certificateRepository.GetAll();
            foreach (Certificate cert in trash3)
                _certificateRepository.Delete(cert);
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

        public Certificate GenericCertificate()
        {
            Certificate response = new Certificate 
            { 
                CertificateName = "participation", 
                CreatedBy = "a fool" 
            };
            return response;
        }
        
        [Test]
        public void CanAssignCertificateToEmployee()
        {
            EmployeeCertificate employeeCertificate = new EmployeeCertificate 
            { 
                EmployeeId = 1, 
                CertificateId = 1, 
                CreatedBy = "Foo"
            };

            EmployeeCertificate returnValue = _employeeCertificateManager.AssignEmployeeCertificate(employeeCertificate);
            EmployeeCertificate response = _employeeCertificateRepository.GetAll().FirstOrDefault();
            Assert.True(response is not null);
        }

        [Test]
        public void CanSetCreatedByOnEmployeeCertificate()
        {
            EmployeeCertificate response = _employeeCertificateManager.AssignEmployeeCertificate
                (new EmployeeCertificate { EmployeeId = 1, CertificateId = 1, CreatedBy = GetTestIdentityContext().UserName });
            Assert.AreEqual(GetTestIdentityContext().UserName, response.CreatedBy);
        }

        [Test]
        public void CanUnassignCertificateFromEmployee()
        {
            EmployeeCertificate employeeCertificate = new EmployeeCertificate { 
                    EmployeeId = 1, CertificateId = 1, CreatedBy = "a second fool" };
            EmployeeCertificate inserted = _employeeCertificateRepository.Insert(employeeCertificate);
            _employeeCertificateManager.UnassignEmployeeCertificate(inserted.EmployeeCertificateId);
            IEnumerable<EmployeeCertificate> employeeCertificates = _employeeCertificateRepository.GetAll();
            Assert.True(employeeCertificates.Count() == 0);
        }

        [Test]
        public void CannotUnassignInvalidEmployeeCertificateFromEmployee()
        {
            EmployeeCertificate employeeCertificate = new EmployeeCertificate { 
                    EmployeeId = 1, CertificateId = 1, CreatedBy = "a third fool" };
            
            EmployeeCertificate response = _employeeCertificateRepository.Insert(employeeCertificate);
            try
            {
                _employeeCertificateManager.UnassignEmployeeCertificate(response.EmployeeCertificateId + 1);
            } catch (DeletingByInvalidIdException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CanGetEmployeeCertificateByEmployeeCertificateId()
        {
            EmployeeCertificate employeeCertificate = new EmployeeCertificate { 
                    EmployeeId = 1, CertificateId = 1, CreatedBy = "a fourth fool" };
            EmployeeCertificate inserted = _employeeCertificateRepository.Insert(employeeCertificate);
            EmployeeCertificate response = _employeeCertificateManager.GetEmployeeCertificate( inserted.EmployeeCertificateId );
            Assert.IsNotNull(response);
        }

        [Test]
        public void CannotGetByInvalidEmployeeCertificateId()
        {
            EmployeeCertificate employeeCertificate = new EmployeeCertificate { 
                    EmployeeId = 1, CertificateId = 1, CreatedBy = "a fifth fool" };
            EmployeeCertificate inserted = _employeeCertificateRepository.Insert(employeeCertificate);
            try
            {
                _employeeCertificateManager.GetEmployeeCertificate(inserted.EmployeeCertificateId + 1);
            } catch(GettingByInvalidIdException)
            {
                Assert.Pass();
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void CannotAssignDuplicateCertificateToEmployee()
        {
            EmployeeCertificate entry1 = new EmployeeCertificate
            {
                EmployeeId = 1,
                CertificateId = 1,
                CreatedBy = "a sixth fool"
            };
            EmployeeCertificate entry2 = new EmployeeCertificate
            {
                EmployeeId = 1,
                CertificateId = 1,
                CreatedBy = "a sixth fool"
            };
            _employeeCertificateRepository.Insert(entry1);
            try
            {
                _employeeCertificateManager.AssignEmployeeCertificate(entry2);
            } catch(Exception)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CanGetEmployeeCertificateByEmployeeIdAndCertificateId()
        {
            EmployeeCertificate entry = _employeeCertificateRepository.Insert(new EmployeeCertificate
            {
                EmployeeId = 1,
                CertificateId = 1,
                CreatedBy = "a seventh fool"
            });

            EmployeeCertificate Response = _employeeCertificateManager.GetEmployeeCertificate(entry.EmployeeId,entry.CertificateId);
            Assert.True(
                Response.EmployeeId == entry.EmployeeId &&
                Response.CertificateId == entry.CertificateId &&
                Response.EmployeeCertificateId == entry.EmployeeCertificateId &&
                Response.CreatedBy == entry.CreatedBy
                );
        }

        [Test]
        public void CanGetMultipleEmployeeCertificatesByCertificateId()
        {
            for (int i = 0; i < 3; i++)
            {
                _employeeRepository.Insert(GenericEmployee());
                _certificateRepository.Insert(GenericCertificate());
            }
            for (int i = 1; i <= 4; i++)
            {
                _employeeCertificateRepository.Insert(new EmployeeCertificate
                {
                    EmployeeId = i,
                    CertificateId = i%2,
                    CreatedBy = "an eighth fool"
                });
            }

            EmployeeCertificateFilterDTO filter = new EmployeeCertificateFilterDTO { CertificateId = new int[] { 1 } };
            IEnumerable<EmployeeCertificate> response = _employeeCertificateManager.GetEmployeeCertificates(filter);
            Assert.True(response.Count() == 2);
        }

        [Test]
        public void CanGetMultipleEmployeeCertificatesByMultipleCertificateIds()
        {
            for (int i = 0; i < 3; i++)
            {
                _employeeRepository.Insert(GenericEmployee());
            }

            _certificateRepository.Insert(GenericCertificate());

            EmployeeCertificate empCert;

            for (int i = 1; i <= 4; i++)
            {
                empCert = _employeeCertificateRepository.Insert(new EmployeeCertificate
                {
                    EmployeeId = i,
                    CertificateId = 1 + i%2,
                    CreatedBy = "an eighth fool"
                });
            }

            EmployeeCertificateFilterDTO filter = new EmployeeCertificateFilterDTO 
            { 
                CertificateId = new int[] { 1,2 } 
            };

            IEnumerable<EmployeeCertificate> response = _employeeCertificateManager.GetEmployeeCertificates(filter);
            int filterCount = filter.CertificateId.Count();
            int count = response.Count();
            var all = _employeeCertificateRepository.GetAll();
            var certificates = _certificateRepository.GetAll();

            if(count != filterCount)
            {
                Assert.Fail();
            }
            Assert.True(response.Count() == filterCount);
            
        }

        [Test]
        public void CanGetMultipleEmployeeCertificatesByMultipleEmployeeIds()
        {
            for (int i = 0; i < 3; i++)
                _employeeRepository.Insert(GenericEmployee());
            for (int i = 1; i <= 4; i++)
            {
                _employeeCertificateRepository.Insert(new EmployeeCertificate
                {
                    EmployeeId = i%2+27,
                    CertificateId = 1,
                    CreatedBy = "a fool"
                });
            }
            _certificateRepository.Insert(GenericCertificate());
            _certificateRepository.Insert(GenericCertificate());
            _employeeCertificateRepository.Insert(new EmployeeCertificate { EmployeeId = 26, CertificateId = 2, CreatedBy = "debug" });
            _employeeCertificateRepository.Insert(new EmployeeCertificate { EmployeeId = 29, CertificateId = 3, CreatedBy = "debug" });

            EmployeeCertificateFilterDTO filter = new EmployeeCertificateFilterDTO { EmployeeId = new int[] { 27, 28 } };
            IEnumerable<EmployeeCertificate> response = _employeeCertificateManager.GetEmployeeCertificates(filter);
            int count = response.Count();
            
            if (response.Any(row => row.CertificateId == 2))
                Assert.Fail("miss by -1");
            if (response.Any(row => row.CertificateId == 3))
                Assert.Fail("miss by +1");

            if (response.Count() == 4)
                Assert.Pass();
            else
                Assert.Fail("got " + response.Count() + " responses");
        }

        [Test]
        public void CanGetMultipleCertificatesByCertificateName()
        {
            _employeeRepository.Insert(GenericEmployee());
            IEnumerable<Employee> employees = _employeeRepository.GetAll();
            _certificateRepository.Insert(GenericCertificate());
            Certificate target = _certificateRepository.Insert(new Certificate { CertificateName = "target", CreatedBy = "a debugger"});

            IEnumerable<Certificate> certificates = _certificateRepository.GetAll();
            foreach (Certificate cert in certificates)
                for (int i = 1; i < 4; i++)
                    foreach (Employee employee in employees)
                        _employeeCertificateRepository.Insert(
                            new EmployeeCertificate { 
                                EmployeeId = employee.EmployeeId,
                                CertificateId = cert.CertificateId, 
                                CreatedBy = "HR" 
                            });

            EmployeeCertificateFilterDTO filter = new EmployeeCertificateFilterDTO { CertificateName = new string[] { "target" } };
            IEnumerable<EmployeeCertificate> response = _employeeCertificateManager.GetEmployeeCertificates(filter);
            if (response.All(row => row.CertificateId == target.CertificateId))
                Assert.Pass();
            else
                Assert.Fail("got " + response.Where(row => row.CertificateId != target.CertificateId).Count() + " incorrect responses");
        }


        public Employee GenericEmployee2()
        {
            Employee response = new Employee
            {
                FirstName = "Jane",
                LastName = "Doe",
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
            Certificate cert1 = _certificateRepository.Insert(GenericCertificate());
            
            Certificate target = GenericCertificate();
            target.CertificateName = "target";

            Certificate cert2 = _certificateRepository.Insert(target);
            Certificate cert3 = _certificateRepository.Insert(GenericCertificate());
            Employee emp1 = _employeeRepository.Insert(GenericEmployee());    
            Employee emp2 = _employeeRepository.Insert(GenericEmployee2());
            Employee emp3 = _employeeRepository.Insert(GenericEmployee3());
            _employeeCertificateRepository.Insert(new EmployeeCertificate { EmployeeId = emp1.EmployeeId
                    , CertificateId = cert1.CertificateId, CreatedBy = "debug" });
            _employeeCertificateRepository.Insert(new EmployeeCertificate { EmployeeId = emp1.EmployeeId
                    , CertificateId = cert2.CertificateId, CreatedBy = "debug" });
            _employeeCertificateRepository.Insert(new EmployeeCertificate { EmployeeId = emp3.EmployeeId
                    , CertificateId = cert3.CertificateId, CreatedBy = "debug" });

            EmployeeCertificateFilterDTO filter = new EmployeeCertificateFilterDTO { CertificateName = new string[] { "target"}
            , CertificateId = new int[] {cert2.CertificateId,cert3.CertificateId}
            , EmployeeId = new int[] {emp1.EmployeeId}
            };
            IEnumerable<EmployeeCertificate> response = _employeeCertificateManager.GetEmployeeCertificates(filter);

            if (response.Count() == 1)
                Assert.Pass();
            Assert.Fail(response.Count().ToString());
        }

        [Test]
        public void CanUpdateEmployeeCertificate()
        {
            EmployeeCertificate empcertificate = _employeeCertificateRepository.Insert(
                new EmployeeCertificate { CertificateId = 1, EmployeeId = 1, CreatedBy = "test8" });
            String target = "Success";
            empcertificate.CreatedBy = target;
            EmployeeCertificate cert = _employeeCertificateManager.UpdateEmployeeCertificate(empcertificate);
            Assert.True(_employeeCertificateRepository.GetBy(empcertificate.EmployeeCertificateId).CreatedBy.Equals(target));
        }
    }
}