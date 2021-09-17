using VisitorRegistrationApp.Data.Entities;

namespace VisitorRegistrationApp.Data.Repository
{
    public class EmployeeRepository : BaseRepository<Employee, ApplicationDbContext>, IEmployeeRespository
    {
        private readonly ApplicationDbContext applicationDbContext;
        public EmployeeRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }


    }

    public interface IEmployeeRespository : IRepository<Employee>
    {

    }
}
