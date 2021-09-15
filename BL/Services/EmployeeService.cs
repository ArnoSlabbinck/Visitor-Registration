using System.Collections.Generic;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;

namespace BL.Services
{
    // This layer is for validating entity so they can be passed down to the view or to the data access
    public class EmployeeService : IEmployeeService
    {
        public void Add(Employee employee)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task<Employee> Get(int Id)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Employee>> getAll()
        {
            throw new System.NotImplementedException();
        }

        public void Update(Employee employee)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> getAll();

        Task<Employee> Get(int Id);

        void Update(Employee employee);

        void Add(Employee employee);

        void Delete(int id);
    }
}
