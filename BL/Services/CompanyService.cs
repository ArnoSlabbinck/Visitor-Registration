using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;
using VisitorRegistrationApp.Data.Repository;

namespace BL.Services
{
    public class CompanyService : ICompanyService, IDisposable 
    {
        private readonly ICompanyRespository companyRes;
        private bool _disposed = false;
        public CompanyService(ICompanyRespository companyRes)
        {
            this.companyRes = companyRes;
        }
        public Building GetBuilding()
        {
            return companyRes.getBuilding();
            
        }

        //Ophalen van alle companies

        public  Task<IEnumerable<Company>> getAll()
        {
            return companyRes.GetAll();
        }

        //Ophalen van 1Company
        
        public async Task<Company> Get(int Id)
        {
            return await companyRes.Get(Id);
        }

        //Update van een company

        public async void Update(Company company)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("The current instance has been disposed!");
            }
            await companyRes.Update(company);
        }

        // Creeeren van company

        public async void Add(Company company)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("The current instance has been disposed!");
            }
            await companyRes.Add(company);
        }

        //Verwijderen van een company

        public async void Delete(int id)
        {
            await companyRes.Delete(id);
        }

        public void Dispose()
        {
            _disposed = true;
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
    }
}
