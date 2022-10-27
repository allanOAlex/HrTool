using GSG.Model;
using GSG.Model.DTO.Filters;

namespace GSG.Service.Interfaces
{
    public interface IProjectManager
    {
        Project CreateProject(Project project);
        bool DeleteProject(int projectId);
        IEnumerable<Project> GetProjects(ProjectFilterDTO filter);
        Project GetProject(int projectId);
        Project UpdateProject(Project project);
    }
}


