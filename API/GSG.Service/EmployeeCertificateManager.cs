using GSG.Model;
using GSG.Model.DTO.Filters;
using GSG.Repository;
using GSG.Repository.Capability;
using GSG.Service.Interfaces;
using GSG.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace GSG.Service
{
    public class EmployeeCertificateManager : IEmployeeCertificateManager
    {
        private readonly IRepository<EmployeeCertificate> _employeeCertificateRepository;
        private readonly IRepository<Certificate> _certificateRepository;
        private readonly IIdentityContext _context;
        private readonly ModelValidator _modelValidator;
        private DateTime dateTime = new DateTime();
    

        public EmployeeCertificateManager(IRepository<EmployeeCertificate> employeeCertificateRepository, 
                IRepository<Certificate> certificateRepository, IIdentityContext context, ModelValidator modelValidator)
        {
            _employeeCertificateRepository = employeeCertificateRepository;
            _certificateRepository = certificateRepository;
            _context = context;
            _modelValidator = modelValidator;
        }

        public EmployeeCertificate AssignEmployeeCertificate(EmployeeCertificate employeeCertificate)
        {
            var validation = _modelValidator.Validate(employeeCertificate);
            if (!validation.IsValid)
            {
                throw new InvalidModelException("Model is not valid");
            }

            employeeCertificate.CertificateId = employeeCertificate.CertificateId;
            employeeCertificate.EmployeeId = employeeCertificate.EmployeeId;
            employeeCertificate.Created = DateTime.Now.Date;
            employeeCertificate.Updated = employeeCertificate.Created;
            employeeCertificate.CreatedBy = _context.UserName;
            employeeCertificate.RowVer = Guid.NewGuid();


            EmployeeCertificate existingEmpCertificates = _employeeCertificateRepository.GetAll(row => 
            row.CertificateId == employeeCertificate.CertificateId && 
            row.EmployeeId == employeeCertificate.EmployeeId).FirstOrDefault();

            if (existingEmpCertificates is not null)
                throw new CreatingDuplicateException("EmployeeCertificate already exists");

            return _employeeCertificateRepository.Insert(employeeCertificate);
        }

        public EmployeeCertificate GetEmployeeCertificate(int id)
        {
            EmployeeCertificate empCertificate = _employeeCertificateRepository.GetBy(id);
            if (empCertificate is null)
                throw new GettingByInvalidIdException("EmployeeCertificateId does not exist");
            return empCertificate;
        }

        public EmployeeCertificate CreateEmployeeCertificate(EmployeeCertificate employeeCert)
        {
            employeeCert.CertificateId = employeeCert.CertificateId;
            employeeCert.EmployeeId = employeeCert.EmployeeId;
            employeeCert.AwardedDate = employeeCert.AwardedDate;
            employeeCert.Created = DateTime.Now.Date;
            employeeCert.Updated = employeeCert.Created;
            employeeCert.CreatedBy = _context.UserName;
            employeeCert.RowVer = Guid.NewGuid();


            var validation = _modelValidator.Validate(employeeCert);
            if (!validation.IsValid)
            {
                throw new InvalidModelException("Model is not valid");
            }

            IEnumerable<EmployeeCertificate> employeeCerts = _employeeCertificateRepository.GetAll().Where(row =>
                row.EmployeeCertificateId.Equals(employeeCert.EmployeeCertificateId) &&
                row.CertificateId.Equals(employeeCert.CertificateId) &&
                row.EmployeeId.Equals(employeeCert.EmployeeId));

            if (employeeCerts.Any())
                throw new CreatingDuplicateException("Duplicate Employee Certificate");



            return _employeeCertificateRepository.Insert(employeeCert);
        }

        public EmployeeCertificate GetEmployeeCertificate(int employeeId, int certificateId)
        {
            EmployeeCertificate empCertificate = _employeeCertificateRepository.GetAll(row =>
                row.EmployeeId == employeeId && row.CertificateId == certificateId).FirstOrDefault();
            if (empCertificate is not null)
                return empCertificate;
            throw new GettingByInvalidIdException("EmployeeCertificate does not exist for the given Employee and Certificate Id's");
        }

        public IEnumerable<EmployeeCertificate> GetEmployeeCertificates(EmployeeCertificateFilterDTO filter)
        {
            //IQueryable<EmployeeCertificate> result = _employeeCertificateRepository.GetAll().Include(c => c.Certificate).AsQueryable();
            IQueryable<EmployeeCertificate> result = _employeeCertificateRepository.Include($"{nameof(Certificate)}").GetAll().AsQueryable();

            if (result is null)
            {
                return null;
            }

            if (filter?.EmployeeId is not null)
                result = result.Where(row => filter.EmployeeId.Contains(row.EmployeeId));

            if (filter?.CertificateId is not null)
                result = result.Where(row => filter.CertificateId.Contains(row.CertificateId));

            if (filter?.CertificateName is not null)
            {
                IEnumerable<Certificate> certificates = _certificateRepository
                    .Include($"{nameof(EmployeeCertificate)}.{nameof(Employee)}")
                    .GetAll(row => filter.CertificateName.Contains(row.CertificateName));

                result = result.Where(row => certificates.Any(cert => cert.CertificateId == row.CertificateId));
            }

            if (filter?.From is not null)
                result = result.Where(row => DateTime.Compare(filter.From.Value.Date , row.AwardedDate) < 0);
               

            if (filter?.To is not null)
                result = result.Where(row => filter.To.Value.CompareTo(row.AwardedDate) > 0);

            return result;
        }

        public bool UnassignEmployeeCertificate(int employeeCertificateId)
        {
            EmployeeCertificate empCertificate = _employeeCertificateRepository
                .GetBy(employeeCertificateId);
            
            if (empCertificate is not null)
                return _employeeCertificateRepository.Delete(empCertificate);
            throw new DeletingByInvalidIdException("EmployeeCertificateID does not exist");
        }

        public bool UnassignEmployeeCertificate(int certId, int empId)
        {
            EmployeeCertificate existingEmployeeCertificate = _employeeCertificateRepository.GetAll().Where(
                row => row.EmployeeId == empId && row.CertificateId == certId).FirstOrDefault();
            if (existingEmployeeCertificate is not null)
                return _employeeCertificateRepository.Delete(existingEmployeeCertificate);
            else throw new DeletingByInvalidIdException("Employee Certificate does not exist for EmployeeId and CertificateId");
        }

        public EmployeeCertificate UpdateEmployeeCertificate(EmployeeCertificate empCertificate)
        {
            var validation = _modelValidator.Validate(empCertificate);
            if (!validation.IsValid)
            {
                throw new InvalidModelException("Model is not valid");
            }
            return _employeeCertificateRepository.Update(empCertificate);
        }
    }
}
