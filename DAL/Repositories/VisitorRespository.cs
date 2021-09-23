using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data;
using VisitorRegistrationApp.Data.Repository;

namespace DAL.Repositories
{
    public class VisitorRespository : BaseRepository<ApplicationUser, ApplicationDbContext> , IVisitorRepository
    {
        private readonly ApplicationDbContext applicationDbContext;
        public VisitorRespository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
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
    }

    public interface IVisitorRepository : IRepository<ApplicationUser>
    {
        ApplicationUser getUserByName(string firstname, string lastname);

        void DeleteUserWithUserId(string userid);
    }
}
