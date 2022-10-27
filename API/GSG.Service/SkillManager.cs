using GSG.Model;
using GSG.Model.DTO.Filters;
using GSG.Repository;
using GSG.Repository.Capability;
using GSG.Service.Interfaces;
using GSG.Shared.Exceptions;

namespace GSG.Service
{
    public class SkillManager : ISkillManager
    {
        public IRepository<Skill> _skillRepository;
        private readonly IIdentityContext _context;
        private readonly ModelValidator _modelValidator;

        public SkillManager(IRepository<Skill> skillRepository, IIdentityContext context, ModelValidator modelValidator)
        {
            _skillRepository = skillRepository;
            _context = context;
            _modelValidator = modelValidator;
        }

        public Skill CreateSkill(Skill skill)
        {
            var validation = _modelValidator.Validate(skill);
            if (!validation.IsValid)
            {
                throw new InvalidModelException("Model is not valid");
            }
            skill.CreatedBy = _context.UserName;
            Skill existingSkill = _skillRepository.GetAll(row => row.SkillName.Equals(skill.SkillName)).FirstOrDefault();
            if (existingSkill is null)
                return _skillRepository.Insert(skill);
            
            throw new CreatingDuplicateException("Skill already exists");
        }

        public bool DeleteSkill(int skillId)
        {
            Skill existingSkill = _skillRepository.Include($"{nameof(EmployeeSkill)}")
                .GetAll(row => row.SkillId == skillId).FirstOrDefault();
            
            if (existingSkill is not null)
            {
                if (existingSkill.EmployeeSkill.Any())
                    throw new DeletingEntityWithExistingAttachmentsException("Skill has attached EmployeeSkills");
                else
                    return _skillRepository.Delete(existingSkill);
            }
            else
                throw new DeletingByInvalidIdException("SkillId does not exist");
        }

        public IEnumerable<Skill> GetSkills(SkillFilterDTO filter = null)
        {
            if (filter?.SkillName is not null && filter.SkillName.Where(row => !string.IsNullOrWhiteSpace(row)).Any())
                return _skillRepository.GetAll(row => filter.SkillName.
                    Contains(row.SkillName));

            if (filter?.SkillId is not null)
                return _skillRepository.GetAll(row => filter.SkillId
                    .Contains(row.SkillId));

            return _skillRepository.GetAll();
        }


        public Skill GetSkill(int skillId)
        {
            Skill skill = _skillRepository.GetBy(skillId);
            if (skill is not null)
                return skill;
            else
                throw new GettingByInvalidIdException("Invalid SkillId");
        }

        public Skill UpdateSkill(Skill skill)
        {
            var validation = _modelValidator.Validate(skill);
            if (!validation.IsValid)
            {
                throw new InvalidModelException("Model is not valid");
            }
            return _skillRepository.Update(skill);
        }
    }
}
