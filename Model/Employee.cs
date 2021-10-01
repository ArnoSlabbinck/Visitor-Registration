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
        [StringLength(20, ErrorMessage = "Only 20 character are allowed")]
        public string Job { get; set; }

        public  double Salary { get; set; }
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        [Required(ErrorMessage = "An Employee needs to have an Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Required(ErrorMessage = "An Employee needs to have a hire date")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime HireDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime? EndDate { get; set; }

        public int? PictureId { get; set; }
        [StringLength(50, ErrorMessage = "Only 50 character are allowed")]
        [DisplayName("Address")]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [StringLength(50, ErrorMessage = "Only 50 character are allowed")]
    
        public string City { get; set; }

        public string State { get; set; } = "Antwerp";
        [StringLength(10, ErrorMessage = "Only 10 character are allowed")]

        public string PostalCode { get; set; }
        [StringLength(25, ErrorMessage = "Only 25 character are allowed")]

        public string Country { get; set; }
        [Required(ErrorMessage = "Please enter Mobile No")]
        [Display(Name = "Contact Number")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public virtual Image Picture { get; set; }

        public virtual Company Company { get; set; }
    }
}
