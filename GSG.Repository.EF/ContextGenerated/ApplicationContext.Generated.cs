using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using GSG.Model;

//---------------------------------------------------------------------------------------
//       This code was generated from a template
//       Manual changes to this file may cause unexpected behavior in your application.
//       Manual changes to this file will be overwritten if the code is regenerated.
//---------------------------------------------------------------------------------------


namespace GSG.Repository.EF
{
    public abstract class ApplicationContext : DbContext
    {
        public virtual DbSet<Certificate> Certificate { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<EmployeeCertificate> EmployeeCertificate { get; set; }
        public virtual DbSet<EmployeeProject> EmployeeProject { get; set; }
        public virtual DbSet<EmployeeRole> EmployeeRole { get; set; }
        public virtual DbSet<EmployeeSkill> EmployeeSkill { get; set; }
        public virtual DbSet<Employer> Employer { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Skill> Skill { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ConfigureContext(optionsBuilder);
        }
        protected abstract void ConfigureContext(DbContextOptionsBuilder optionsBuilder);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Certificate>(entity =>
            {
                entity.HasIndex(e => e.CertificateName, "Certificate_certificateName_key")
                    .IsUnique();

                entity.Property(e => e.CertificateId).HasColumnName("certificateId");

                entity.Property(e => e.CertificateName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("certificateName");

                entity.Property(e => e.Created)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("createdBy");

                entity.Property(e => e.RowVer).HasColumnName("rowVer");

                entity.Property(e => e.Updated)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.EmployeeId).HasColumnName("employeeId");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("address");

                entity.Property(e => e.Address2)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("address2");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("city");

                entity.Property(e => e.Created).HasColumnName("created");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("createdBy");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("email");

                entity.Property(e => e.EmployeeState)
                    .IsRequired()
                    .HasMaxLength(2)
                    .HasColumnName("employeeState");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("firstName");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("lastName");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("phoneNumber");

                entity.Property(e => e.PictureUrl)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("pictureUrl");

                entity.Property(e => e.RowVer).HasColumnName("rowVer");

                entity.Property(e => e.Updated)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updated");

                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("zip");
            });

            modelBuilder.Entity<EmployeeCertificate>(entity =>
            {
                entity.Property(e => e.EmployeeCertificateId).HasColumnName("employeeCertificateId");

                entity.Property(e => e.AwardedDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("awardedDate");

                entity.Property(e => e.CertificateId).HasColumnName("certificateId");

                entity.Property(e => e.Created)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("createdBy");

                entity.Property(e => e.EmployeeId).HasColumnName("employeeId");

                entity.Property(e => e.RowVer).HasColumnName("rowVer");

                entity.Property(e => e.Updated)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updated");

                entity.HasOne(d => d.Certificate)
                    .WithMany(p => p.EmployeeCertificate)
                    .HasForeignKey(d => d.CertificateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_certificateId");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeCertificate)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_employeeId");
            });

            modelBuilder.Entity<EmployeeProject>(entity =>
            {
                entity.Property(e => e.EmployeeProjectId).HasColumnName("employeeProjectId");

                entity.Property(e => e.Created)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("createdBy");

                entity.Property(e => e.EmployeeId).HasColumnName("employeeId");

                entity.Property(e => e.EndDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("endDate");

                entity.Property(e => e.ProjectId).HasColumnName("projectId");

                entity.Property(e => e.StartDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("startDate");

                entity.Property(e => e.Updated)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updated");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeProject)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_employeeId");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.EmployeeProject)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_projectId");
            });

            modelBuilder.Entity<EmployeeRole>(entity =>
            {
                entity.Property(e => e.EmployeeRoleId).HasColumnName("employeeRoleId");

                entity.Property(e => e.Created)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("createdBy");

                entity.Property(e => e.EmployeeId).HasColumnName("employeeId");

                entity.Property(e => e.EmployerId).HasColumnName("employerId");

                entity.Property(e => e.EndDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("endDate");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.RowVer).HasColumnName("rowVer");

                entity.Property(e => e.StartDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("startDate");

                entity.Property(e => e.Updated)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updated");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeRole)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EmployeeId");

                entity.HasOne(d => d.Employer)
                    .WithMany(p => p.EmployeeRole)
                    .HasForeignKey(d => d.EmployerId)
                    .HasConstraintName("FK_EmployerId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.EmployeeRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RoleId");
            });

            modelBuilder.Entity<EmployeeSkill>(entity =>
            {
                entity.Property(e => e.EmployeeSkillId).HasColumnName("employeeSkillId");

                entity.Property(e => e.Created)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("createdBy");

                entity.Property(e => e.EmployeeId).HasColumnName("employeeId");

                entity.Property(e => e.RowVer).HasColumnName("rowVer");

                entity.Property(e => e.SkillId).HasColumnName("skillId");

                entity.Property(e => e.Updated)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updated");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.EmployeeSkill)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_employeeId");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.EmployeeSkill)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_skillId");
            });

            modelBuilder.Entity<Employer>(entity =>
            {
                entity.HasIndex(e => e.EmployerName, "Employer_employerName_key")
                    .IsUnique();

                entity.Property(e => e.EmployerId).HasColumnName("employerId");

                entity.Property(e => e.Created)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("createdBy");

                entity.Property(e => e.EmployerName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("employerName");

                entity.Property(e => e.RowVer).HasColumnName("rowVer");

                entity.Property(e => e.Updated)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasIndex(e => e.ProjectName, "Project_projectName_key")
                    .IsUnique();

                entity.Property(e => e.ProjectId).HasColumnName("projectId");

                entity.Property(e => e.Created)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("createdBy");

                entity.Property(e => e.EndDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("endDate");

                entity.Property(e => e.ProjectName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("projectName");

                entity.Property(e => e.RowVer).HasColumnName("rowVer");

                entity.Property(e => e.StartDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("startDate");

                entity.Property(e => e.Updated)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasIndex(e => e.RoleName, "Role_roleName_key")
                    .IsUnique();

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.Created)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("createdBy");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("roleName");

                entity.Property(e => e.RowVer).HasColumnName("rowVer");

                entity.Property(e => e.Updated)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updated");
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasIndex(e => e.SkillName, "Skill_skillName_key")
                    .IsUnique();

                entity.Property(e => e.SkillId).HasColumnName("skillId");

                entity.Property(e => e.Created)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("created");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("createdBy");

                entity.Property(e => e.RowVer).HasColumnName("rowVer");

                entity.Property(e => e.SkillName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("skillName");

                entity.Property(e => e.Updated)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("updated");
            });

            OnModelCreatingPartial(modelBuilder);
        }
        public abstract IDbContextTransaction GetContextTransaction();

        protected abstract void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
