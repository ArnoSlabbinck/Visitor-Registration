using Microsoft.Extensions.Logging;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data;
using VisitorRegistrationApp.Data.Repository;

namespace DAL.Repositories
{
    public class ImageRepository : BaseRepository<Image, ApplicationDbContext>, IImageRespository
    {
        private readonly ApplicationDbContext applicationDbContext;
        public ImageRepository(ApplicationDbContext applicationDbContext, ILogger<Image> logger) : base(applicationDbContext, logger)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public Image GetImageFromName(string name)
        {
            return applicationDbContext.Images.Where(i => i.ImageName == name).FirstOrDefault();
        }
    }

    public interface IImageRespository : IRepository<Image>
    {
        Image GetImageFromName(string name);
    }
}
