using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data;
using VisitorRegistrationApp.Data.Entities;
using VisitorRegistrationApp.Data.Repository;

namespace DAL.Repositories
{
    public class VisitorRespository : BaseRepository<ApplicationUser, ApplicationDbContext> , IVisitorRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly ILogger<ApplicationUser> logger;
        public VisitorRespository(ApplicationDbContext applicationDbContext, ILogger<ApplicationUser> logger) : base(applicationDbContext, logger)
        {
            this.applicationDbContext = applicationDbContext;
            this.logger = logger;
        }

        public void DeleteUserWithUserId(string userid)
        {
            ApplicationUser applicationUser = applicationDbContext.Users.Where(u => u.Id == userid).FirstOrDefault();
            applicationDbContext.Users.Remove(applicationUser);
        }

        public ApplicationUser getUserByName(string firstname, string lastname)
        {
            //je krijgt een fullname
            // Ophalen van Visitor in de database en status veranderen van visitor
            return applicationDbContext.Users.Where(u => u.FirstName.ToLower() == firstname && u.LastName.ToLower() == lastname).FirstOrDefault();
        }

        public ApplicationUser GetUserByNameWithImage(string firstname, string lastname, int id)
        {
            return applicationDbContext.Users.Where(u => u.FirstName.ToLower() == firstname && u.LastName.ToLower() == lastname).Include(i => i.Picture).Where(image => image.PictureId == id).FirstOrDefault();
        }

        void IVisitorRepository.DeleteUserWithUserId(string userid)
        {
            throw new NotImplementedException();
        }

        ApplicationUser IVisitorRepository.getUserByName(string firstname, string lastname)
        {
            throw new NotImplementedException();
        }

        ApplicationUser IVisitorRepository.GetUserByNameWithImage(string firstname, string lastname, int id)
        {
            throw new NotImplementedException();
        }
    }

    public interface IVisitorRepository : IRepository<ApplicationUser>
    {
        ApplicationUser getUserByName(string firstname, string lastname);

        void DeleteUserWithUserId(string userid);

        ApplicationUser GetUserByNameWithImage(string firstname, string lastname, int id);
    }
}
