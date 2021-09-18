﻿

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public string Fullname { get { return FirstName + " " + LastName; } }
        [Required(ErrorMessage ="A visitor needs to visit a company if he/she wants to have access to the building")]
        public virtual Company VisitingCompany { get; set; }
        [Required(ErrorMessage = "A visitor needs to have an appointment with somebody in the building")]

        public virtual ICollection<Employee> AppointmenrWith { get; set; }
        

        [Display(Name = "Check-In")]
        public DateTime? CheckIn { get; set; } = DateTime.Now; // Van zodra een user inlogt

        [Display(Name ="Check-Out")]
        public DateTime? CheckOut { get; set; }

        [Display(Name = "VisitDay")]
        public DateTime? VisitDay { get; set; }


        public TimeSpan? VisitedTime { get; set; }// Bereiken bij het completed visiting

        public VisitStatus VisitStatus { get; set; }

        public string VisitorPhoto { get; set; }
    }

    public enum VisitStatus
    {

        CheckIn,
        UpComming,
        ExpectedArrival, 
        Completed, 
        Admin, 
        Employee
    }
}