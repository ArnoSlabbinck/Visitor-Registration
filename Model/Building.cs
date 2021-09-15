using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorRegistrationApp.Data.Entities
{
    public class Building : IEntity
    {
        public Building()
        {
            Visitors = new List<ApplicationUser>();
            Companies = new List<Company>();
        }
        [DisplayName("BuildingId")]
        public int Id { get; set; }
        [Required(ErrorMessage = "A building needs to have a name")]
        public string Name { get; set; } = "4Wings";
        

        public virtual ICollection<ApplicationUser> Visitors { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        
    }
}
