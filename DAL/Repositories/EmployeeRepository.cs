using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using VisitorRegistrationApp.Data.Entities;

namespace VisitorRegistrationApp.Data.Repository
{
    public class EmployeeRepository : BaseRepository<Employee, ApplicationDbContext>, IEmployeeRespository
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly ILogger<Employee> logger;
        public EmployeeRepository(ApplicationDbContext applicationDbContext, ILogger<Employee> logger) : base(applicationDbContext, logger)
        {
            this.applicationDbContext = applicationDbContext;
            this.logger = logger;
        }

        public IQueryable<Employee> GetEmployeesWithCompanies()
        {
            return applicationDbContext.Employees.Include(c => c.Company);
        }
    }

    public interface IEmployeeRespository : IRepository<Employee>
    {
        IQueryable<Employee> GetEmployeesWithCompanies();
    }
}
