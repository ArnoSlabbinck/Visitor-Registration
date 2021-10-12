using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data;

namespace BLL.Validators
{
    public class VisitorValidator : AbstractValidator<ApplicationUser>
    {
        public VisitorValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("You need to fill in a firstname").Length(5, 20).WithMessage("First Name Should be min 5 and max 20 length");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("You need to fill in a lastname").Length(5, 20).WithMessage("Last Name Should be min 5 and max 20 length"); 
            RuleFor(x => x.Gender).NotNull().WithMessage("You need to give a gender");
            RuleFor(model => model.VisitingCompany)
                .NotNull()
                .SetValidator(new CompanyValidator());
          
            RuleFor(x => x.Email).EmailAddress().WithMessage("You need to give the right email address");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("You need to give a phone number");
            
        
        }

    }
}
