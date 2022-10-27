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
    public class EmployeeProjectTests : BaseServiceTest
    {
        IContainer _container;
        IEmployeeProjectManager _employeeProjectManager;
        IRepository<Project> _projectRepository;
        IRepository<EmployeeProject> _employeeProjectRepository;
        IRepository<Employee> _employeeRepository;

        public bool repositoriesCreated = false;
        protected override void PostSetup(IContainer container)
        {
            // employee project objects
            _employeeProjectRepository = container.Resolve<IRepository<EmployeeProject>>();
            _employeeProjectManager = container.Resolve<IEmployeeProjectManager>();

            // extra repositories
            _employeeRepository = container.Resolve<IRepository<Employee>>();
            _projectRepository = container.Resolve<IRepository<Project>>();
        }

        [TearDown]
        public void CleanUp()
        {
            IEnumerable<Employee> employees = _employeeRepository.GetAll();
            IEnumerable<Project> projects = _projectRepository.GetAll();
            IEnumerable<EmployeeProject> employeeProjects = _employeeProjectRepository.GetAll();

            foreach (Employee emp in employees)
                _employeeRepository.Delete(emp);
            foreach (Project pro in projects)
                _projectRepository.Delete(pro);
            foreach (EmployeeProject empPro in employeeProjects)
                _employeeProjectRepository.Delete(empPro);
        }

        private Employee GenericEmployee()
        {
            Employee response = new Employee
            {
                FirstName = "John",
                LastName = "Doe",
                PhoneNumber = "911",
                PictureUrl = "google.com",
                City = "Smyrna",
                EmployeeState = "Georgia",
                CreatedBy = "HR",
                Email = "JDoe@gmail.com",
                Address = "123 road",
                Address2 = "apartment 456",
                Zip = "55555"
            };
            return response;
        }

        private Project GenericProject()
        {
            Project response = new Project
            {
                ProjectName = "Generic Project",
                StartDate = DateTime.Now,
                CreatedBy = "HR"
            };
            return response;
        }

        [Test]
        public void CanAssignProjectToEmployee()
        {
            EmployeeProject employeeProject = new EmployeeProject
            {
                EmployeeId = 1,
                ProjectId = 1,
                CreatedBy = "a fool"
            };
            EmployeeProject returnValue = _employeeProjectManager.AssignEmployeeProject(employeeProject);
            EmployeeProject response = _employeeProjectRepository.GetAll().FirstOrDefault();
            Assert.True(response is not null);
        }

        [Test]
        public void CanSetCreatedByOnEmployeeProject()
        {
            EmployeeProject employeeProject = new EmployeeProject
            {
                EmployeeId = 1,
                ProjectId = 1,
                CreatedBy = GetTestIdentityContext().UserName
            };
            EmployeeProject returnValue = _employeeProjectManager.AssignEmployeeProject(employeeProject);
            EmployeeProject response = _employeeProjectRepository.GetAll().FirstOrDefault();
            Assert.AreEqual(GetTestIdentityContext().UserName, response.CreatedBy);
        }

        [Test]
        public void CanUnassignProjectFromEmployee()
        {
            EmployeeProject employeeProject = new EmployeeProject
            {
                EmployeeId = 1,
                ProjectId = 1,
                CreatedBy = "a second fool"
            };
            EmployeeProject inserted = _employeeProjectRepository.Insert(employeeProject);
            _employeeProjectManager.UnassignEmployeeProject(inserted.EmployeeProjectId);
            IEnumerable<EmployeeProject> employeeProjects = _employeeProjectRepository.GetAll();
            Assert.True(employeeProjects.Count() == 0);
        }

        [Test]
        public void CannotUnassignInvalidEmployeeProjectFromEmployee()
        {
            EmployeeProject employeeProject = new EmployeeProject
            {
                EmployeeId = 1,
                ProjectId = 1,
                CreatedBy = "a third fool"
            };

            EmployeeProject response = _employeeProjectRepository.Insert(employeeProject);
            try
            {
                _employeeProjectManager.UnassignEmployeeProject(response.EmployeeProjectId + 1);
            }
            catch (DeletingByInvalidIdException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CanGetEmployeeProjectByEmployeeProjectId()
        {
            EmployeeProject employeeProject = new EmployeeProject
            {
                EmployeeId = 1,
                ProjectId = 1,
                CreatedBy = "a fourth fool"
            };
            EmployeeProject inserted = _employeeProjectRepository.Insert(employeeProject);
            EmployeeProject response = _employeeProjectManager.GetEmployeeProject(inserted.EmployeeProjectId);
            Assert.IsNotNull(response);
        }

        [Test]
        public void CannotGetByInvalidEmployeeProjectId()
        {
            EmployeeProject employeeProject = new EmployeeProject
            {
                EmployeeId = 1,
                ProjectId = 1,
                CreatedBy = "a fifth fool"
            };
            EmployeeProject inserted = _employeeProjectRepository.Insert(employeeProject);
            try
            {
                _employeeProjectManager.GetEmployeeProject(inserted.EmployeeProjectId + 1);
            }
            catch (GettingByInvalidIdException)
            {
                Assert.Pass();
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void CannotAssignDuplicateProjectToEmployee()
        {
            EmployeeProject entry1 = new EmployeeProject
            {
                EmployeeId = 1,
                ProjectId = 1,
                CreatedBy = "a sixth fool"
            };
            EmployeeProject entry2 = new EmployeeProject
            {
                EmployeeId = 1,
                ProjectId = 1,
                CreatedBy = "a sixth fool"
            };
            _employeeProjectRepository.Insert(entry1);
            try
            {
                _employeeProjectManager.AssignEmployeeProject(entry2);
            }
            catch (Exception)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CanGetEmployeeProjectByEmployeeIdAndProjectId()
        {
            EmployeeProject entry = _employeeProjectRepository.Insert(new EmployeeProject
            {
                EmployeeId = 1,
                ProjectId = 1,
                CreatedBy = "a seventh fool"
            });

            EmployeeProject Response = _employeeProjectManager.GetEmployeeProject(entry.EmployeeId, entry.ProjectId);
            Assert.True(
                Response.EmployeeId == entry.EmployeeId &&
                Response.ProjectId == entry.ProjectId &&
                Response.EmployeeProjectId == entry.EmployeeProjectId &&
                Response.CreatedBy == entry.CreatedBy
                );
        }

        [Test]
        public void CanGetMultipleEmployeeProjectsByProjectId()
        {
            for (int i = 0; i < 3; i++)
            {
                _employeeRepository.Insert(GenericEmployee());
                _projectRepository.Insert(GenericProject());
            }
            for (int i = 1; i <= 4; i++)
            {
                _employeeProjectRepository.Insert(new EmployeeProject
                {
                    EmployeeId = 1 + i%2,
                    ProjectId = 1 + i%2,
                    CreatedBy = "an eighth fool"
                });
            }

            EmployeeProjectFilterDTO filter = new EmployeeProjectFilterDTO { ProjectId = new int[] { 1 } };
            IEnumerable<EmployeeProject> response = _employeeProjectManager.GetEmployeeProjects(filter);
            Assert.True(response.Count() == 2);
        }

        [Test]
        public void CanGetMultipleEmployeeProjectsByMultipleProjectIds()
        {
            for (int i = 0; i < 3; i++)
            {
                _employeeRepository.Insert(GenericEmployee());
            }
            _projectRepository.Insert(GenericProject());
            _projectRepository.Insert(GenericProject());
            EmployeeProject empProj;
            for (int i = 1; i <= 4; i++)
            {
                empProj = _employeeProjectRepository.Insert(new EmployeeProject
                {
                    EmployeeId = i,
                    ProjectId = 1 + i % 2,
                    CreatedBy = "an eighth fool"
                });
            }

            EmployeeProjectFilterDTO filter = new EmployeeProjectFilterDTO { ProjectId = new int[] { 1, 2 } };
            IEnumerable<EmployeeProject> response = _employeeProjectManager.GetEmployeeProjects(filter);
            int count = response.Count();
            var all = _employeeProjectRepository.GetAll();
            var certificates = _projectRepository.GetAll();
            if (count is not 4)
            {
                Assert.Fail();
            }
            Assert.True(response.Count() == 4);

        }

        [Test]
        public void CanGetMultipleEmployeeProjectsByMultipleEmployeeIds()
        {
            for (int i = 0; i < 3; i++)
                _employeeRepository.Insert(GenericEmployee());
            for (int i = 1; i <= 4; i++)
            {
                _employeeProjectRepository.Insert(new EmployeeProject
                {
                    EmployeeId = i % 2 + 27,
                    ProjectId = 1,
                    CreatedBy = "a fool"
                });
            }
            _projectRepository.Insert(GenericProject());
            _projectRepository.Insert(GenericProject());
            _employeeProjectRepository.Insert(new EmployeeProject { EmployeeId = 26, ProjectId = 2, CreatedBy = "debug" });
            _employeeProjectRepository.Insert(new EmployeeProject { EmployeeId = 29, ProjectId = 3, CreatedBy = "debug" });

            EmployeeProjectFilterDTO filter = new EmployeeProjectFilterDTO { EmployeeId = new int[] { 27, 28 } };
            IEnumerable<EmployeeProject> response = _employeeProjectManager.GetEmployeeProjects(filter);
            int count = response.Count();

            if (response.Any(row => row.ProjectId == 2))
                Assert.Fail("miss by -1");
            if (response.Any(row => row.ProjectId == 3))
                Assert.Fail("miss by +1");

            if (response.Count() == 4)
                Assert.Pass();
            else
                Assert.Fail("got " + response.Count() + " responses");
        }

        [Test]
        public void CanGetMultipleProjectsByProjectName()
        {
            _employeeRepository.Insert(GenericEmployee());
            IEnumerable<Employee> employees = _employeeRepository.GetAll();
            _projectRepository.Insert(GenericProject());
            Project target = _projectRepository.Insert(new Project { ProjectName = "target", CreatedBy = "a debugger" });

            IEnumerable<Project> projects = _projectRepository.GetAll();
            foreach (Project proj in projects)
                for (int i = 1; i < 4; i++)
                    foreach (Employee employee in employees)
                        _employeeProjectRepository.Insert(
                            new EmployeeProject
                            {
                                EmployeeId = employee.EmployeeId,
                                ProjectId = proj.ProjectId,
                                CreatedBy = "HR"
                            });

            EmployeeProjectFilterDTO filter = new EmployeeProjectFilterDTO { ProjectName = new string[] { "target" } };
            IEnumerable<EmployeeProject> response = _employeeProjectManager.GetEmployeeProjects(filter);
            if (response.All(row => row.ProjectId == target.ProjectId))
                Assert.Pass();
            else
                Assert.Fail("got " + response.Where(row => row.ProjectId != target.ProjectId).Count() + " incorrect responses");
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
            Project proj1 = _projectRepository.Insert(GenericProject());

            Project target = GenericProject();
            target.ProjectName = "target";

            Project proj2 = _projectRepository.Insert(target);
            Project proj3 = _projectRepository.Insert(GenericProject());
            Employee emp1 = _employeeRepository.Insert(GenericEmployee());
            Employee emp2 = _employeeRepository.Insert(GenericEmployee2());
            Employee emp3 = _employeeRepository.Insert(GenericEmployee3());
            _employeeProjectRepository.Insert(new EmployeeProject
            {
                EmployeeId = emp1.EmployeeId
                    ,
                ProjectId = proj1.ProjectId,
                CreatedBy = "debug"
            });
            _employeeProjectRepository.Insert(new EmployeeProject
            {
                EmployeeId = emp1.EmployeeId
                    ,
                ProjectId = proj2.ProjectId,
                CreatedBy = "debug"
            });
            _employeeProjectRepository.Insert(new EmployeeProject
            {
                EmployeeId = emp3.EmployeeId
                    ,
                ProjectId = proj3.ProjectId,
                CreatedBy = "debug"
            });

            EmployeeProjectFilterDTO filter = new EmployeeProjectFilterDTO
            {
                ProjectName = new string[] { "target" }
            ,
                ProjectId = new int[] { 2, 3 }
            ,
                EmployeeId = new int[] { 1 }
            };
            IEnumerable<EmployeeProject> response = _employeeProjectManager.GetEmployeeProjects(filter);

            if (response.Count() == 1)
                Assert.Pass();
            Assert.Fail();
        }

        [Test]
        public void CanGetEmployeeProjectTime()
        {
            EmployeeProject inserted1 = _employeeProjectRepository.Insert(new EmployeeProject
            {
                CreatedBy = "test7",
                StartDate = new DateTime(2015, 7, 15),
                EndDate = new DateTime(2015, 7, 15)
            });EmployeeProject inserted2 = _employeeProjectRepository.Insert(new EmployeeProject
            {
                CreatedBy = "test7",
                StartDate = new DateTime(2016, 7, 15),
                EndDate = new DateTime(2016, 7, 15)
            });EmployeeProject inserted3 = _employeeProjectRepository.Insert(new EmployeeProject
            {
                CreatedBy = "test7",
                StartDate = new DateTime(2017, 7, 15),
                EndDate = new DateTime(2017, 7, 15)
            });EmployeeProject inserted4 = _employeeProjectRepository.Insert(new EmployeeProject
            {
                CreatedBy = "test7",
                StartDate = new DateTime(2018, 7, 15),
                EndDate = new DateTime(2018, 7, 15)
            });

            EmployeeProjectFilterDTO filter1 = new EmployeeProjectFilterDTO { From = new DateTime(2016, 1, 1) };
            EmployeeProjectFilterDTO filter2 = new EmployeeProjectFilterDTO
            {
                From = new DateTime(2016, 1, 1),
                To = new DateTime(2018, 1, 1)
            };
            EmployeeProjectFilterDTO filter3 = new EmployeeProjectFilterDTO { To = new DateTime(2018, 1, 1) };

            IEnumerable<EmployeeProject> results1 = _employeeProjectManager.GetEmployeeProjects(filter1);
            Assert.AreEqual(0, results1.Count());

            IEnumerable<EmployeeProject> results2 = _employeeProjectManager.GetEmployeeProjects(filter2);
            int count = results2.Count();
            Assert.AreEqual(0, count);

            IEnumerable<EmployeeProject> results3 = _employeeProjectManager.GetEmployeeProjects(filter3);
            Assert.AreEqual(0, results3.Count());
        }

        [Test]
        public void CanUpdateProject()
        {
            EmployeeProject project = _employeeProjectRepository.Insert(
                new EmployeeProject { ProjectId = 1, CreatedBy = "test8", EmployeeId = 1 });
            String target = "Success";
            project.CreatedBy = target;
            _employeeProjectManager.UpdateEmployeeProject(project);
            Assert.True(_employeeProjectRepository.GetBy(project.EmployeeProjectId).CreatedBy.Equals(target));
        }
    }
}
