using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Helper
{
    public class ConvertPhoto
    {
        private readonly IWebHostEnvironment hostEnvironment;
        public ConvertPhoto(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        private string UploadPhoto(IFormFile photo)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            string pathName = Path.Combine(hostEnvironment.WebRootPath, "photos");
            string fileNameWithPath = Path.Combine(pathName, uniqueFileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                photo.CopyTo(stream);
            }

            return uniqueFileName;
        }

        private void DeletePhoto(string photoUrl)
        {
            string path = Path.Combine(hostEnvironment.WebRootPath, photoUrl.Substring(1));
            System.IO.File.Delete(path);
        }
    }
}
