using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;

namespace VisitorRegistrationApp.Data.Repository
{
    public class EmployeeRepository : BaseRepository<Employee, ApplicationDbContext>, IEmployeeRespository
    {
        public EmployeeRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            
        }
    }

    public interface IEmployeeRespository
    {

    }
}
