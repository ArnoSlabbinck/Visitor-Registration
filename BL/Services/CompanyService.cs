using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;
using VisitorRegistrationApp.Data.Repository;

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
            return await companyRes.Get(Id);
        }

        //Update van een company

        public  void Update(Company company)
        {
          
            companyRes.Update(company);
        }

        // Creeeren van company

        public  void Add(Company company)
        {
           
            companyRes.Add(company);
        }

        //Verwijderen van een company

        public  void Delete(int id)
        {
            companyRes.Delete(id);
            
        }

        public IEnumerable<Company> SearchByName(string searchTerm)
        {
            return companyRes.GetOrderedCompanies().Where(o => o.Name.ToLower() == searchTerm.Trim().ToLower());
            
            
        }
    }

    public interface ICompanyService 
    {
        Building GetBuilding();

        Task<IEnumerable<Company>> getAll();

        Task<Company> Get(int Id);

        void Update(Company company);

        void Add(Company company);

        void Delete(int id);

        IEnumerable<Company> SearchByName(string searchTerm);
    }
}
