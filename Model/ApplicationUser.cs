

using Microsoft.AspNetCore.Http;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;

namespace VisitorRegistrationApp.Data
{
    public class ApplicationUser : Microsoft.AspNetCore.Identity.IdentityUser
    {
       
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Provide Last Name")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "First Name Should be min 5 and max 20 length")]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "A visitor needs to have a lastname")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Last Name Should be min 5 and max 20 length")]
        public string LastName { get; set; }
       
        [Required(ErrorMessage = "Please Provide Gender")]
        public bool Gender { get; set; } // true male, false female
 
        [NotMapped]
        public string Fullname { get { return FirstName + " " + LastName; } }
        [Required(ErrorMessage ="A visitor needs to visit a company if he/she wants to have access to the building")]
        public virtual Company VisitingCompany { get; set; }
        [Required(ErrorMessage = "A visitor needs to have an appointment with somebody in the building")]

        public virtual Employee Host { get; set; }


        public TimeSpan CheckIn { get; set; } = DateTime.Now.TimeOfDay; // Van zodra een user inlogt

       
        public TimeSpan? CheckOut { get; set; }


        public DateTime? VisitDay { get; set; } = DateTime.Now.Date;


        public VisitStatus VisitStatus { get; set; }

        public int? PictureId { get; set; }
        public virtual Image Picture { get; set; }

        public string Notes { get; set; }


    }

    public enum VisitStatus
    {

        CheckIn,
        UpComming,
        ExpectedArrival, 
        CheckOut, 
        Admin, 
        Employee, 
        Manager, 
        Constractor
    }
}
