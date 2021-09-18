﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;

namespace VisitorRegistrationApp.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="You need to fill in a name")]
        [StringLength(30, MinimumLength = 2)]
        public string Name { get; set; }
        [DataType(DataType.Date, ErrorMessage ="You need to fill in a date")]
        public DateTime BirthDay { get; set; } = DateTime.Now;

        [Required(ErrorMessage ="You need to fill in an Employee Job")]
        [StringLength(30, MinimumLength = 2)]
        public string Job { get; set; }

        public double Salary { get; set; } = 2000;

        public IFormFile ProfilePhoto { get; set; }

        public string PhotoUrl { get; set; }

        public bool AtWorkStatus { get; set; } = true;

        
        public int CompanyId { get; set; }
        [Required(ErrorMessage ="An Employee works for a company")]
        public virtual Company Company { get; set; }
    }
}