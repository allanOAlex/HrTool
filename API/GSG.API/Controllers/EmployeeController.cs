using AutoMapper;
using GSG.Model;
using GSG.Model.DTO.Filters;
using GSG.Model.DTO.Requests;
using GSG.Model.DTO.Responses;
using GSG.Service;
using GSG.Service.Interfaces;
using GSG.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace GSG.API.Controllers
{
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    [Authorize(Roles = "Manager")]
    [Route("api/employee")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeManager _employeeManager;
        private readonly IEmployeeCertificateManager _employeeCertificateManager;
        private readonly IEmployeeProjectManager _employeeProjectManager;
        private readonly IEmployeeRoleManager _employeeRoleManager;
        private readonly IEmployeeSkillManager _employeeSkillManager;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeCertificateManager employeeCertificateManager, IEmployeeProjectManager employeeProjectManager,
                                    IEmployeeRoleManager employeeRoleManager, IEmployeeSkillManager employeeSkillManager, 
                                    IMapper mapper, IEmployeeManager employeeManager)
        {
            _employeeManager = employeeManager;
            _employeeCertificateManager = employeeCertificateManager;
            _employeeProjectManager = employeeProjectManager;
            _employeeRoleManager = employeeRoleManager;
            _employeeSkillManager = employeeSkillManager;
            _mapper = mapper;
        }

        [HttpPost("createnewemployee")]
        public ActionResult<EmployeeResponse> CreateEmployee(EmployeeRequest employee)
        {
            Employee employeeBO = _mapper.Map<Employee>(employee);
            Employee resultBO = _employeeManager.CreateEmployee(employeeBO);
            var result = _mapper.Map<EmployeeResponse>(resultBO);
            return Ok(result);
        }

        [HttpDelete]
        public IActionResult DeleteEmployee(int employeeId)
        {
            _employeeManager.DeleteEmployee(employeeId);
            return Ok();
        }

        [HttpGet]
        public ActionResult<ResponseBody<IEnumerable<EmployeeResponse>>> GetEmployees([FromQuery] EmployeeFilterDTO filter)
        {
            IEnumerable<Employee> resultBO = _employeeManager.GetEmployees(filter);
            IEnumerable<EmployeeGridResponse> employees = _mapper.Map<IEnumerable<EmployeeGridResponse>>(resultBO);


            return Ok(new ResponseBody<IEnumerable<EmployeeGridResponse>>
            {
                Body = employees
            });
        }

        [HttpGet("full")]
        public ActionResult<ResponseBody<IEnumerable<EmployeeGridResponse>>> GetEmployeesFull([FromQuery] EmployeeFilterDTO filter)
        {
            IEnumerable<Employee> resultBO = _employeeManager.GetEmployeesFull(filter).ToList();
            IEnumerable<EmployeeGridResponse> employees = _mapper.Map<IEnumerable<EmployeeGridResponse>>(resultBO);


            return Ok(new ResponseBody<IEnumerable<EmployeeGridResponse>>
            {
                Body = employees
            });
        }


        [HttpGet("{employeeId}")]
        public ActionResult<ResponseBody<EmployeeResponse>> GetEmployee(int employeeId)
        {
            Employee emp = _employeeManager.GetEmployee(employeeId);
            EmployeeResponse result = _mapper.Map<EmployeeResponse>(emp);
            return Ok(new ResponseBody<EmployeeResponse>
            {
                Body = result
            });
        }

        [HttpPost]
        public ActionResult<EmployeeCertificateResponse> AssignEmployeeCertificate(EmployeeCertificateRequest employeeCertificate)
        {
            EmployeeCertificate employeeCertificateBO = _mapper.Map<EmployeeCertificate>(employeeCertificate);
            EmployeeCertificate resultBO = _employeeCertificateManager.AssignEmployeeCertificate(employeeCertificateBO);
            var result = _mapper.Map<EmployeeCertificateResponse>(resultBO);
            return Ok(result);
        }

        [HttpDelete]
        [Route("certificate")]
        public IActionResult UnassignEmployeeCertificate(int employeeCertificateId)
        {
            _employeeCertificateManager.UnassignEmployeeCertificate(employeeCertificateId);
            return Ok();
        }

        [HttpPost]
        [Route("project")]
        public ActionResult<EmployeeProjectResponse> AssignEmployeeProject(EmployeeProjectRequest employeeProject)
        {
            EmployeeProject employeeProjectBO = _mapper.Map<EmployeeProject>(employeeProject);
            EmployeeProject resultBO = _employeeProjectManager.AssignEmployeeProject(employeeProjectBO);
            var result = _mapper.Map<EmployeeProjectResponse>(resultBO);
            return Ok(result);
        }

        [HttpDelete]
        [Route("project")]
        public IActionResult UnassignEmployeeProject(int employeeProjectId)
        {
            _employeeProjectManager.UnassignEmployeeProject(employeeProjectId);
            return Ok();
        }

        [HttpPost]
        [Route("role")]
        public ActionResult<EmployeeRoleResponse> AssignEmployeeRole(EmployeeRoleRequest employeeRole)
        {
            EmployeeRole employeeRoleBO = _mapper.Map<EmployeeRole>(employeeRole);
            EmployeeRole resultBO = _employeeRoleManager.AssignEmployeeRole(employeeRoleBO);
            var result = _mapper.Map<EmployeeRoleResponse>(resultBO);
            return Ok(result);
        }

        [HttpDelete]
        [Route("role")]
        public IActionResult UnassignEmployeeRole(int employeeRoleId)
        {
            _employeeRoleManager.UnassignEmployeeRole(employeeRoleId);
            return Ok();
        }

        [HttpPost]
        [Route("skill")]
        public ActionResult<EmployeeSkillResponse> AssignEmployeeSkill(EmployeeSkillRequest employeeSkill)
        {
            EmployeeSkill employeeSkillBO = _mapper.Map<EmployeeSkill>(employeeSkill);
            EmployeeSkill resultBO = _employeeSkillManager.AssignEmployeeSkill(employeeSkillBO);
            var result = _mapper.Map<EmployeeSkillResponse>(resultBO);
            return Ok(result);
        }

        [HttpDelete]
        [Route("skill")]
        public IActionResult UnassignEmployeeSkill(int employeeId, int skillId)
        {
            _employeeSkillManager.UnassignEmployeeSkill(employeeId, skillId);
            return Ok();
        }

        
        [HttpPut("updateemployee/{empId}")]
        public ActionResult<EmployeeResponse> UpdateEmployee(int empId, EmployeeRequest updateEmpRequest)
        {
            try
            {
                Employee employeeBO = _mapper.Map<Employee>(updateEmpRequest);
                Employee resultBO = _employeeManager.UpdateEmployee(empId, employeeBO);
                var result = _mapper.Map<EmployeeResponse>(resultBO);
                return Ok(result);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error updating data");
            }
        }

    }
}
