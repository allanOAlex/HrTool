//---------------------------------------------------------------------------------------
//       This code was generated from a template
//       Manual changes to this file may cause unexpected behavior in your application.
//       Manual changes to this file will be overwritten if the code is regenerated.
//---------------------------------------------------------------------------------------


using FluentValidation;
using GSG.Model;

namespace GSG.Repository.Validation;
    public partial class CertificateValidation : AbstractModelValidator<Certificate>
    {
        public CertificateValidation()
        {
            RuleFor(x => x.CertificateId);
            RuleFor(x => x.CertificateName).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.Created);
            RuleFor(x => x.CreatedBy).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.RowVer);
            RuleFor(x => x.Updated);
        }
    }
