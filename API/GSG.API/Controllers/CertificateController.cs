using AutoMapper;
using GSG.Model;
using GSG.Model.DTO.Filters;
using GSG.Model.DTO.Requests;
using GSG.Model.DTO.Responses;
using GSG.Service.Interfaces;
using GSG.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace GSG.API.Controllers
{
    [Route("api/certificate")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    [Authorize(Roles = "Manager")]
    [ApiController]
    public class CertificateController : Controller
    {
        private readonly ICertificateManager _certificateManager;
        private readonly IMapper _mapper;

        public CertificateController(ICertificateManager certificateManager, IMapper mapper)
        {
            _certificateManager = certificateManager;
            _mapper = mapper;
        }

        /// <summary>
        /// this 
        /// </summary>
        /// <param name="certificate"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<CertificateResponse> CreateCertificate(CertificateRequest certificate)
        {
            Certificate certificateBO = _mapper.Map<Certificate>(certificate);
            Certificate resultBO = _certificateManager.CreateCertificate(certificateBO);
            var result = _mapper.Map<CertificateResponse>(resultBO);
            return Ok(result);
        }

        [HttpDelete]
        [Route("{certificateId}")]
        public IActionResult DeleteCertificate(int certificateId)
        {
            _certificateManager.DeleteCertificate(certificateId);
            return Ok();
        }

        [HttpGet]
        public ActionResult<ResponseBody<IEnumerable<CertificateResponse>>> GetCertificates([FromQuery] CertificateFilterDTO filter)
        {
            IEnumerable<Certificate> resultBO = _certificateManager.GetCertificates(filter);
            IEnumerable<CertificateResponse> certificates = _mapper.Map<IEnumerable<CertificateResponse>>(resultBO);
            return Ok(
                new ResponseBody<IEnumerable<CertificateResponse>>
                {
                    Body = certificates
                });
        }


        [HttpGet]
        [Route("{certificateId}")]
        public ActionResult<ResponseBody<CertificateResponse>> GetCertificate(int certificateId)
        {
            Certificate certificate = _certificateManager.GetCertificate(certificateId);
            CertificateResponse certificateResponse = _mapper.Map<CertificateResponse>(certificate);
            return Ok(
                new ResponseBody<CertificateResponse>
                {
                    Body = certificateResponse
                });
        }
    }
}