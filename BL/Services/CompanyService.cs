using BLL.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;
using VisitorRegistrationApp.Data.Repository;
using VisitorRegistrationApp.Helper;

namespace BL.Services
{
    public class CompanyService : ICompanyService 
    {
        private readonly ICompanyRespository companyRes;
     


        public CompanyService(ICompanyRespository companyRes)
        {
            this.companyRes = companyRes;
       
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
            Guard.AgainstNull(Id, nameof(Id));
            return await companyRes.Get(Id);
        }

        //Update van een company

        public  bool Update(Company company)
        {
            Guard.AgainstNull(company, nameof(company));
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
            Guard.AgainstNull(id, nameof(id));
            companyRes.Delete(id);
            return true;
            
        }

        public IEnumerable<Company> SearchByName(string searchTerm)
        {
            Guard.AgainstNull(searchTerm, nameof(searchTerm));
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
