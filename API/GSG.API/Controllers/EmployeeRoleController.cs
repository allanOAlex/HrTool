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
    [Route("api/employeeRole")]
    [ApiController]
    public class EmployeeRoleController : Controller
    {
        private readonly IEmployeeRoleManager _employeeRoleManager;
        private readonly IMapper _mapper;

        public EmployeeRoleController(IEmployeeRoleManager employeeRoleManager, IMapper mapper)
        {
            _employeeRoleManager = employeeRoleManager;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<ResponseBody<IEnumerable<EmployeeRoleProfileResponse>>> GetEmployeeRoles([FromQuery] EmployeeRoleFilterDTO filter)
        {
            IEnumerable<EmployeeRole> resultBO = _employeeRoleManager.GetEmployeeRoles(filter);
            IEnumerable<EmployeeRoleProfileResponse> results = _mapper.Map<IEnumerable<EmployeeRoleProfileResponse>>(resultBO);
            return Ok(new ResponseBody<IEnumerable<EmployeeRoleProfileResponse>>
            {
                Body = results
            });
        }


        [HttpGet]
        [Route("{employeeRoleId}")]
        public ActionResult<EmployeeRoleResponse> GetEmployeeRole(int id)
        {
            EmployeeRole employeeRole = _employeeRoleManager.GetEmployeeRole(id);
            EmployeeRoleResponse result = _mapper.Map<EmployeeRoleResponse>(employeeRole);
            return Ok(result);
        }

    }
}
