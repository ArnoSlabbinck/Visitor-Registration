using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;

namespace VisitorRegistrationApp.Models
{
    public class CompanyViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }

        public virtual Building Building { get; set; }

        public IFormFile CompanyPhoto { get; set; }

        public string PhotoUrl { get; set; }
    }
}
