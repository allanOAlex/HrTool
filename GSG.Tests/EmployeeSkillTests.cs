using Autofac;
using GSG.Model;
using GSG.Repository.Capability;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using GSG.Model.DTO.Filters;
using GSG.Shared.Exceptions;
using GSG.Service.Interfaces;

namespace GSG.Tests
{
    public class EmployeeSkillTests : BaseServiceTest
    {

        IContainer _container;
        IEmployeeSkillManager _employeeSkillManager;
        IRepository<Skill> _skillRepository;
        IRepository<EmployeeSkill> _employeeSkillRepository;
        IRepository<Employee> _employeeRepository;

        public bool repositoriesCreated = false;
        protected override void PostSetup(IContainer container)
        {
            _employeeSkillRepository = container.Resolve<IRepository<EmployeeSkill>>();
            _employeeSkillManager = container.Resolve<IEmployeeSkillManager>();

            _employeeRepository = container.Resolve<IRepository<Employee>>();
            _skillRepository = container.Resolve<IRepository<Skill>>();
        }

        [TearDown]
        public void Clean()
        {
            var trash = _employeeRepository.GetAll();
            foreach (Employee emp in trash)
                _employeeRepository.Delete(emp);

            var trash2 = _skillRepository.GetAll();
            foreach (Skill skill in trash2)
                _skillRepository.Delete(skill);

            var trash3 = _employeeSkillRepository.GetAll();
            foreach (EmployeeSkill empSkill in trash3)
                _employeeSkillRepository.Delete(empSkill);
        }

        public Skill GenericSkill()
        {
            Skill response = new Skill
            {
                SkillName = "Pro",
                CreatedBy = "Hr"
            };
            return response;
        }

        public Employee GenericEmployee()
        {
            Employee response = new Employee
            {
                FirstName = "John",
                LastName = "Smith",
                Address = "Nunya",
                Address2 = "Bee's Knees",
                City = "Mars Vegas",
                Email = "gmail",
                EmployeeState = "Georgia",
                PictureUrl = "image",
                PhoneNumber = "911",
                Zip = "it",
                CreatedBy = "a fool"
            };
            return response;
        }

        [Test]
        public void CanAssignSkillToEmployee()
        {
            EmployeeSkill employeeSkill = new EmployeeSkill
            {
                EmployeeId = 1,
                SkillId = 1,
                CreatedBy = "a fool"
            };
            EmployeeSkill returnValue = _employeeSkillManager.AssignEmployeeSkill(employeeSkill);
            EmployeeSkill response = _employeeSkillRepository.GetAll().FirstOrDefault();
            Assert.True(response is not null);
        }

        [Test]
        public void CanSetCreatedByOnEmployeeSkill()
        {
            EmployeeSkill employeeSkill = new EmployeeSkill
            {
                EmployeeId = 1,
                SkillId = 1,
                CreatedBy = GetTestIdentityContext().UserName
            };
            EmployeeSkill returnValue = _employeeSkillManager.AssignEmployeeSkill(employeeSkill);
            EmployeeSkill response = _employeeSkillRepository.GetAll().FirstOrDefault();
            Assert.AreEqual(GetTestIdentityContext().UserName, response.CreatedBy);
        }

        [Test]
        public void CanUnassignSkillFromEmployee()
        {
            EmployeeSkill employeeSkill = new EmployeeSkill
            {
                EmployeeId = 1,
                SkillId = 1,
                CreatedBy = "a second fool"
            };
            EmployeeSkill inserted = _employeeSkillRepository.Insert(employeeSkill);
            _employeeSkillManager.UnassignEmployeeSkill(inserted.EmployeeSkillId);
            IEnumerable<EmployeeSkill> employeeSkills = _employeeSkillRepository.GetAll();
            Assert.True(employeeSkills.Count() == 0);
        }

        [Test]
        public void CannotUnassignInvalidEmployeeSkillFromEmployee()
        {
            EmployeeSkill employeeSkill = new EmployeeSkill
            {
                EmployeeId = 1,
                SkillId = 1,
                CreatedBy = "a third fool"
            };

            EmployeeSkill response = _employeeSkillRepository.Insert(employeeSkill);
            try
            {
                _employeeSkillManager.UnassignEmployeeSkill(response.EmployeeSkillId + 1);
            }
            catch (DeletingByInvalidIdException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CanGetEmployeeSkillByEmployeeSkillId()
        {
            EmployeeSkill employeeSkill = new EmployeeSkill
            {
                EmployeeId = 1,
                SkillId = 1,
                CreatedBy = "a fourth fool"
            };
            EmployeeSkill inserted = _employeeSkillRepository.Insert(employeeSkill);
            EmployeeSkill response = _employeeSkillManager.GetEmployeeSkill(inserted.EmployeeSkillId);
            Assert.IsNotNull(response);
        }

        [Test]
        public void CannotGetByInvalidEmployeeSkillId()
        {
            EmployeeSkill employeeSkill = new EmployeeSkill
            {
                EmployeeId = 1,
                SkillId = 1,
                CreatedBy = "a fifth fool"
            };
            EmployeeSkill inserted = _employeeSkillRepository.Insert(employeeSkill);
            try
            {
                _employeeSkillManager.GetEmployeeSkill(inserted.EmployeeSkillId + 1);
            }
            catch (GettingByInvalidIdException)
            {
                Assert.Pass();
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void CannotAssignDuplicateSkillToEmployee()
        {
            EmployeeSkill entry1 = new EmployeeSkill
            {
                EmployeeId = 1,
                SkillId = 1,
                CreatedBy = "a sixth fool"
            };
            EmployeeSkill entry2 = new EmployeeSkill
            {
                EmployeeId = 1,
                SkillId = 1,
                CreatedBy = "a sixth fool"
            };
            _employeeSkillRepository.Insert(entry1);
            try
            {
                _employeeSkillManager.AssignEmployeeSkill(entry2);
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CanGetEmployeeSkillByEmployeeIdAndSkillId()
        {
            EmployeeSkill entry = _employeeSkillRepository.Insert(new EmployeeSkill
            {
                EmployeeId = 1,
                SkillId = 1,
                CreatedBy = "a seventh fool"
            });

            EmployeeSkill Response = _employeeSkillManager.GetEmployeeSkill(entry.EmployeeId, entry.SkillId);
            Assert.True(
                Response.EmployeeId == entry.EmployeeId &&
                Response.SkillId == entry.SkillId &&
                Response.EmployeeSkillId == entry.EmployeeSkillId &&
                Response.CreatedBy == entry.CreatedBy
                );
        }

        [Test]
        public void CanGetMultipleEmployeeSkillsBySkillId()
        {
            for (int i = 0; i < 3; i++)
            {
                _employeeRepository.Insert(GenericEmployee());
                _skillRepository.Insert(GenericSkill());
            }
            for (int i = 1; i <= 4; i++)
            {
                _employeeSkillRepository.Insert(new EmployeeSkill
                {
                    EmployeeId = i,
                    SkillId = i % 2,
                    CreatedBy = "an eighth fool"
                });
            }

            EmployeeSkillFilterDTO filter = new EmployeeSkillFilterDTO { SkillIds = new int[] { 1 } };
            IEnumerable<EmployeeSkill> response = _employeeSkillManager.GetEmployeeSkills(filter);
            Assert.True(response.Count() == 2);
        }

        [Test]
        public void CanGetMultipleEmployeeSkillsByMultipleSkillIds()
        {
            for (int i = 0; i < 3; i++)
            {
                _employeeRepository.Insert(GenericEmployee());
            }
            _skillRepository.Insert(GenericSkill());
            _skillRepository.Insert(GenericSkill());
            EmployeeSkill empCert;
            for (int i = 1; i <= 4; i++)
            {
                empCert = _employeeSkillRepository.Insert(new EmployeeSkill
                {
                    EmployeeId = i,
                    SkillId = 1 + i % 2,
                    CreatedBy = "an eighth fool"
                });
            }

            EmployeeSkillFilterDTO filter = new EmployeeSkillFilterDTO { SkillIds = new int[] { 1, 2 } };
            IEnumerable<EmployeeSkill> response = _employeeSkillManager.GetEmployeeSkills(filter);
            int count = response.Count();
            var all = _employeeSkillRepository.GetAll();
            var skills = _skillRepository.GetAll();
            if (count is not 4)
            {
                Assert.Fail();
            }
            Assert.True(response.Count() == 4);

        }

        [Test]
        public void CanGetMultipleEmployeeSkillsByMultipleEmployeeIds()
        {
            for (int i = 0; i < 3; i++)
                _employeeRepository.Insert(GenericEmployee());
            for (int i = 1; i <= 4; i++)
            {
                _employeeSkillRepository.Insert(new EmployeeSkill
                {
                    EmployeeId = i % 2 + 27,
                    SkillId = 1,
                    CreatedBy = "a fool"
                });
            }
            _skillRepository.Insert(GenericSkill());
            _skillRepository.Insert(GenericSkill());
            _employeeSkillRepository.Insert(new EmployeeSkill { EmployeeId = 26, SkillId = 2, CreatedBy = "debug" });
            _employeeSkillRepository.Insert(new EmployeeSkill { EmployeeId = 29, SkillId = 3, CreatedBy = "debug" });

            EmployeeSkillFilterDTO filter = new EmployeeSkillFilterDTO { EmployeeId = new int[] { 27, 28 } };
            IEnumerable<EmployeeSkill> response = _employeeSkillManager.GetEmployeeSkills(filter);
            int count = response.Count();

            if (response.Any(row => row.SkillId == 2))
                Assert.Fail("miss by -1");
            if (response.Any(row => row.SkillId == 3))
                Assert.Fail("miss by +1");

            if (response.Count() == 4)
                Assert.Pass();
            else
                Assert.Fail("got " + response.Count() + " responses");
        }

        [Test]
        public void CanGetMultipleSkillsBySkillName()
        {
            _employeeRepository.Insert(GenericEmployee());
            IEnumerable<Employee> employees = _employeeRepository.GetAll();
            _skillRepository.Insert(GenericSkill());
            Skill target = _skillRepository.Insert(new Skill { SkillName = "target", CreatedBy = "a debugger" });

            IEnumerable<Skill> skills = _skillRepository.GetAll();
            foreach (Skill ski in skills)
                for (int i = 1; i < 4; i++)
                    foreach (Employee employee in employees)
                        _employeeSkillRepository.Insert(
                            new EmployeeSkill
                            {
                                EmployeeId = employee.EmployeeId,
                                SkillId = ski.SkillId,
                                CreatedBy = "HR"
                            });

            EmployeeSkillFilterDTO filter = new EmployeeSkillFilterDTO { SkillName = new string[] { "target" } };
            IEnumerable<EmployeeSkill> response = _employeeSkillManager.GetEmployeeSkills(filter);
            if (response.All(row => row.SkillId == target.SkillId))
                Assert.Pass();
            else
                Assert.Fail("got " + response.Where(row => row.SkillId != target.SkillId).Count() + " incorrect responses");
        }


        public Employee GenericEmployee2()
        {
            Employee response = new Employee
            {
                FirstName = "Jane",
                LastName = "Doe",
                Address = "Nunya",
                Address2 = "Bee's Knees",
                City = "Mars Vegas",
                Email = "gmail",
                EmployeeState = "Texas",
                PictureUrl = "image",
                PhoneNumber = "911",
                Zip = "it",
                CreatedBy = "a fool"
            };
            return response;
        }
        public Employee GenericEmployee3()
        {
            Employee response = new Employee
            {
                FirstName = "Charles",
                LastName = "Schwab",
                Address = "Nunya",
                Address2 = "Bee's Knees",
                City = "Mars Vegas",
                Email = "gmail",
                EmployeeState = "Florida",
                PictureUrl = "image",
                PhoneNumber = "911",
                Zip = "it",
                CreatedBy = "a fool"
            };
            return response;
        }
        [Test]
        public void CanFilterMultipleParameters()
        {
            Skill ski1 = _skillRepository.Insert(GenericSkill());

            Skill target = GenericSkill();
            target.SkillName = "target";

            Skill ski2 = _skillRepository.Insert(target);
            Skill ski3 = _skillRepository.Insert(GenericSkill());
            Employee emp1 = _employeeRepository.Insert(GenericEmployee());
            Employee emp2 = _employeeRepository.Insert(GenericEmployee2());
            Employee emp3 = _employeeRepository.Insert(GenericEmployee3());
            _employeeSkillRepository.Insert(new EmployeeSkill
            {
                EmployeeId = emp1.EmployeeId
                    ,
                SkillId = ski1.SkillId,
                CreatedBy = "debug"
            });
            _employeeSkillRepository.Insert(new EmployeeSkill
            {
                EmployeeId = emp1.EmployeeId
                    ,
                SkillId = ski2.SkillId,
                CreatedBy = "debug"
            });
            _employeeSkillRepository.Insert(new EmployeeSkill
            {
                EmployeeId = emp3.EmployeeId
                    ,
                SkillId = ski3.SkillId,
                CreatedBy = "debug"
            });

            EmployeeSkillFilterDTO filter = new EmployeeSkillFilterDTO
            {
                SkillName = new string[] { "target" }
            ,
                SkillIds = new int[] { 2, 3 }
            ,
                EmployeeId = new int[] { 1 }
            };
            IEnumerable<EmployeeSkill> response = _employeeSkillManager.GetEmployeeSkills(filter);

            if (response.Count() == 1)
                Assert.Pass();
            Assert.Fail();
        }

        [Test]
        public void CanUpdateEmployeeSkill()
        {
            EmployeeSkill employeeSkill = _employeeSkillRepository.Insert(new EmployeeSkill { SkillId = 1, EmployeeId = 1, CreatedBy = "test8" });
            String target = "Success";
            employeeSkill.CreatedBy = target;
            _employeeSkillManager.UpdateEmployeeSkill(employeeSkill);
            Assert.True(_employeeSkillRepository.GetBy(employeeSkill.EmployeeSkillId).CreatedBy.Equals(target));
        }
    }
}