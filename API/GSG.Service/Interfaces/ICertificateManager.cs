using GSG.Model;
using GSG.Model.DTO.Filters;

namespace GSG.Service.Interfaces
{
    public interface ICertificateManager
    {
        Certificate CreateCertificate(Certificate certificate);
        bool DeleteCertificate(int certificateId);
        IEnumerable<Certificate> GetCertificates(CertificateFilterDTO filter = null);
        Certificate GetCertificate(int certificateId);
        Certificate UpdateCertificate(Certificate certificate);
    }
}