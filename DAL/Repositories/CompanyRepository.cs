using System.Linq;
using VisitorRegistrationApp.Data.Entities;

namespace VisitorRegistrationApp.Data.Repository
{
    public class CompanyRepository : BaseRepository<Company, ApplicationDbContext>, ICompanyRespository
    {
        private readonly ApplicationDbContext applicationDbContext;
        public CompanyRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public Building getBuilding()
        {
            return applicationDbContext.Building.SingleOrDefault();
            
        }
    }

    public interface ICompanyRespository : IRepository<Company>
    {
        Building getBuilding();
    }
}
