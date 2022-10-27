using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GSG.Model;
using GSG.Model.DTO.Responses;
using GSG.Model.DTO.Requests;

namespace GSG.Repository.Profiles
{
    public class BOToDTOProfile : Profile
    {
        public BOToDTOProfile()
        {
            CreateMap<Certificate, CertificateResponse>().ReverseMap();
            CreateMap<EmployeeCertificate, EmployeeCertificateResponse>().ReverseMap();
            CreateMap<Employee, EmployeeResponse>().ReverseMap();
            CreateMap<EmployeeProject, EmployeeProjectResponse>().ReverseMap();
            CreateMap<EmployeeRole, EmployeeRoleResponse>().ReverseMap();
            CreateMap<EmployeeSkill, EmployeeSkillResponse>().ReverseMap();
            CreateMap<Employer, EmployerResponse>().ReverseMap();
            CreateMap<Project, ProjectResponse>().ReverseMap();
            CreateMap<Role, RoleResponse>().ReverseMap();
            CreateMap<Skill, SkillResponse>().ReverseMap();
            CreateMap<Certificate, CertificateRequest>().ReverseMap();
            CreateMap<EmployeeCertificate, EmployeeCertificateRequest>().ReverseMap();
            CreateMap<Employee, EmployeeRequest>().ReverseMap();
            CreateMap<EmployeeProject, EmployeeProjectRequest>().ReverseMap();
            CreateMap<EmployeeRole, EmployeeRoleRequest>().ReverseMap();
            CreateMap<EmployeeSkill, EmployeeSkillRequest>().ReverseMap();
            CreateMap<Employer, EmployerRequest>().ReverseMap();
            CreateMap<Project, ProjectRequest>().ReverseMap();
            CreateMap<Role, RoleRequest>().ReverseMap();
            CreateMap<Skill, SkillRequest>().ReverseMap();

            CreateMap<Employee, EmployeeGridResponse>()
                .ForMember(
                dst => dst.SkillName,
                opt =>
                    opt.MapFrom(src => string.Join(", ", src.EmployeeSkill.Select(row => row.Skill.SkillName))))
                .ForMember(
                dst => dst.EmployerName,
                opt =>
                    opt.MapFrom(src => string.Join(", ", src.EmployeeRole.Select(row => row.Employer.EmployerName))))
                .ForMember(
                dst => dst.CertificateName,
                opt =>
                    opt.MapFrom(src => string.Join(", ", src.EmployeeCertificate.Select(row => row.Certificate.CertificateName))))
                .ForMember(
                dst => dst.ProjectName,
                opt =>
                    opt.MapFrom(src => string.Join(", ", src.EmployeeProject.Select(row => row.Project.ProjectName))))
                .ForMember(
                    dst => dst.RoleName,
                    opt => opt.MapFrom(src => string.Join(", ", src.EmployeeRole.Select(row => row.Role.RoleName))));

            CreateMap<EmployeeCertificate, EmployeeCertificateProfileResponse>()
                .ForMember(
                dst => dst.CertificateName,
                opt => opt.MapFrom(src => src.Certificate.CertificateName));

            CreateMap<EmployeeProject, EmployeeProjectProfileResponse>()
                .ForMember(
                dst => dst.ProjectName,
                opt => opt.MapFrom(src => src.Project.ProjectName));

            CreateMap<EmployeeRole, EmployeeRoleProfileResponse>()
                .ForMember(
                dst => dst.RoleName,
                opt => opt.MapFrom(src => src.Role.RoleName))
                .ForMember(
                dst => dst.EmployerName,
                opt => opt.MapFrom(src => src.Employer.EmployerName));

            CreateMap<EmployeeSkill, EmployeeSkillProfileResponse>()
                .ForMember(
                dst => dst.SkillName,
                opt => opt.MapFrom(src => src.Skill.SkillName));
        }
    }
}