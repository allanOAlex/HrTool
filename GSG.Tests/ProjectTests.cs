using System;
using System.Collections.Generic;
using System.Linq;
using GSG.Model;
using Autofac;
using GSG.Repository.Capability;
using NUnit.Framework;
using GSG.Model.DTO.Filters;
using GSG.Shared.Exceptions;
using GSG.Service.Interfaces;

namespace GSG.Tests
{
    public class ProjectTests : BaseServiceTest
    {
        IContainer _container;
        IProjectManager _projectManager;
        IRepository<Project> _projectRepository;
        IRepository<EmployeeProject> _employeeProjectRepository;

        protected override void PostSetup(IContainer container)
        {
            _projectRepository = container.Resolve<IRepository<Project>>();
            _projectManager = container.Resolve<IProjectManager>();
            _employeeProjectRepository = container.Resolve<IRepository<EmployeeProject>>();
        }

        [TearDown]
        public void ClearRepository()
        {
            IEnumerable<Project> trash = _projectRepository.GetAll();
            foreach(Project Project in trash)
                _projectRepository.Delete(Project);

            IEnumerable<EmployeeProject> employeeProjects = _employeeProjectRepository.GetAll();
            foreach (EmployeeProject empPro in employeeProjects)
                _employeeProjectRepository.Delete(empPro);
        }

        [Test]
        public void CanCreateProject()
        {
            Project project = new Project { ProjectName = "test7", CreatedBy = "test6" };
            Project response = _projectManager.CreateProject(project);
            Assert.True(response is not null);
        }

        [Test]
        public void CanSetCreatedByOnCreateProject()
        {
            Project response = _projectManager.CreateProject(new Project { ProjectName = "test7", CreatedBy = "6" });

            Assert.AreEqual(GetTestIdentityContext().UserName, response.CreatedBy);
        }

        [Test]
        public void CanGetAllProjects()
        {
            _projectRepository.Insert(new Project { ProjectName = "test1", CreatedBy = "test1" });
            _projectRepository.Insert(new Project { ProjectName = "test2", CreatedBy = "test1" });

            IEnumerable<Project> projects = _projectManager.GetProjects(null);
            Assert.AreEqual(2, projects.Count());
        }
        [Test]
        public void CanGetByProjectId()
        {
            String target = "gotcha";
            Project inserted1 = _projectRepository.Insert(new Project { ProjectName = "test1", CreatedBy = "test2" });
            Project inserted2 = _projectRepository.Insert(new Project { ProjectName = target, CreatedBy = "test2" });
            Project inserted3 = _projectRepository.Insert(new Project { ProjectName = "test1", CreatedBy = "test2" });
            Project response = _projectManager.GetProject(inserted2.ProjectId);
            Assert.True(response.ProjectName.Equals(target));
        }
        [Test]
        public void CannotCreateProjectWithDuplicateName()
        {
            _projectRepository.Insert(new Project { ProjectName = "test3", CreatedBy = "test3" });
            try
            {
                _projectManager.CreateProject(new Project { ProjectName = "test3", CreatedBy = "test3" });
            }
            catch (CreatingDuplicateException)
            {
                Assert.Pass();
                return;
            }
            Assert.Fail();
        }

        [Test]
        public void CanGetByProjectName()
        {
            _projectRepository.Insert(new Project { ProjectName = "test", CreatedBy = "test4" });

            var projects = _projectManager.GetProjects(new Model.DTO.Filters.ProjectFilterDTO
            {
                ProjectName = new string[] { "test" }
            });

            Assert.IsTrue(projects is not null);
            Assert.AreEqual(1, projects.Count());
        }
        [Test]
        public void DeleteProject()
        {
            Project project = new Project { ProjectName = "test", CreatedBy = "test5" };
            Project inserted = _projectRepository.Insert(project);
            bool exists = _projectManager.DeleteProject(inserted.ProjectId);
            Project test = _projectRepository.GetBy(inserted.ProjectId);
            Assert.IsNull(test);
            Assert.IsTrue(exists);
        }
        [Test]
        public void CannotDeleteProjectByInvalidProjectId()
        {
            Project inserted = _projectRepository.Insert(new Project { ProjectName = "test", CreatedBy = "test6" });
            try
            {
                _projectManager.DeleteProject(inserted.ProjectId + 1);
            } catch(DeletingByInvalidIdException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void GetMultipleProjectsByProjectId()
        {
            _projectRepository.Insert(new Project { ProjectName = "test", CreatedBy = "test6" });
            _projectRepository.Insert(new Project { ProjectName = "test1", CreatedBy = "test6" });
            _projectRepository.Insert(new Project { ProjectName = "test2", CreatedBy = "test6" });
            _projectRepository.Insert(new Project { ProjectName = "test3", CreatedBy = "test6" });

            IEnumerable<Project> projects = _projectRepository.GetAll();
            Project[] list = projects.ToArray();
            int[] ids = new int[list.Length];

            for (int i = 0; i < list.Length; i++)
            {
                ids[i] = list[i].ProjectId;
            }
            IEnumerable<Project> response = _projectManager.GetProjects(
                    new Model.DTO.Filters.ProjectFilterDTO { ProjectId = ids });

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Count() is 4);
        }

        [Test]
        public void CannotGetProjectByInvalidProjectId()
        {
            Project inserted = _projectRepository.Insert(new Project { ProjectName = "test", CreatedBy = "test7" });
            try
            {
                _projectManager.GetProject(inserted.ProjectId + 1);
            } catch (DeletingByInvalidIdException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CanGetProjectByTime()
        {
            Project inserted1 = _projectRepository.Insert(new Project
            {
                ProjectName = "test",
                CreatedBy = "test7",
                StartDate = new DateTime(2015, 7, 15),
                EndDate = new DateTime(2015, 7, 15)
            });

            Project inserted2 = _projectRepository.Insert(new Project
            {
                ProjectName = "test",
                CreatedBy = "test7",
                StartDate = new DateTime(2016, 7, 15),
                EndDate = new DateTime(2016,7,15)
            });

            Project inserted3 = _projectRepository.Insert(new Project
            {
                ProjectName = "test",
                CreatedBy = "test7",
                StartDate = new DateTime(2017, 7, 15),
                EndDate = new DateTime(2017,7,15)
            });
            Project inserted4 = _projectRepository.Insert(new Project
            {
                ProjectName = "test",
                CreatedBy = "test7",
                StartDate = new DateTime(2018, 7, 15),
                EndDate = new DateTime(2018,7,15)
            });
            ProjectFilterDTO filter1 = new ProjectFilterDTO{ From = new DateTime(2016,1,1) };
            ProjectFilterDTO filter2 = new ProjectFilterDTO { From = new DateTime(2016,1,1) 
                , To = new DateTime(2018,1,1)};
            ProjectFilterDTO filter3 = new ProjectFilterDTO { To = new DateTime(2018,1,1) };

            IEnumerable<Project> results1 = _projectManager.GetProjects(filter1);
            Assert.AreEqual(3, results1.Count());

            IEnumerable<Project> results2 = _projectManager.GetProjects(filter2);
            int count = results2.Count();
            Assert.AreEqual(2, count);
            
            IEnumerable<Project> results3 = _projectManager.GetProjects(filter3);
            Assert.AreEqual(3, results3.Count());
        }

        [Test]
        public void CannotDeleteProjectWithEmployeeProject()
        {
            Project project = _projectRepository.Insert(new Project { ProjectName = "test", CreatedBy = "HR" });
            _employeeProjectRepository.Insert(new EmployeeProject
            {
                EmployeeId = 1,
                ProjectId = project.ProjectId,
                CreatedBy = "HR",
                StartDate = DateTime.Now
            });

            try
            {
                _projectManager.DeleteProject(project.ProjectId);
            } catch(DeletingEntityWithExistingAttachmentsException)
            {
                Assert.Pass();
            }
            Assert.Fail();
        }

        [Test]
        public void CanUpdateProject()
        {
            Project project = _projectRepository.Insert(new Project { ProjectName = "test", CreatedBy = "test8" });
            String target = "Success";
            project.ProjectName = target;
            _projectManager.UpdateProject(project);
            Assert.True(_projectRepository.GetBy(project.ProjectId).ProjectName.Equals(target));
        }
    }
}
