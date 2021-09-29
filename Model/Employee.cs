using Microsoft.AspNetCore.Http;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorRegistrationApp.Data.Entities
{
    public class Employee 
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

        public bool AtWorkStatus { get; set; } = true;


        [Required(ErrorMessage = "An Employee needs to have a hire date")]
        public DateTime HireDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? PictureId { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }

        public virtual Image Picture { get; set; }

        public virtual Company Company { get; set; }
    }
}
