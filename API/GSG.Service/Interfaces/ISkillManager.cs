using GSG.Model;
using GSG.Model.DTO.Filters;

namespace GSG.Service.Interfaces
{
    public interface ISkillManager
    {
        Skill CreateSkill(Skill skill);
        bool DeleteSkill(int skillId);
        IEnumerable<Skill> GetSkills(SkillFilterDTO filter);
        Skill GetSkill(int id);
        Skill UpdateSkill(Skill skill);
    }
}
