using AutoMapper;
using GSG.Model;
using GSG.Model.DTO.Filters;
using GSG.Model.DTO.Requests;
using GSG.Model.DTO.Responses;
using GSG.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GSG.API.Controllers
{

    [Authorize]
    [Route("api/employer")]
    [ApiController]
    public class EmployerController : Controller
    {
        private readonly IEmployerManager _employerManager;
        private readonly IMapper _mapper;

        public EmployerController(IEmployerManager employerManager, IMapper mapper)
        {
            _employerManager = employerManager;
            _mapper = mapper;
        }

        //HTTP VERBS
        //POST(create) GET PUT(update) DELETE

        [HttpPost]
        public ActionResult<EmployerResponse> CreateEmployer(EmployerRequest employer)
        {
            Employer employerBO = _mapper.Map<Employer>(employer);
            Employer resultBO = _employerManager.CreateEmployer(employerBO);
            var result = _mapper.Map<EmployerResponse>(resultBO);
            return Ok(result);
        }

        [HttpDelete]
        public IActionResult DeleteEmployer(int employerId)
        {
            _employerManager.DeleteEmployer(employerId);
            return Ok();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Employer>> GetEmployers([FromQuery] EmployerFilterDTO filter)
        {
            IEnumerable<Employer> resultBO = _employerManager.GetEmployers(filter); 
            IEnumerable<EmployerResponse> employers = _mapper.Map<IEnumerable<EmployerResponse>>(resultBO);
            return Ok(employers);
        }


        [HttpGet]
        [Route("{employerId}")]
        public ActionResult<Employer> GetEmployer(int id)
        {
            Employer employer = _employerManager.GetEmployer(id);
            return Ok(employer);
        }
    }
}
