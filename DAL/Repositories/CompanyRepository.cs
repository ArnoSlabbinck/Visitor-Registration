using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VisitorRegistrationApp.Data.Entities;

namespace VisitorRegistrationApp.Data.Repository
{
    public class CompanyRepository : BaseRepository<Company, ApplicationDbContext>, ICompanyRespository
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly ILogger<Company> logger;
        public CompanyRepository(ApplicationDbContext applicationDbContext, ILogger<Company> logger) : base(applicationDbContext, logger)
        {
            this.applicationDbContext = applicationDbContext;
            this.logger = logger;
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
