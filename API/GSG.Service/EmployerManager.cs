using GSG.Model;
using GSG.Model.DTO.Filters;
using GSG.Repository;
using GSG.Repository.Capability;
using GSG.Service.Interfaces;
using GSG.Shared.Exceptions;

namespace GSG.Service;

public class EmployerManager : IEmployerManager
{
    public IRepository<Employer> _employerRepository;
    private readonly IIdentityContext _context;
    private readonly ModelValidator _modelValidator;

    public EmployerManager(IRepository<Employer> employerRepository, IIdentityContext context, ModelValidator modelValidator)
    {
        _employerRepository = employerRepository;
        _context = context;
        _modelValidator = modelValidator;
    }
    public Employer CreateEmployer(Employer employer)
    {
        var validation = _modelValidator.Validate(employer);
        if (!validation.IsValid)
        {
            throw new InvalidModelException("Model is not valid");
        }
        employer.CreatedBy = _context.UserName;
        Employer existingEmployer = _employerRepository.GetAll(
            row => row.EmployerName.Equals(employer.EmployerName)).FirstOrDefault();

        if (existingEmployer is null)
            return _employerRepository.Insert(employer);
        throw new CreatingDuplicateException("Employer already exists");
    }

    public bool DeleteEmployer(int employerId)
    {
        Employer existingEmployer = _employerRepository 
            .Include($"{nameof(EmployeeRole)}")
            .GetAll(row => row.EmployerId == employerId).FirstOrDefault();

        if (existingEmployer is not null)
        {
            if (existingEmployer.EmployeeRole.Any())
            {
                throw new DeletingEntityWithExistingAttachmentsException("Employer has an attached EmployeeRole");
            }
            return _employerRepository.Delete(existingEmployer);
        }
        throw new DeletingByInvalidIdException("EmployerId does not exist");
    }

    public Employer GetEmployer(int employerId)
    {
        Employer employer = _employerRepository.GetBy(employerId);
        if (employer is null)
            throw new GettingByInvalidIdException("Invalid EmployerId");
        return employer;
    }

    public IEnumerable<Employer> GetEmployers(EmployerFilterDTO filter)
    {
        if (filter?.EmployerName is not null && filter.EmployerName.Where(row => !string.IsNullOrWhiteSpace(row)).Any())
            return _employerRepository.GetAll(
                row => filter.EmployerName.Contains(row.EmployerName));
            
        if (filter?.EmployerId is not null)
            return _employerRepository.GetAll(
                row => filter.EmployerId.Contains(row.EmployerId));

        return _employerRepository.GetAll();
    }
    public Employer UpdateEmployer(Employer employer)
    {
        var validation = _modelValidator.Validate(employer);
        if (!validation.IsValid)
        {
            throw new InvalidModelException("Model is not valid");
        }
        return _employerRepository.Update(employer);
    }
}