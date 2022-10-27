using GSG.Model;
using GSG.Model.DTO.Filters;
using GSG.Repository;
using GSG.Repository.Capability;
using GSG.Service.Interfaces;
using GSG.Shared.Exceptions;

namespace GSG.Service
{
    public class CertificateManager : ICertificateManager
    {
        private readonly IRepository<Certificate> _certificateRepository;
        private readonly IIdentityContext _context;
        private readonly ModelValidator _modelValidator;

        public CertificateManager(
            IRepository<Certificate> certificateRepository, 
            IIdentityContext context, 
            ModelValidator modelValidator)
        {
            _certificateRepository = certificateRepository;
            _context = context;
            _modelValidator = modelValidator;
        }

        public Certificate CreateCertificate(Certificate certificate)
        {
            var validation = _modelValidator.Validate(certificate);
            if (!validation.IsValid)
            {
                throw new InvalidModelException("Model is not valid");
            }
            certificate.CreatedBy = _context.UserName;
            Certificate existingCertificate = _certificateRepository.GetAll(
                row => row.CertificateName.Equals(certificate.CertificateName)).FirstOrDefault();

            if (existingCertificate is null)
                return _certificateRepository.Insert(certificate);
            throw new CreatingDuplicateException("Certificate already exists");
        }

        public bool DeleteCertificate(int certificateId)
        { 
            Certificate existingCertificate = _certificateRepository
                .Include($"{nameof(EmployeeCertificate)}")
                .GetAll(row => row.CertificateId == certificateId).FirstOrDefault();

            if (existingCertificate is not null)
            {
                if (existingCertificate.EmployeeCertificate.Any())
                {
                    throw new DeletingEntityWithExistingAttachmentsException(
                        "Certificate has attached EmployeeCertificate");
                }
                return _certificateRepository.Delete(existingCertificate);
            }
            throw new DeletingByInvalidIdException("CertificateId does not exist");
        }

        public IEnumerable<Certificate> GetCertificates(CertificateFilterDTO filter = null)
        {
            if (filter?.CertificateName is not null && filter.CertificateName.Where(row=>!string.IsNullOrWhiteSpace(row)).Any())
            {
                return _certificateRepository.GetAll(row => filter.CertificateName.
                    Contains(row.CertificateName));
            }
            if (filter?.CertificateId is not null) 
            {
                return _certificateRepository.GetAll(row => filter.CertificateId
                    .Contains(row.CertificateId));
            }
            return _certificateRepository.GetAll();
        }

        public Certificate GetCertificate(int certificateId)
        {
            Certificate certificate = _certificateRepository.GetBy(certificateId);
            if (certificate is null)
                throw new GettingByInvalidIdException("Invalid CertificateId");
            return certificate;
        }

        public Certificate UpdateCertificate(Certificate certificate)
        {
            var validation = _modelValidator.Validate(certificate);
            if (!validation.IsValid)
            {
                throw new InvalidModelException("Model is not valid");
            }
            return _certificateRepository.Update(certificate);
        }
    }
}