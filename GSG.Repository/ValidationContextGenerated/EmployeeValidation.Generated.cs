//---------------------------------------------------------------------------------------
//       This code was generated from a template
//       Manual changes to this file may cause unexpected behavior in your application.
//       Manual changes to this file will be overwritten if the code is regenerated.
//---------------------------------------------------------------------------------------


using FluentValidation;
using GSG.Model;

namespace GSG.Repository.Validation;
    public partial class EmployeeValidation : AbstractModelValidator<Employee>
    {
        public EmployeeValidation()
        {
            RuleFor(x => x.EmployeeId);
            RuleFor(x => x.Address).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.Address2).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.City).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.Created);
            RuleFor(x => x.CreatedBy).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty().NotNull().MaximumLength(200);
            RuleFor(x => x.EmployeeState).NotEmpty().NotNull().MaximumLength(2);
            RuleFor(x => x.FirstName).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.LastName).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(x => x.PhoneNumber).NotEmpty().NotNull().MaximumLength(10);
            RuleFor(x => x.PictureUrl).NotEmpty().NotNull().MaximumLength(255);
            RuleFor(x => x.RowVer);
            RuleFor(x => x.Updated);
            RuleFor(x => x.Zip).NotEmpty().NotNull().MaximumLength(50);
        }
    }
