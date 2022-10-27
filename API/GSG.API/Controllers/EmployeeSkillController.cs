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

namespace GSG.API.Controllers
{

    [Authorize]
    [Route("api/employeeSkill")]
    [ApiController]
    public class EmployeeSkillController : Controller
    {
        private readonly IEmployeeSkillManager _employeeSkillManager;
        private readonly IMapper _mapper;

        public EmployeeSkillController(IEmployeeSkillManager employeeSkillManager, IMapper mapper)
        {
            _employeeSkillManager = employeeSkillManager;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<EmployeeSkillProfileResponse>> GetEmployeeSkills([FromQuery] EmployeeSkillFilterDTO filter)
        {
            IEnumerable<EmployeeSkill> resultBO = _employeeSkillManager.GetEmployeeSkills(filter);
            IEnumerable<EmployeeSkillProfileResponse> results = _mapper.Map<IEnumerable<EmployeeSkillProfileResponse>>(resultBO);
            return Ok(new ResponseBody<IEnumerable<EmployeeSkillProfileResponse>>
            {
                Body = results
            });
        }


        [HttpGet]
        [Route("{Id}")]
        public ActionResult<ResponseBody<EmployeeSkillResponse>> GetEmployeeSkill(int Id)
        {
            EmployeeSkill result = _employeeSkillManager.GetEmployeeSkill(Id);
            EmployeeSkillResponse empskillResponse = _mapper.Map<EmployeeSkillResponse>(result);
            return Ok(new ResponseBody<EmployeeSkillResponse>
            {
                Body = empskillResponse
            });
        }

        [HttpPost]
        public ActionResult<EmployeeSkillResponse> CreateEmployeeSkill(EmployeeSkillRequest empSkill)
        {
            EmployeeSkill empSkillBO = _mapper.Map<EmployeeSkill>(empSkill);
            EmployeeSkill resultBO = _employeeSkillManager.CreateEmployeeSkill(empSkillBO);
            var result = _mapper.Map<EmployeeSkillResponse>(resultBO);
            return Ok(result);
        }

        [HttpDelete]
        public ActionResult<EmployeeSkillResponse> DeleteEmployeeSkill(EmployeeSkillRequest empSkill)
        {
            EmployeeSkill empSkillBO = _mapper.Map<EmployeeSkill>(empSkill);
            bool resultBO = _employeeSkillManager.UnassignEmployeeSkill(empSkillBO.EmployeeId, empSkillBO.SkillId);
            var result = _mapper.Map<EmployeeSkillResponse>(resultBO);
            return Ok(new ResponseBody<EmployeeSkillResponse>
            {
                Body = result
            });
        }

        [HttpDelete("{skillId}/{empId}")]
        public async Task<IActionResult> DeleteEmployeeSkill(int skillId, int empId)
        {
            try
            {
                //bool result = _employeeSkillManager.DeleteEmployeeSkill(skillId, empId);
                bool result = _employeeSkillManager.UnassignEmployeeSkill(skillId, empId);
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
