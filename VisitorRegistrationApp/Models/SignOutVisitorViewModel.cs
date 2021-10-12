using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorRegistrationApp.Models
{
    public class SignOutVisitorViewModel
    {
    
        [Required(ErrorMessage = "You need to give a firstname")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "You need to give a lastname")]
        public string LastName { get; set; }

        public string Fullname { get { return FirstName + " " + LastName; } }
    }
}
