using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data;
using VisitorRegistrationApp.Data.Entities;

namespace VisitorRegistrationApp.Models
{
    public class VisitorViewModel
    {
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
       
        public string Fullname { get { return FirstName + " " + LastName; } }


        public string Email { get; set; }

        public virtual Company VisitingCompany { get; set; }

        public virtual IList<Employee> AppointmentPeople { get; set; }

        public VisitStatus VisitStatus { get; set; }

        //Each registration must have a start and end date
        public TimeSpan VisitedTime { get; set; }

        public DateTime VisitDate { get; set; }

        public string ChosenPurpose { get; set; }


        public List<SelectListItem> Purpose {
            get;
            set;
        }
    }
}
