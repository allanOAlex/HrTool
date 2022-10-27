using Autofac;
using GSG.Model;
using GSG.Repository.Capability;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using GSG.Shared.Exceptions;
using GSG.Service.Interfaces;

namespace GSG.Tests
{
    public class CertificateTests : BaseServiceTest
    {

        IContainer _container;
        ICertificateManager _certificateManager;
        IRepository<Certificate> _certificateRepository;
        IRepository<EmployeeCertificate> _employeeCertificateRepository;
        IRepository<Employee> _employeeRepository;

        protected override void PostSetup(IContainer container)
        {
            _certificateRepository = container.Resolve<IRepository<Certificate>>();
            _certificateManager = container.Resolve<ICertificateManager>();
            _employeeCertificateRepository = container.Resolve<IRepository<EmployeeCertificate>>();
            _employeeRepository = container.Resolve<IRepository<Employee>>();
        }

        [TearDown]
        public void CleanUp()
        {
            IEnumerable<Certificate> trash = _certificateRepository.GetAll();
            foreach (Certificate cert in trash)
                _certificateRepository.Delete(cert);

            IEnumerable<EmployeeCertificate> trash2 = _employeeCertificateRepository.GetAll();
            foreach (EmployeeCertificate employeeCertificate in trash2)
                _employeeCertificateRepository.Delete(employeeCertificate);

            IEnumerable<Employee> trash3 = _employeeRepository.GetAll();
            foreach (Employee employee in trash3)
                _employeeRepository.Delete(employee);
        }

        public static Employee GenericEmployee()
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

        [Test]
        public void CanCreateCertificate()
        {
            Certificate response = _certificateManager.CreateCertificate(new Certificate { CertificateName = "test7", CreatedBy = "6" });
            
            //Assert.True(response is not null);
            Assert.AreEqual(GetTestIdentityContext().UserName,response.CreatedBy);
        }
        
        [Test]
        public void CanSetCreatedByOnCreateCertificate()
        {
            Certificate response = _certificateManager.CreateCertificate(new Certificate { CertificateName = "test7", CreatedBy = "6" });
            
            Assert.AreEqual(GetTestIdentityContext().UserName,response.CreatedBy);
        }

        [Test]
        public void CanDeleteCertificate()
        {
            Certificate inserted = _certificateRepository.Insert(new Certificate { CertificateName = "test", CreatedBy = "test5" });
            bool exists = _certificateManager.DeleteCertificate(inserted.CertificateId);
            Certificate test = _certificateRepository.GetBy(inserted.CertificateId);
            Assert.IsNull(test);
            Assert.IsTrue(exists);
        }

        [Test]
        public void CannotDeleteCertificateWithInvalidId()
        {
            Certificate inserted = _certificateRepository.Insert(new Certificate { CertificateName = "test", CreatedBy = "test6" });
            Employee employee = _employeeRepository.Insert(GenericEmployee());
            _employeeCertificateRepository.Insert(new EmployeeCertificate { CertificateId = inserted.CertificateId, EmployeeId = employee.EmployeeId, CreatedBy = "HR" });

            try
            {
                _certificateManager.DeleteCertificate(inserted.CertificateId + 1);
            }
            catch (DeletingByInvalidIdException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CanGetAllCertificates()
        {
            _certificateRepository.Insert(new Certificate { CertificateName = "test1", CreatedBy = "test2" });
            _certificateRepository.Insert(new Certificate { CertificateName = "test2", CreatedBy = "test2" });

            IEnumerable<Certificate> certificates = _certificateManager.GetCertificates();
            Assert.AreEqual(2, certificates.Count());
        }

        [Test]
        public void CanGetCertificateByCertificateId()
        {
            String target = "gotcha";
            _certificateRepository.Insert(new Certificate { CertificateName = "test1", CreatedBy = "test2"});
            Certificate targetCertificate = _certificateRepository.Insert(new Certificate { CertificateName = target, CreatedBy = "test2" });
            _certificateRepository.Insert(new Certificate { CertificateName = "test1", CreatedBy = "test2" });

            Employee employee = _employeeRepository.Insert(GenericEmployee());
            EmployeeCertificate employeeCertificate = _employeeCertificateRepository.Insert(new EmployeeCertificate
            { EmployeeId = employee.EmployeeId, CertificateId = targetCertificate.CertificateId, CreatedBy = "HR" });

            Certificate response = _certificateManager.GetCertificate(targetCertificate.CertificateId);
            Assert.True(response is not null);
        }

        [Test]
        public void CanGetCertificateByCertificateName()
        {
            _certificateRepository.Insert(new Certificate { CertificateName = "test", CreatedBy = "test4" });

            IEnumerable<Certificate> certificates = _certificateManager.GetCertificates(new Model.DTO.Filters.CertificateFilterDTO
            {
                CertificateName = new string[] { "test" }
            });

            Assert.IsTrue(certificates is not null);
            Assert.AreEqual(1, certificates.Count());
        }

        [Test]
        public void CannotInsertDuplicateCertificateNames()
        {
            _certificateRepository.Insert(new Certificate { CertificateName = "test3", CreatedBy = "test3" });
            try
            {
                _certificateManager.CreateCertificate(new Certificate { CertificateName = "test3", CreatedBy = "test3" });
            }
            catch (CreatingDuplicateException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CanGetMultipleCertificatesById()
        {
            _certificateRepository.Insert(new Certificate { CertificateName = "test", CreatedBy = "test6" });
            Certificate target1 = _certificateRepository.Insert(new Certificate { CertificateName = "test1", CreatedBy = "test6" });
            Certificate target2 = _certificateRepository.Insert(new Certificate { CertificateName = "test2", CreatedBy = "test6" });
            _certificateRepository.Insert(new Certificate { CertificateName = "test3", CreatedBy = "test6" });
            
            
            IEnumerable<Certificate> response = _certificateManager.GetCertificates(
                    new Model.DTO.Filters.CertificateFilterDTO { CertificateId = new int[] {target1.CertificateId,target2.CertificateId } });

            Assert.IsTrue(response.Count() is 2);
        }

        [Test]
        public void CannotGetCertificateByInvalidCertificateId()
        {
            Certificate inserted = _certificateRepository.Insert(new Certificate { CertificateName = "test", CreatedBy = "test7" });

            try
            {
                _certificateManager.GetCertificate(inserted.CertificateId + 1);
            }
            catch (GettingByInvalidIdException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CannotDeleteCertificateWithAttachedEmployeeCertificate()
        {
            Certificate certificate = _certificateRepository.Insert(new Certificate { CertificateName = "test", CreatedBy = "test8" });
            Employee employee = _employeeRepository.Insert(GenericEmployee());
            EmployeeCertificate inserted = _employeeCertificateRepository.Insert(new EmployeeCertificate
            {
                CertificateId = certificate.CertificateId,
                EmployeeId = employee.EmployeeId,
                CreatedBy = "test 8"
            });

            try
            {
                _certificateManager.DeleteCertificate(certificate.CertificateId);
            }
            catch (DeletingEntityWithExistingAttachmentsException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CanUpdateCertificate()
        {
            Certificate certificate = _certificateRepository.Insert(new Certificate { CertificateName = "test", CreatedBy = "test8" });
            String target = "Success";
            certificate.CertificateName = target;
            _certificateRepository.Update(certificate);
            Assert.True(_certificateRepository.GetBy(certificate.CertificateId).CertificateName.Equals(target));
        }
    }
}
