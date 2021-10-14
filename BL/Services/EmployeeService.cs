
using DAL.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Model;
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
        private readonly IImageRespository imageRespository;
        private readonly IValidator<Employee> validator;
        private readonly ILogger<EmployeeService> logger;
        private IList<string> Errors;
        

        //Validation doen op basis van uw data annotations op uw models 
        public EmployeeService(IEmployeeRespository employee, 
            ICompanyRespository companyRespository, 
            IValidator<Employee> validator, 
            ILogger<EmployeeService> logger, 
            IImageRespository imageRespository)
        {
            employeeRespository = employee;
            this.companyRespository = companyRespository;
            this.validator = validator;
            this.logger = logger;
            this.imageRespository = imageRespository;
            
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

        public async Task<IEnumerable<Employee>> getAll()
        {
            return await Task.FromResult(employeeRespository.GetEmployeesWithCompanies());
        }

        public async  Task<IEnumerable<Employee>> GetEmployeesFromCompany(int id)
        {
            Company company;
            Errors = new List<string>() { Guard.AgainstNull(id, nameof(id)) };
            if (Errors.All(x => string.IsNullOrEmpty(x)) == true)
            {
                company = await companyRespository.GetEmployeesFromCompany(id);
                return company.Employees;
            }
            

            return null; 
            
        }

        public async Task<Employee> GetEmployeeWithCompanyAndImage(int CompanyId)
        {
            return await employeeRespository.GetEmployeeWithCompanyAndImage(CompanyId);
        }

        public async Task<IList<string>> Update(Employee employee, byte[] ImageFile)
        {
            Image image = new Image() { ImageFile = ImageFile, ImageName = $"{employee.Name}Image" };
            Image addedImage = await imageRespository.Add(image);
            employee.PictureId = addedImage.Id;
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

        Task<IList<string>> Update(Employee employee, byte[] ImageFile);

        Task<IList<string>> Add(Employee employee);

        Task<bool> Delete(int id);

        Task<IEnumerable<Employee>> GetEmployeesFromCompany(int id);

        Task<Employee> GetEmployeeWithCompanyAndImage(int CompanyId);

        
    }
}
