using AutoMapper;
using GSG.Model;
using GSG.Model.DTO.Filters;
using GSG.Model.DTO.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GSG.Service.Interfaces;
using GSG.Shared;
using GSG.Model.DTO.Requests;
using GSG.Service;

namespace GSG.API.Controllers
{

    [Authorize]
    [Route("api/employeecertificate")]
    [ApiController]
    public class EmployeeCertificateController : Controller
    {
        private readonly IEmployeeCertificateManager _employeeCertificateManager;
        private readonly IMapper _mapper;

        public EmployeeCertificateController(IEmployeeCertificateManager employeeCertificateManager, IMapper mapper)
        {
            _employeeCertificateManager = employeeCertificateManager;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<ResponseBody<IEnumerable<EmployeeCertificateProfileResponse>>> GetEmployeeCertificates
            ([FromQuery] EmployeeCertificateFilterDTO filter)
        {
            IEnumerable<EmployeeCertificate> resultBO = _employeeCertificateManager.GetEmployeeCertificates(filter);
            IEnumerable<EmployeeCertificateProfileResponse> result = _mapper.Map<IEnumerable<EmployeeCertificateProfileResponse>>(resultBO);
            return Ok(
                new ResponseBody<IEnumerable<EmployeeCertificateProfileResponse>>
                {
                    Body = result
                });
        }

        [Route("{id}")]
        [HttpGet]
        public ActionResult<EmployeeCertificateResponse> GetEmployeeCertificate(int id)
        {
            EmployeeCertificate resultBO = _employeeCertificateManager.GetEmployeeCertificate( id);
            EmployeeCertificateResponse result = _mapper.Map<EmployeeCertificateResponse>(resultBO);
            return Ok(new ResponseBody<EmployeeCertificateResponse>
            {
                Body = result
            });
            //return Ok(result);
        }

        [HttpPost]
        public ActionResult<EmployeeCertificateResponse> CreateEmployeeCertificate(EmployeeCertificateRequest empCert)
        {
            EmployeeCertificate empCertBO = _mapper.Map<EmployeeCertificate>(empCert);
            EmployeeCertificate resultBO = _employeeCertificateManager.CreateEmployeeCertificate(empCertBO);
            var result = _mapper.Map<EmployeeCertificateResponse>(resultBO);
            return Ok(result);
        }

        [HttpDelete("{certId}/{empId}")]
        public async Task<IActionResult> DeleteEmployeeCertificate(int certId, int empId)
        {
            try
            {
                bool result = _employeeCertificateManager.UnassignEmployeeCertificate(certId, empId);
                if (result == true)
                {
                    return Ok(new ResponseBody<bool>
                    {
                        Body = result
                    });
                }

                return BadRequest(new ResponseBody<bool>
                {
                    Body = result
                });

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }
    }
}
