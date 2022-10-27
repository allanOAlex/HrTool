//---------------------------------------------------------------------------------------
//       This code was generated from a template
//       Manual changes to this file may cause unexpected behavior in your application.
//       Manual changes to this file will be overwritten if the code is regenerated.
//---------------------------------------------------------------------------------------


using Autofac;
using FluentValidation;
using GSG.Model;

namespace GSG.Repository.Validation;
public partial class ModelValidationModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<CertificateValidation>().As(typeof(AbstractModelValidator<Certificate>));
        builder.RegisterType<EmployeeValidation>().As(typeof(AbstractModelValidator<Employee>));
        builder.RegisterType<EmployeeCertificateValidation>().As(typeof(AbstractModelValidator<EmployeeCertificate>));
        builder.RegisterType<EmployeeProjectValidation>().As(typeof(AbstractModelValidator<EmployeeProject>));
        builder.RegisterType<EmployeeRoleValidation>().As(typeof(AbstractModelValidator<EmployeeRole>));
        builder.RegisterType<EmployeeSkillValidation>().As(typeof(AbstractModelValidator<EmployeeSkill>));
        builder.RegisterType<EmployerValidation>().As(typeof(AbstractModelValidator<Employer>));
        builder.RegisterType<ProjectValidation>().As(typeof(AbstractModelValidator<Project>));
        builder.RegisterType<RoleValidation>().As(typeof(AbstractModelValidator<Role>));
        builder.RegisterType<SkillValidation>().As(typeof(AbstractModelValidator<Skill>));
        AddToBuilder(builder);
    }

    partial void AddToBuilder(ContainerBuilder builder);
}
