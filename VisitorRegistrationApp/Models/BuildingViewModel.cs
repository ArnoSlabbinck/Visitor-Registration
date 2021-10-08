using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorRegistrationApp.Models
{
    public class BuildingViewModel
    {
       
        public int Id { get; set; }
        [Required(ErrorMessage = "A building needs to have a name")]
        [StringLength(50, ErrorMessage = "Only 50 character are allowed")]

        public string Name { get; set; } = "4Wings";



    }
}
