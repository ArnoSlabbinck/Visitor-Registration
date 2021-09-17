using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
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

        public IQueryable<Company> GetOrderedCompanies()
        {
            return applicationDbContext.Companies.OrderByDescending(p => p);
        }

        public Company  GetEmployeesFromCompany(int id)
        {
            return applicationDbContext.Companies.Where(c => c.Id == id).Include(e => e.Employees).SingleOrDefault();
        }

    }

    public interface ICompanyRespository : IRepository<Company>
    {
        Building getBuilding();

        IQueryable<Company> GetOrderedCompanies();

        Company GetEmployeesFromCompany(int id);
    }
}
