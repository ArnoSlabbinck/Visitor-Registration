using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorRegistrationApp.Data.Entities
{
    public class Building 
    {
        public Building()
        {
            Visitors = new List<ApplicationUser>();
            Companies = new List<Company>();
        }
        [DisplayName("BuildingId")]
        public int Id { get; set; }
        [Required(ErrorMessage = "A building needs to have a name")]
        [StringLength(50, ErrorMessage = "Only 50 character are allowed")]
        [DisplayName("Building Name")]
        public string Name { get; set; } = "4Wings";
        [DisplayName("Building Picture")]
        public string BuildingPhoto { get; set; }

        [Required(ErrorMessage = "A building needs to have visitors")]
        public virtual ICollection<ApplicationUser> Visitors { get; set; }
        [Required(ErrorMessage = "A building needs to have companies")]
        public virtual ICollection<Company> Companies { get; set; }
        
    }
}
