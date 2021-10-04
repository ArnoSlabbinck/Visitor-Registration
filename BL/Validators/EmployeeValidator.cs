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
            RuleFor(x => x.Job).NotEmpty().WithMessage("Every employee has a job").Length(20);
            RuleFor(x => x.Email).EmailAddress().WithMessage("An Employee needs to have a valid Email Address");
            RuleFor(x => x.HireDate).NotEmpty().WithMessage("You need to give a hire date for the employee");


        }
    }
}
