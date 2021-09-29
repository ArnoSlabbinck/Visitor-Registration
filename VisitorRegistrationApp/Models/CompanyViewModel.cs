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
 

        public virtual ICollection<Employee> Employees { get; set; }

        public virtual Building Building { get; set; }
        public virtual Model.Image Picture { get; set; } 

        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Photo")]
        //Display the Image 
        public string Base64Image { get; set; }
        //Uploaden van imagefile 
        //Omzetten naar bytearray
        // Omzetting onmiddelijk in de Viewmodel laten gebeure
        [Required(ErrorMessage = "You need to upload the right image type")]
 
        public IFormFile file { 
            get; set; 
        }

     


    }
}
