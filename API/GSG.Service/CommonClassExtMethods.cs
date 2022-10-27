using Autofac;
using GSG.Service.Interfaces;

namespace GSG.Service
{
    //SOLID
    //Single Responsability Principle

    //Polymorphism 

    public static class CommonClassExtMethods
    {
        public static ContainerBuilder AddServices(this ContainerBuilder builder)
        {
            builder.RegisterType<CertificateManager>().As<ICertificateManager>();
            builder.RegisterType<EmployeeCertificateManager>().As<IEmployeeCertificateManager>();
            builder.RegisterType<EmployeeManager>().As<IEmployeeManager>();    
            builder.RegisterType<EmployeeProjectManager>().As<IEmployeeProjectManager>();
            builder.RegisterType<EmployeeRoleManager>().As<IEmployeeRoleManager>();
            builder.RegisterType<EmployeeSkillManager>().As<IEmployeeSkillManager>();
            builder.RegisterType<EmployerManager>().As<IEmployerManager>();
            builder.RegisterType<ProjectManager>().As<IProjectManager>();
            builder.RegisterType<RoleManager>().As<IRoleManager>();
            builder.RegisterType<SkillManager>().As<ISkillManager>();
            return builder;
        }
    }
}
