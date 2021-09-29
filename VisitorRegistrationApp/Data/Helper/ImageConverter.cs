using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorRegistrationApp.Data.Helper
{
    public static class ImageConverter
    {
        //Omzetten van file naar byte array
        public static byte[] fileToByteArray(IFormFile file)
        {
            var emptyBytes = new byte[0];
            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    return fileBytes;
                }
            }
            else
            {
                return emptyBytes;
            }
        }
    }
}
