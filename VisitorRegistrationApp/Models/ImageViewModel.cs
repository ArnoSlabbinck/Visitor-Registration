using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorRegistrationApp.Models
{
    public class ImageViewModel
    {
        public int Id { get; set; }
        [Required]
        [StringLength(40, ErrorMessage = "Image name can't be more than 40 characters long")]
        public string ImageName { get; set; }

        public string OriginalFormat { get; set; }
        [Required(ErrorMessage = "Image needs byte array")]

        public byte[] ImageFile { get; set; }
    }
}
