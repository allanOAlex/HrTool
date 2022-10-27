using GSG.Model;
using GSG.Model.DTO.Filters;
using GSG.Repository;
using GSG.Repository.Capability;
using GSG.Service.Interfaces;
using GSG.Shared.Exceptions;

namespace GSG.Service
{
    public class EmployeeProjectManager : IEmployeeProjectManager
    {
        private readonly IRepository<EmployeeProject> _employeeProjectRepository;
        private readonly IRepository<Project> _projectRepository;
        private readonly IIdentityContext _context;
        private readonly ModelValidator _modelValidator;

        public EmployeeProjectManager(IRepository<EmployeeProject> employeeProjectRepository,
                IRepository<Project> projectRepository, IIdentityContext context, ModelValidator modelValidator)
        {
            _employeeProjectRepository = employeeProjectRepository;
            _projectRepository = projectRepository;
            _context = context;
            _modelValidator = modelValidator;
        }

        public EmployeeProject AssignEmployeeProject(EmployeeProject employeeProject)
        {
            var validation = _modelValidator.Validate(employeeProject);
            if (!validation.IsValid)
            {
                throw new InvalidModelException("Model is not valid");
            }
            employeeProject.CreatedBy = _context.UserName;
            EmployeeProject existingProjects = _employeeProjectRepository.GetAll(
                    row => row.ProjectId == employeeProject.ProjectId &&
                    row.EmployeeId == employeeProject.EmployeeId).FirstOrDefault();
            if (existingProjects is not null)
                throw new CreatingDuplicateException("EmployeeProject already exists");

            return _employeeProjectRepository.Insert(employeeProject);
        }

        public EmployeeProject GetEmployeeProject(int id)
        {
            EmployeeProject empProject = _employeeProjectRepository.GetBy(id);
            if (empProject is null)
                throw new GettingByInvalidIdException("EmployeeProjectId does not exist");
            return empProject;
        }

        public EmployeeProject GetEmployeeProject(int employeeId, int projectId)
        {
            EmployeeProject empProject = _employeeProjectRepository.GetAll(row =>
                row.EmployeeId == employeeId && row.ProjectId == projectId).FirstOrDefault();
            if (empProject is not null)
                return empProject;
            throw new GettingByInvalidIdException("EmployeeProject does not exist for the given Employee and Project Id's");
        }

        public IEnumerable<EmployeeProject> GetEmployeeProjects(EmployeeProjectFilterDTO filter)
        {
            IQueryable<EmployeeProject> result = _employeeProjectRepository.Include($"{nameof(Project)}").GetAll().AsQueryable();

            if (filter?.EmployeeId is not null)
                result = result.Where(row => filter.EmployeeId.Contains(row.EmployeeId));

            if (filter?.ProjectId is not null)
                result = result.Where(row => filter.ProjectId.Contains(row.ProjectId));

            if (filter?.ProjectName is not null)
            {
                IEnumerable<Project> projects =
                    _projectRepository
                        .Include($"{nameof(EmployeeProject)}.{nameof(Employee)}")
                        .GetAll(row => filter.ProjectName.Contains(row.ProjectName));
                result = result.Where(row => projects.Any(cert => cert.ProjectId == row.ProjectId));
            }

            if (filter?.From is not null)
                result = result.Where(row => DateTime.Compare(filter.From.Value, row.StartDate) < 0);

            if (filter?.To is not null)
                result = result.Where(row => row.EndDate.HasValue && filter.To.Value.CompareTo(row.EndDate) > 0);

            return result;
        }

        public bool UnassignEmployeeProject(int employeeProjectId)
        {
            EmployeeProject empProject = _employeeProjectRepository
                .GetBy(employeeProjectId);

            if (empProject is not null)
                return _employeeProjectRepository.Delete(empProject);
            throw new DeletingByInvalidIdException("EmployeeProjectID does not exist");
        }

        public EmployeeProject UpdateEmployeeProject(EmployeeProject empProject)
        {
            var validation = _modelValidator.Validate(empProject);
            if (!validation.IsValid)
            {
                throw new InvalidModelException("Model is not valid");
            }
            return _employeeProjectRepository.Update(empProject);
        }
    }
}