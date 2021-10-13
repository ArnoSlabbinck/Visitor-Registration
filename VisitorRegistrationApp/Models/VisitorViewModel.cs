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
            Hosts = new List<EmployeeViewModel>();
          
        }
        [Required(ErrorMessage = "You need to give a firstname")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "You need to give a lastname")]
        public string LastName { get; set; }

        public string Fullname { get { return FirstName + " " + LastName; } }


        public string Email { get; set; }

        public virtual CompanyViewModel VisitingCompany { get; set; }

        public string CompanyName { get; set; } 

        public virtual ICollection<EmployeeViewModel> Hosts { get; set; }

        [Required(ErrorMessage = "A visitor needs to have a checkin")]
        public VisitStatus VisitStatus { get; set; }

        //Each registration must have a start and end date
        public TimeSpan VisitedTime { get; set; }

        public DateTime VisitDate { get; set; }

        public string ChosenPurpose { get; set; } = "Visitor";

        public string Base64Image { get; set; }
  
        [Required(ErrorMessage = "You need to file in an image")]

        public IFormFile file { get; set; }

        public byte[] ImageFile { get; set; }

        public TimeSpan CheckIn { get; set; }

        public TimeSpan CheckOut { get; set; }

        public List<SelectListItem> Purpose
        {
            get;
            set;
        }

        public void SetPurposes()
        {
            Purpose = new List<SelectListItem>
            {
                     new SelectListItem { Text = "Visitor", Value = "Visitor" },
                    // new SelectListItem { Text = "Making a Delivery", Value = "Delivery" },
                     new SelectListItem { Text = "Siging Out", Value = "SignOut" }
                     //new SelectListItem { Text = " UpComing", Value = " UpComing"}
            };

        }
    }
}
