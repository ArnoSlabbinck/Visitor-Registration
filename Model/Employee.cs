using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VisitorRegistrationApp.Data.Entities
{
    public class Employee
    {
     
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "An Employee needs to have a name")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Name Should be min 5 and max 20 length")]
        public string Name { get; set; }
        [Required(ErrorMessage = "An Employee needs to have a birthday")]
        public DateTime BirthDay { get; set; }
        [Required(ErrorMessage = "An Employee needs to have a job")]
        [StringLength(20, ErrorMessage = "Only 20 character are allowed")]
        public string Job { get; set; }

        public double Salary { get; set; }
      

        public int? PictureId { get; set; }
        

        public virtual Image Picture { get; set; }

        public virtual Company Company { get; set; }

        public virtual List<ApplicationUser> Visitor { get; set; }
    }
}
