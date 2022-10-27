using GSG.Model;
using GSG.Model.DTO.Filters;
using GSG.Repository;
using GSG.Repository.Capability;
using GSG.Service.Interfaces;
using GSG.Shared.Exceptions;


namespace GSG.Service
{
    public class EmployeeSkillManager : IEmployeeSkillManager
    {
        public IRepository<EmployeeSkill> _employeeSkillRepository;
        public IRepository<Skill> _skillRepository;
        private readonly IIdentityContext _context;
        private readonly ModelValidator _modelValidator;

        public EmployeeSkillManager(IRepository<EmployeeSkill> employeeSkillRepository, IRepository<Skill> skillRepository,
                IRepository<Employee> employeeRepository, IIdentityContext context, ModelValidator modelValidator)
        {
            _employeeSkillRepository = employeeSkillRepository;
            _skillRepository = skillRepository;
            _context = context;
            _modelValidator = modelValidator;
        }

        public EmployeeSkill AssignEmployeeSkill(EmployeeSkill employeeSkill)
        {
            var validation = _modelValidator.Validate(employeeSkill);
            if (!validation.IsValid)
            {
                throw new InvalidModelException("Model is not valid");
            }
            employeeSkill.CreatedBy = _context.UserName;
            EmployeeSkill existingSkill = _employeeSkillRepository.GetAll(
                    row => row.SkillId == employeeSkill.SkillId &&
                    row.EmployeeId == employeeSkill.EmployeeId).FirstOrDefault();
            if (existingSkill is not null)
                throw new CreatingDuplicateException("EmployeeSkill already exists");

            return _employeeSkillRepository.Insert(employeeSkill);
        }
        public EmployeeSkill GetEmployeeSkill(int id)
        {
            EmployeeSkill existingEmployeeSkill = _employeeSkillRepository.GetBy(id);
            if (existingEmployeeSkill is null)
                throw new GettingByInvalidIdException("EmployeeSkillId does not exist");
            return existingEmployeeSkill;
        }

        public EmployeeSkill GetEmployeeSkill(int employeeId, int skillId)
        {
            EmployeeSkill existingEmployeeSkill = _employeeSkillRepository.GetAll(row =>
                row.EmployeeId == employeeId && row.SkillId == skillId).FirstOrDefault();
            if (existingEmployeeSkill is not null)
                return existingEmployeeSkill;
            throw new GettingByInvalidIdException("EmployeeSkill does not exist for the given Employee and Skill Id's");
        }
        
        public IEnumerable<EmployeeSkill> GetEmployeeSkills(EmployeeSkillFilterDTO filter)
        {
            IQueryable<EmployeeSkill> result = _employeeSkillRepository.Include($"{nameof(Skill)}").GetAll().AsQueryable();

            if (filter?.EmployeeId is not null)
                result = result.Where(row => filter.EmployeeId.Any(id => id == row.EmployeeId));

            if (filter?.SkillIds is not null)
                result = result.Where(row => filter.SkillIds.Any(id => id ==row.SkillId));

            if (filter?.SkillName is not null)
            {
                IEnumerable<Skill> skills =
                    _skillRepository
                        .Include($"{nameof(EmployeeSkill)}.{nameof(Employee)}")
                        .GetAll(row => filter.SkillName.Contains(row.SkillName));

                result = result.Where(row => skills.Any(cert => cert.SkillId == row.SkillId));
            }

            return result;
        }

        public EmployeeSkill CreateEmployeeSkill(EmployeeSkill employeeSkill)
        {
            employeeSkill.EmployeeId = employeeSkill.EmployeeId;
            employeeSkill.SkillId = employeeSkill.SkillId;
            employeeSkill.Created = DateTime.Now.Date;
            employeeSkill.Updated = employeeSkill.Created;
            employeeSkill.CreatedBy = _context.UserName;
            employeeSkill.RowVer = Guid.NewGuid();


            var validation = _modelValidator.Validate(employeeSkill);
            if (!validation.IsValid)
            {
                throw new InvalidModelException("Model is not valid");
            }

            IEnumerable<EmployeeSkill> employeeSkills = _employeeSkillRepository.GetAll().Where(row =>
                row.EmployeeSkillId.Equals(employeeSkill.EmployeeSkillId) &&
                row.SkillId.Equals(employeeSkill.SkillId) &&
                row.EmployeeId.Equals(employeeSkill.EmployeeId));

            if (employeeSkills.Any())
                throw new CreatingDuplicateException("Duplicate Employee Skill");

            return _employeeSkillRepository.Insert(employeeSkill);
        }

        public bool UnassignEmployeeSkill(int employeeSkillId)
        {
            EmployeeSkill existingEmployeeSkill = _employeeSkillRepository.GetBy(employeeSkillId);

            if (existingEmployeeSkill is not null)
                return _employeeSkillRepository.Delete(existingEmployeeSkill);
         
            else throw new DeletingByInvalidIdException("EmployeeSkillID does not exist");
        }

        public bool UnassignEmployeeSkill(int employeeId, int SkillId)
        {
            EmployeeSkill existingEmployeeSkill = _employeeSkillRepository.GetAll().Where(
                row => row.EmployeeId == employeeId && row.SkillId == SkillId).FirstOrDefault();
            if (existingEmployeeSkill is not null)
                return _employeeSkillRepository.Delete(existingEmployeeSkill);
            else throw new DeletingByInvalidIdException("EmployeeSkill does not exist for EmployeeId and SkillId");
        }

        public EmployeeSkill UpdateEmployeeSkill(EmployeeSkill empSkill)
        {
            var validation = _modelValidator.Validate(empSkill);
            if (!validation.IsValid)
            {
                throw new InvalidModelException("Model is not valid");
            }
            return _employeeSkillRepository.Update(empSkill);
        }

        public bool DeleteEmployeeSkill(int skillId, int empId)
        {
            EmployeeSkill empSkillToDelete = _employeeSkillRepository.GetAll(row =>
                row.SkillId == skillId &&
                row.EmployeeId == empId).FirstOrDefault();

            if (empSkillToDelete is null)
            {
                throw new GettingByInvalidIdException("EmployeeSkill does not exist for the given Employee and Skill Id's");
            }

            return _employeeSkillRepository.Delete(empSkillToDelete);
        }

    }
}
