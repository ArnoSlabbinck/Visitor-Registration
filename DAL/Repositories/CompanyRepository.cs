using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<Company>  GetEmployeesFromCompany(int id)
        {
            return await Task.FromResult(await applicationDbContext.Companies.Where(c => c.Id == id).Include(e => e.Employees).FirstOrDefaultAsync());
        }

        public async Task<Company> GetCompanyWithImage(int id)
        {
           return await Task.FromResult(await applicationDbContext.Companies.Where(c => c.Id == id).Include(e => e.Picture).FirstOrDefaultAsync());
        }

        public async Task<Company> GetCompanyWithImageAndEmployees(int id)
        {
            return await Task.FromResult(await applicationDbContext.Companies.Where(c => c.Id == id).Include(e => e.Picture).Include(h => h.Employees).FirstOrDefaultAsync());
        }

        public async Task<Company> GetEmployeesFromCompany(string companyName)
        {
            return await Task.FromResult(await applicationDbContext.Companies.Where(c => c.Name == companyName).Include(e => e.Employees).FirstOrDefaultAsync());
        }
    }

    public interface ICompanyRespository : IRepository<Company>
    {
        Building getBuilding();

        IQueryable<Company> GetOrderedCompanies();

        Task<Company> GetEmployeesFromCompany(int id);

        Task<Company> GetCompanyWithImage(int id);

        Task<Company> GetCompanyWithImageAndEmployees(int id);

        Task<Company> GetEmployeesFromCompany(string companyName);
    }
}
