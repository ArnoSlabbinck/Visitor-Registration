using Microsoft.AspNetCore.Http;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;
using System.Drawing;
namespace VisitorRegistrationApp.Models
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "A company needs a name")]
        [Display(Name = "The Name")]
        public string Name { get; set; }
 

        public virtual ICollection<EmployeeViewModel> Employees { get; set; }

        public virtual Building Building { get; set; }
        public virtual Model.Image Picture { get; set; } 

        [Display(Name = "Description")]
        [Required(ErrorMessage = "A company needs a description")]
        public string Description { get; set; }
        [Display(Name = "Photo")]
        //Display the Image 
        public string Base64Image { get; set; }
        [Required(ErrorMessage = "A company has a type")]
        public string TypeOfBusiness { get; set; }
        [Required(ErrorMessage = "A company has a location")]
        public string Location { get; set; }


        [Required(ErrorMessage = "You need to upload the right image type")]
 
        public IFormFile file { 
            get; set; 
        }

     


    }
}
