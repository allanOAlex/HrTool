using AutoMapper;
using GSG.Model;
using GSG.Model.DTO.Filters;
using GSG.Model.DTO.Responses;
using GSG.Service.Interfaces;
using GSG.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GSG.API.Controllers
{

    [Authorize]
    [Route("api/employeeproject")]
    [ApiController]
    public class EmployeeProjectController : Controller
    {
        private readonly IEmployeeProjectManager _employeeProjectManager;
        private readonly IMapper _mapper;

        public EmployeeProjectController(IEmployeeProjectManager employeeProjectManager, IMapper mapper)
        {
            _employeeProjectManager = employeeProjectManager;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<ResponseBody<IEnumerable<EmployeeProjectResponse>>> GetEmployeeProjects([FromQuery] EmployeeProjectFilterDTO filter)
        {
            IEnumerable<EmployeeProject> resultBO = _employeeProjectManager.GetEmployeeProjects(filter);
            IEnumerable<EmployeeProjectProfileResponse> results = _mapper.Map<IEnumerable<EmployeeProjectProfileResponse>>(resultBO);
            return Ok(new ResponseBody<IEnumerable<EmployeeProjectProfileResponse>>
            {
                Body = results
            });
        }


        [HttpGet]
        [Route("{employeeProjectId}")]
        public ActionResult<EmployeeProjectResponse> GetEmployeeProject(int id)
        {
            EmployeeProject employeeProject = _employeeProjectManager.GetEmployeeProject(id);
            IEnumerable<EmployeeProjectResponse> result = _mapper.Map<IEnumerable<EmployeeProjectResponse>>(employeeProject);
            return Ok(result);
        }

    }
}
