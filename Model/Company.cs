using Microsoft.AspNetCore.Http;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorRegistrationApp.Data.Entities
{
    public class Company 
    {
        public Company()
        {
            Employees = new List<Employee>();
        }
       
        public int Id { get; set; }
        [Required(ErrorMessage = "An Company needs to have a name")]
        public string Name { get; set; }
       
        public string Description { get; set; } = "Nothing yet";
        public int? PictureId { get; set; }
        [Required(ErrorMessage ="A Company needs to have employees")]

        public virtual ICollection<Employee> Employees { get; set; }
       

        public string TypeOfBusiness { get; set; }

        public string Location { get; set; }
        [Required(ErrorMessage = "An Company needs to have a building")]
        public virtual Building Building { get; set; }

        public virtual Image Picture { get; set; }


    }

}
