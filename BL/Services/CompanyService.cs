using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;
using VisitorRegistrationApp.Data.Repository;

namespace BL.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRespository company;
        public CompanyService(ICompanyRespository company)
        {
            this.company = company;

        }
        public Building GetBuilding()
        {
            return company.getBuilding();
            
        }

 
    }

    public interface ICompanyService
    {
        Building GetBuilding();
    }
}
