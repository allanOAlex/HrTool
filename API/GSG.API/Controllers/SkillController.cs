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
    [Route("api/skill")]
    [ApiController]
    public class SkillController : Controller
    {
        private readonly ISkillManager _skillManager;
        private readonly IMapper _mapper;

        public SkillController(ISkillManager skillManager, IMapper mapper)
        {
            _skillManager = skillManager;
            _mapper = mapper;
        }
        //HTTP VERBS
        //POST(create) GET PUT(update) DELETE

        [HttpPost]
        public ActionResult<SkillResponse> CreateSkill(SkillRequest skill)
        {
            Skill skillBO = _mapper.Map<Skill>(skill);
            Skill resultBO = _skillManager.CreateSkill(skillBO);
            var result = _mapper.Map<SkillResponse>(resultBO);
            return Ok(result);
        }

        [HttpDelete]
        public IActionResult DeleteSkill(int skillId)
        {
            _skillManager.DeleteSkill(skillId);
            return Ok();
        }

        [HttpGet]
        public ActionResult<ResponseBody<IEnumerable<SkillResponse>>> GetSkills([FromQuery] SkillFilterDTO filter)
        {
            IEnumerable<Skill> resultBO = _skillManager.GetSkills(filter);
            IEnumerable<SkillResponse> results = _mapper.Map<IEnumerable<SkillResponse>>(resultBO);
            return Ok(new ResponseBody<IEnumerable<SkillResponse>>
            {
                Body = results
            });
        }


        [HttpGet]
        [Route("{skillId}")]
        public ActionResult<ResponseBody<SkillResponse>> GetSkill(int skillId)
        {
            Skill certificate = _skillManager.GetSkill(skillId);
            SkillResponse skillResponse = _mapper.Map<SkillResponse>(certificate);
            return Ok(
                new ResponseBody<SkillResponse>
                {
                    Body = skillResponse
                });
        }
    }
}
