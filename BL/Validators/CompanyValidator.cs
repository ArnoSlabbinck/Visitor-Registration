using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;

namespace BLL.Validators
{
    public class CompanyValidator : AbstractValidator<Company>
    {

        public CompanyValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("A Company needs to have name");
            RuleFor(x => x.Description).NotEmpty().WithMessage("A Company needs to have a description");
            RuleForEach(x => x.Employees).NotNull().WithMessage("You need to fill in a employee").SetValidator(new EmployeeValidator());
        
        }
    }
}
