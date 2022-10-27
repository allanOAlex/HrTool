using GSG.Model;
using GSG.Model.DTO.Filters;
using GSG.Repository;
using GSG.Repository.Capability;
using GSG.Service.Interfaces;
using GSG.Shared.Exceptions;

namespace GSG.Service
{
    public class ProjectManager : IProjectManager
    {
        public IRepository<Project> _projectRepository;
        private readonly IIdentityContext _context;
        private readonly ModelValidator _modelValidator;
        public ProjectManager(IRepository<Project> projectRepository, IIdentityContext context, ModelValidator modelValidator)
        {
            _projectRepository = projectRepository;
            _context = context;
            _modelValidator = modelValidator;
        }

        public Project CreateProject(Project project)
        {
            var validation = _modelValidator.Validate(project);
            if (!validation.IsValid)
            {
                throw new InvalidModelException("Model is not valid");
            }
            project.CreatedBy = _context.UserName;
            Project existingProject = _projectRepository.GetAll(
                row => row.ProjectName.Equals(project.ProjectName)).FirstOrDefault();

            if (existingProject is not null)
                throw new CreatingDuplicateException("Project already exist");

            return _projectRepository.Insert(project);
            
        }

        public bool DeleteProject(int projectId)
        {
            Project existingProject = _projectRepository
                .Include($"{nameof(EmployeeProject)}")
                .GetAll(row => row.ProjectId == projectId).FirstOrDefault();
            if (existingProject is not null)
            {
                if (existingProject.EmployeeProject.Any())
                    throw new DeletingEntityWithExistingAttachmentsException("Project has attached EmployeeProject");
                return _projectRepository.Delete(existingProject);
            }
            else
                throw new DeletingByInvalidIdException("ProjectId does not exist"); 
        }

        public Project GetProject(int projectId)
        
        {
            Project results = _projectRepository.GetAll(row => row.ProjectId == projectId).FirstOrDefault();
            if (results is not null)
                return results;
            else
                throw new DeletingByInvalidIdException("Invalid ProjectId");

        }

        public IEnumerable<Project> GetProjects(ProjectFilterDTO filter)
        {
            IQueryable<Project> result = _projectRepository.GetAll().AsQueryable();

            if (filter?.ProjectName is not null)
                result = result.Where(row => filter.ProjectName.Contains(row.ProjectName));

            if (filter?.ProjectId is not null)
                result = result.Where(row => filter.ProjectId.Contains(row.ProjectId));

            if(filter?.From is not null)
                result = result.Where(row => DateTime.Compare(filter.From.Value, row.StartDate) < 0);

            if (filter?.To is not null)
                result = result.Where(row => row.EndDate.HasValue &&  filter.To.Value.CompareTo(row.EndDate) > 0);

            return result;
        }

        public Project UpdateProject(Project project)
        {
            var validation = _modelValidator.Validate(project);
            if (!validation.IsValid)
            {
                throw new InvalidModelException("Model is not valid");
            }
            return _projectRepository.Update(project);
        }
    }
}


            