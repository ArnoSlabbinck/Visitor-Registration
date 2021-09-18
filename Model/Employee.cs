using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorRegistrationApp.Data.Entities
{
    public class Employee : IEntity
    {
        [DisplayName("EmployeeId")]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "An Employee needs to have a name")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Name Should be min 5 and max 20 length")]
        public string Name { get; set; }
        [Required(ErrorMessage = "An Employee needs to have a birthday")]
        public DateTime BirthDay { get; set; }
        [Required(ErrorMessage = "An Employee needs to have a job")]
        public string Job { get; set; }

        public  double Salary { get; set; }

        public string ProfilePhoto { get; set; }
        public bool AtWorkStatus { get; set; } = true;
    
        [Required]
        public virtual Company Company { get; set; }



        
    }
}
