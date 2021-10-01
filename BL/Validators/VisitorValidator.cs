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
        }
    }
}
