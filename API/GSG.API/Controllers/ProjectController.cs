using AutoMapper;
using GSG.Model;
using GSG.Model.DTO.Filters;
using GSG.Model.DTO.Responses;
using GSG.Service.Interfaces;
using GSG.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using NuGet;

namespace GSG.API.Controllers
{

    [Authorize]
    [Route("api/project")]
    [ApiController]
    public class ProjectController : Controller
    {
        private readonly IProjectManager _projectManager;
        private readonly IMapper _mapper;

        public ProjectController(IProjectManager projectManager, IMapper mapper)
        {
            _projectManager = projectManager;
            _mapper = mapper;
        }

        //HTTP VERBS
        //POST(create) GET PUT(update) DELETE

        [HttpPost]
        public ActionResult<ProjectResponse> CreateProject(ProjectRequest project)
        {
            Project projectBO = _mapper.Map<Project>(project);
            Project resultBO = _projectManager.CreateProject(projectBO);
            var result = _mapper.Map<ProjectResponse>(resultBO);
            return Ok(result);
        }

        [HttpDelete]
        public IActionResult DeleteProject(int projectId)
        {
            _projectManager.DeleteProject(projectId);
            return Ok();
        }

        [HttpGet]
        public ActionResult<ResponseBody<IEnumerable<ProjectResponse>>> GetProjects([FromQuery] ProjectFilterDTO filter)
        {
            IEnumerable<Project> resultBO = _projectManager.GetProjects(filter);
            IEnumerable<ProjectResponse> projects = _mapper.Map<IEnumerable<ProjectResponse>>(resultBO);
            return Ok(new ResponseBody<IEnumerable<ProjectResponse>>
            {
                Body = projects
            });
        }


        [HttpGet]
        [Route("{projectId}")]
        public ActionResult<Project> GetProject(int id)
        {
            Project result = _projectManager.GetProject(id);
            return Ok(result);
        }
    }
}
