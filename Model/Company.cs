using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorRegistrationApp.Data.Entities
{
    public class Company : IEntity
    {
        public Company()
        {
            Employees = new List<Employee>();
        }
        [DisplayName("CompanyId")]
        public int Id { get; set; }
        [Required(ErrorMessage = "An Company needs to have a name")]
        public string Name { get; set; }
        
        public virtual ICollection<Employee> Employees { get; set; }
        [Required(ErrorMessage = "An Company needs to have a building")]
        public virtual Building Building { get; set; }
    }
}
