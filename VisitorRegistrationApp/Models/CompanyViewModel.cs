using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;

namespace VisitorRegistrationApp.Models
{
    public class CompanyViewModel
    {
        public int Id { get; set; }
        [Display(Name = "The Name")]
        public string Name { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        public virtual Building Building { get; set; }
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "Photo")]
        public IFormFile Photo { get; set; }

        public string CompanyPhoto { get; set; }
    }
}
