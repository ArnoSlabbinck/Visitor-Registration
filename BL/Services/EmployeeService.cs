
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;
using VisitorRegistrationApp.Data.Repository;
using VisitorRegistrationApp.Helper;

namespace BL.Services
{
    // This layer is for validating entity so they can be passed down to the view or to the data access
    public class EmployeeService : IEmployeeService
    {
        //Maken van validation rules en checken daar op via Guard 
        private readonly IEmployeeRespository employeeRespository;
        private readonly ICompanyRespository companyRespository;
        private readonly IValidator<Employee> validator;
        private readonly ILogger<EmployeeService> logger;
        private IList<string> Errors;
        

        //Validation doen op basis van uw data annotations op uw models 
        public EmployeeService(IEmployeeRespository employee, 
            ICompanyRespository companyRespository, 
            IValidator<Employee> validator, 
            ILogger<EmployeeService> logger)
        {
            employeeRespository = employee;
            this.companyRespository = companyRespository;
            this.validator = validator;
            this.logger = logger;
            
        }

        public async Task<IList<string>> Add(Employee employee)
        {
            ValidationResult validationResult = validator.Validate(employee);
            Errors = Guard.AgainstErrors(validationResult);
            if (Errors.Any() == false)
            {
                var employeeAdded = await employeeRespository.Add(employee);
                logger.LogInformation($"A new employee has been added with id {employeeAdded.Id}, {employeeAdded.Name}, {employeeAdded.Company.Name}");
            }
           
            return Errors;

        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var result = await employeeRespository.Delete(id);
                logger.LogInformation($"The employee with Id {result.Id} has been deleted");
                return true;
            }
            catch(Exception e)
            {
                return false;
            }

        }

        public Task<Employee> Get(int Id)
        {
            Errors = new List<string>() { Guard.AgainstNull(Id, nameof(Id)) };
            if (Errors.All(x => string.IsNullOrEmpty(x)) == true)
                return employeeRespository.Get(Id);
            return null;
        }

        public Task<IEnumerable<Employee>> getAll()
        {
            return employeeRespository.GetAll();
        }

        public IEnumerable<Employee> GetEmployeesFromCompany(int id)
        {
            Errors = new List<string>() { Guard.AgainstNull(id, nameof(id)) };
            if (Errors.All(x => string.IsNullOrEmpty(x)) == true)
                return companyRespository.GetEmployeesFromCompany(id).Result.Employees;

            return null; 
            
        }

        public async Task<IList<string>> Update(Employee employee)
        {

            ValidationResult validationResult = validator.Validate(employee);
            Errors = Guard.AgainstErrors(validationResult);
            if(Errors.Any() == false)
            {
                await employeeRespository.Update(employee);
                logger.LogInformation($"The employee has been updated with id {employee.Id}, {employee.Name}, {employee.Company.Name}");
            }
            return Errors;
        }
    }

    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> getAll(); // get all employees in de datatbase van het gebouw

        Task<Employee> Get(int Id);

        Task<IList<string>> Update(Employee employee);

        Task<IList<string>> Add(Employee employee);

        Task<bool> Delete(int id);

        IEnumerable<Employee> GetEmployeesFromCompany(int id);

        
    }
}
