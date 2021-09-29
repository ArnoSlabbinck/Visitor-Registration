
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;
using VisitorRegistrationApp.Data.Repository;
using VisitorRegistrationApp.Helper;

namespace BL.Services
{
    // This layer is for validating entity so they can be passed down to the view or to the data access
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRespository employeeRespository;
        private readonly ICompanyRespository companyRespository;
       

        public EmployeeService(IEmployeeRespository employee, 
            ICompanyRespository companyRespository)
        {
            employeeRespository = employee;
            this.companyRespository = companyRespository;
            
        }

        public  int Add(Employee employee)
        {
            
            employeeRespository.Add(employee);
            return 0;

        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var result = await employeeRespository.Delete(id);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }

        }

        public Task<Employee> Get(int Id)
        {
            return employeeRespository.Get(Id);
        }

        public Task<IEnumerable<Employee>> getAll()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Employee> GetEmployeesFromCompany(int id)
        {
            //verkrijg alle employees van de Company

            var employees = companyRespository.GetEmployeesFromCompany(id).Result.Employees;

            return employees;

            
            // Krijg de company van de 

            
        }

        public async Task<Employee> Update(Employee employee)
        {
            await employeeRespository.Update(employee);
            return employee;
        }
    }

    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> getAll(); // get all employees in de datatbase van het gebouw

        Task<Employee> Get(int Id);

        Task<Employee> Update(Employee employee);

        int Add(Employee employee);

        Task<bool> Delete(int id);

        IEnumerable<Employee> GetEmployeesFromCompany(int id);

        
    }
}
