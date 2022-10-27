//---------------------------------------------------------------------------------------
//       This code was generated from a template
//       Manual changes to this file may cause unexpected behavior in your application.
//       Manual changes to this file will be overwritten if the code is regenerated.
//---------------------------------------------------------------------------------------


using FluentValidation;
using GSG.Model;

namespace GSG.Repository.Validation;
    public partial class EmployeeCertificateValidation : AbstractModelValidator<EmployeeCertificate>
    {
        public EmployeeCertificateValidation()
        {
            RuleFor(x => x.EmployeeCertificateId);
            RuleFor(x => x.AwardedDate);
            RuleFor(x => x.CertificateId);
            RuleFor(x => x.Created);
            RuleFor(x => x.CreatedBy).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.EmployeeId);
            RuleFor(x => x.RowVer);
            RuleFor(x => x.Updated);
        }
    }
