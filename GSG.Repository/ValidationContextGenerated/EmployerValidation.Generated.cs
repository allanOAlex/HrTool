//---------------------------------------------------------------------------------------
//       This code was generated from a template
//       Manual changes to this file may cause unexpected behavior in your application.
//       Manual changes to this file will be overwritten if the code is regenerated.
//---------------------------------------------------------------------------------------


using FluentValidation;
using GSG.Model;

namespace GSG.Repository.Validation;
    public partial class EmployerValidation : AbstractModelValidator<Employer>
    {
        public EmployerValidation()
        {
            RuleFor(x => x.EmployerId);
            RuleFor(x => x.Created);
            RuleFor(x => x.CreatedBy).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.EmployerName).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.RowVer);
            RuleFor(x => x.Updated);
        }
    }
