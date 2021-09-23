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
    public class PhotoService : IPhotoService
    {
    

        private readonly IWebHostEnvironment hostEnvironment;
        public PhotoService(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
           
        }

        public string UploadPhoto(IFormFile photo)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            // Path Vinden voor de photos folder 
            string pathName = Path.Combine(hostEnvironment.WebRootPath, "photos");
            string fileNameWithPath = Path.Combine(pathName, uniqueFileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                photo.CopyTo(stream);
            }

            return uniqueFileName;
        }

        public void DeletePhoto(string photoUrl)
        {
            string path = Path.Combine(hostEnvironment.WebRootPath, photoUrl.Substring(1));
            System.IO.File.Delete(path);
        }

      
    }

    public interface IPhotoService
    {
        void DeletePhoto(string photoUrl);

        string UploadPhoto(IFormFile photo);

    }
}
