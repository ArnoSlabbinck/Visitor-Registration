using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    public interface ICompanyRespository
    {
        Building getBuilding();
    }
}
