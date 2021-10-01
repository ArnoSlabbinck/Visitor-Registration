using BLL.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;
using VisitorRegistrationApp.Data.Repository;
using VisitorRegistrationApp.Helper;
using FluentValidation;

namespace BL.Services
{
    public class CompanyService : ICompanyService 
    {
        private readonly ICompanyRespository companyRes;
        private readonly IValidator<Company> validator;


        public CompanyService(ICompanyRespository companyRes, IValidator<Company> validator)
        {
            this.companyRes = companyRes;
            this.validator = validator;
       
        }
        public Building GetBuilding()
        {
            
            return companyRes.getBuilding();
            
        }

        //Ophalen van alle companies

        public   Task<IEnumerable<Company>> getAll()
        {
            return companyRes.GetAll();
        }

        //Ophalen van 1Company
        
        public async Task<Company> Get(int Id)
        {
            Guard.AgainstNull(Id, nameof(Id), new Company());
            return await companyRes.GetCompanyWithImageAndEmployees(Id);
        }

        //Update van een company

        public  bool Update(Company company)
        {
            Guard.AgainstNull(company, nameof(company), new Company());
            companyRes.Update(company);
            return true;
        }

        // Creeeren van company

        public  bool Add(Company company)
        {
         
            companyRes.Add(company);
            return true;
        }

        //Verwijderen van een company

        public  bool Delete(int id)
        {
            Guard.AgainstNull(id, nameof(id), new Company());
            companyRes.Delete(id);
            return true;
            
        }

        public IEnumerable<Company> SearchByName(string searchTerm)
        {
            Guard.AgainstNull(searchTerm, nameof(searchTerm), new Company());
            return companyRes.GetOrderedCompanies().Where(o => o.Name.ToLower() == searchTerm.Trim().ToLower());
            
            
        }
    }

    public interface ICompanyService 
    {
        Building GetBuilding();

        Task<IEnumerable<Company>> getAll();

        Task<Company> Get(int Id);

        bool Update(Company company);

        bool Add(Company company);

        bool Delete(int id);

        IEnumerable<Company> SearchByName(string searchTerm);
    }
}
