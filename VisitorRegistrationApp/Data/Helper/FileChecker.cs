using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorRegistrationApp.Data.Helper
{
    public static class FileChecker
    {
        public static bool CheckUploadedFileIsImage(IFormFile file)
        {
            if (file.ContentType.Contains("image"))
            {
                //valid image
                return true; 
            }
            else
            {
                //not a valid image
                return false;
            }
        }
    }
}
