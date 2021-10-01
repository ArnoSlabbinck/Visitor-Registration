using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data;
using VisitorRegistrationApp.Data.Entities;

namespace Model
{
    public class Image
    {
        public int ImageId { get; set; }
        [Required]
        [StringLength(40, ErrorMessage = "Image name can't be more than 40 characters long")]
        public string ImageName { get; set; }
        
        public string OriginalFormat { get; set; }
        [Required(ErrorMessage ="Image needs byte array")]

        public byte[] ImageFile { get; set; }

        public virtual Company Company { get; set; }

        public virtual Employee Employee { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
