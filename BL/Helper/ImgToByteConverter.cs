using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Helper
{
    public static  class ImgToByteConverter 
    {
    
        public static byte[] Base64StringToByteArray(string Base64String)
        {
            return Convert.FromBase64String(Base64String);
        }

        public static string byteArrayToImage(byte[] byteArrayIn)
        {
            return Convert.ToBase64String(byteArrayIn);
        }

      
    }

  
}
