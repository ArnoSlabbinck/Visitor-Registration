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

    // Kunnen zoeken op verschillende zoekmethodes instellen

    //1. Zoeken op chronologische volgorde
    //2. Most recent added company
    //3. De grootte van de het bedrijf
    //4. Hoeveel mensen er in het bedrijf zitten
    //5. Zoeken op Btw nummer
    //6. Type van bedrijf 
    //7. Region van het bedrijf
    //8. Address zoeken
    //9.

    // Een bedrijf heeft ook een foto
    // Heeft een gsm nummer



}
