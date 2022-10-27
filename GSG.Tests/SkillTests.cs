using Autofac;
using GSG.Model;
using GSG.Repository.Capability;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using GSG.Shared.Exceptions;
using GSG.Service.Interfaces;

namespace GSG.Tests
{

    public class SkillTests : BaseServiceTest
    {

        IContainer _container;
        ISkillManager _skillManager;
        IRepository<Skill> _skillRepository;
        IRepository<EmployeeSkill> _employeeSkillRepository;

        protected override void PostSetup(IContainer container)
        {
            _skillRepository = container.Resolve<IRepository<Skill>>();
            _skillManager = container.Resolve<ISkillManager>();
            _employeeSkillRepository = container.Resolve<IRepository<EmployeeSkill>>();
        }

        [TearDown]
        public void CleanUp()
        {
            IEnumerable<Skill> trash = _skillRepository.GetAll();
            foreach (Skill skill in trash)
                _skillRepository.Delete(skill);

            IEnumerable<EmployeeSkill> trash2 = _employeeSkillRepository.GetAll();
            foreach (EmployeeSkill empskill in trash2)
                _employeeSkillRepository.Delete(empskill);
        }

        [Test]
        public void CanCreateSkill()
        {
            Skill response = _skillManager.CreateSkill(new Skill { SkillName = "test7", CreatedBy = "test6" });
            Assert.True(response is not null);
            
        }

        [Test]
        public void CanSetCreatedByOnSkill()
        {
            Skill response = _skillManager.CreateSkill(new Skill { SkillName = "test", CreatedBy = "7" });
            Assert.AreEqual(GetTestIdentityContext().UserName, response.CreatedBy);
        }

        [Test]
        public void CanDeleteSkill()
        {
            Skill inserted = _skillRepository.Insert(new Skill { SkillName = "test", CreatedBy = "test5" });
            bool exists = _skillManager.DeleteSkill(inserted.SkillId);
            Skill test = _skillRepository.GetBy(inserted.SkillId);
            Assert.IsNull(test);
            Assert.IsTrue(exists);
        }

        [Test]
        public void CanGetAllSkills()
        {
            _skillRepository.Insert(new Skill { SkillName = "test1", CreatedBy = "test1" });
            _skillRepository.Insert(new Skill { SkillName = "test2", CreatedBy = "test1" });

            IEnumerable<Skill> skills = _skillManager.GetSkills(null);
            Assert.AreEqual(2, skills.Count());
        }

        [Test]
        public void CanGetBySkillId()
        {
            String target = "gotcha";
            _skillRepository.Insert(new Skill { SkillName = "test1", CreatedBy = "test2" });
            Skill targetSkill = _skillRepository.Insert(new Skill { SkillName = target, CreatedBy = "test2" });
            _skillRepository.Insert(new Skill { SkillName = "test1", CreatedBy = "test2" });


            Skill response = _skillManager.GetSkill(targetSkill.SkillId);
            Assert.AreEqual(target,response.SkillName);
            //Assert.True(response.SkillName.Equals(target));
        }

        [Test]
        public void CanGetBySkillName()
        {
            _skillRepository.Insert(new Skill { SkillName = "test", CreatedBy = "test4" });

            IEnumerable<Skill> skills = _skillManager.GetSkills(new Model.DTO.Filters.SkillFilterDTO
            {
                SkillName = new string[] { "test" }
            });

            Assert.IsTrue(skills is not null);
            Assert.AreEqual(1, skills.Count());
        }

        [Test]
        public void CanGetMultipleSkillsBySkillId()
        {
            _skillRepository.Insert(new Skill { SkillName = "test", CreatedBy = "test6" });
            Skill target1 = _skillRepository.Insert(new Skill { SkillName = "test1", CreatedBy = "test6" });
            Skill target2 = _skillRepository.Insert(new Skill { SkillName = "test2", CreatedBy = "test6" });
            _skillRepository.Insert(new Skill { SkillName = "test3", CreatedBy = "test6" });

            IEnumerable<Skill> response = _skillManager.GetSkills(
                    new Model.DTO.Filters.SkillFilterDTO { SkillId = new int[] { target1.SkillId, target2.SkillId } });

            Assert.IsTrue(response.Count() is 2);
        }

        [Test]
        public void CannotCreateSkillWithDuplicateName()
        {
            _skillRepository.Insert(new Skill { SkillName = "test3", CreatedBy = "test3" });
            try
            {
                _skillManager.CreateSkill(new Skill { SkillName = "test3", CreatedBy = "test3" });
            }
            catch (CreatingDuplicateException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CannotDeleteSkillByInvalidSkillId()
        {
            Skill inserted = _skillRepository.Insert(new Skill { SkillName = "test", CreatedBy = "test6" });

            try
            {
                _skillManager.DeleteSkill(inserted.SkillId + 1);
            }
            catch (DeletingByInvalidIdException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CannotDeleteSkillWithEmployeeSkill()
        {
            Skill skill = _skillRepository.Insert(new Skill { SkillName = "test", CreatedBy = "test8" });
            EmployeeSkill empSkill = _employeeSkillRepository.Insert(new EmployeeSkill
            {
                SkillId = skill.SkillId,
                EmployeeId = 1,
                CreatedBy = "HR"
            });

            try
            {
                _skillManager.DeleteSkill(skill.SkillId);
            }
            catch (DeletingEntityWithExistingAttachmentsException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CannotGetSkillByInvalidSkillId()
        {

            Skill inserted = new Skill { SkillName = "test", CreatedBy = "test7" };
            try
            {
                _skillManager.GetSkill(inserted.SkillId + 1);
            }
            catch (GettingByInvalidIdException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CanUpdateSkill()
        {
            Skill skill = _skillRepository.Insert(new Skill { SkillName = "test", CreatedBy = "test8" });
            String target = "Success";
            skill.SkillName = target;
            _skillManager.UpdateSkill(skill);
            Assert.True(_skillRepository.GetBy(skill.SkillId).SkillName.Equals(target));
        }
    }
}
