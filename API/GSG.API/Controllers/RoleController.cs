using AutoMapper;
using GSG.Model;
using GSG.Model.DTO.Filters;
using GSG.Model.DTO.Requests;
using GSG.Model.DTO.Responses;
using GSG.Service.Interfaces;
using GSG.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GSG.API.Controllers
{

    [Authorize]
    [Route("api/role")]
    [ApiController]
    public class RoleController : Controller
    {
        private readonly IRoleManager _roleManager;
        private readonly IMapper _mapper;

        public RoleController(IRoleManager roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }
        
        //HTTP VERBS
        //POST(create) GET PUT(update) DELETE

        [HttpPost]
        public ActionResult<RoleResponse> CreateRole(RoleRequest role)
        {
            Role roleBO = _mapper.Map<Role>(role);
            Role resultBO = _roleManager.CreateRole(roleBO);
            var result = _mapper.Map<RoleResponse>(resultBO);
            return Ok(result);
        }

        [HttpDelete]
        public IActionResult DeleteRole(int roleId)
        {
            _roleManager.DeleteRole(roleId);
            return Ok();
        }

        [HttpGet]
        public ActionResult<ResponseBody<IEnumerable<RoleResponse>>> GetRoles([FromQuery] RoleFilterDTO filter)
        {
            IEnumerable<Role> resultBO = _roleManager.GetRoles(filter);
            IEnumerable<RoleResponse> roles = _mapper.Map<IEnumerable<RoleResponse>>(resultBO);
            return Ok(new ResponseBody<IEnumerable<RoleResponse>>
            {
                Body = roles
            });
        }


        [HttpGet]
        [Route("{roleId}")]
        public IActionResult GetRole(int id)
        {
            Role result = _roleManager.GetRole(id);
            return Ok(result);
        }
    }
}
