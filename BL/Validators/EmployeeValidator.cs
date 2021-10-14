using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;

namespace BLL.Validators
{
    public class EmployeeValidator : AbstractValidator<Employee>
    {
        public EmployeeValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("You need to give an employee a name").Length(5, 20);
            RuleFor(x => x.BirthDay).NotNull().WithMessage("Every Employee needs to have a birthday");
          


        }
    }
}
