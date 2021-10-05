using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Model;
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
        public VisitorViewModel()
        {
            SetPurposes();
        }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }
       
        public string Fullname { get { return FirstName + " " + LastName; } }


        public string Email { get; set; }

        public virtual Company VisitingCompany { get; set; }

        public virtual IList<Employee> AppointmentPeople { get; set; }

        public virtual ICollection<Employee> AppointmenrWith { get; set; }


        public VisitStatus VisitStatus { get; set; }

        //Each registration must have a start and end date
        public TimeSpan VisitedTime { get; set; }

        public DateTime VisitDate { get; set; }

        public string ChosenPurpose { get; set; }

        public string Base64Image { get; set; }
        //Uploaden van imagefile 
        //Omzetten naar bytearray
        // Omzetting onmiddelijk in de Viewmodel laten gebeure
        [Required(ErrorMessage = "You need to file in an image")]
        
        public IFormFile file { get; set; }
    



        public byte[] ImageFile { get; set; }

        public List<SelectListItem> Purpose {
            get;
            set;
        }

        public void SetPurposes()
        {
            Purpose  = new List<SelectListItem>
            {
                     new SelectListItem { Text = "Visitor", Value = "Visitor" },
                     new SelectListItem { Text = "Making a Delivery", Value = "Delivery" },
                     new SelectListItem { Text = "Siging Out", Value = "SignOut" },
                     new SelectListItem { Text = " UpComing", Value = " UpComing"}
            };

        }
    }
}
