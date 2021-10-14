using BLL.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;
using VisitorRegistrationApp.Data.Repository;
using VisitorRegistrationApp.Helper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using DAL.Repositories;
using Model;

namespace BL.Services
{
    public class CompanyService : ICompanyService 
    {
        private readonly ICompanyRespository companyRes;
        private readonly IValidator<Company> validator;
        private readonly ILogger<CompanyService> logger;
        private readonly IImageRespository imageRespository;
        private IList<string> Errors;


        public CompanyService(ICompanyRespository companyRes,
            IValidator<Company> validator, 
            ILogger<CompanyService> logger, 
            IImageRespository  imageRespository)
        {
            this.companyRes = companyRes;
            this.validator = validator;
            this.logger = logger;
            this.imageRespository = imageRespository;
       
        }
        public Building GetBuilding()
        {
            
            return companyRes.getBuilding();
            
        }

        //Ophalen van alle companies

        public  async Task<IEnumerable<Company>> getAll()
        {
            return await Task.FromResult(companyRes.GetAll().AsEnumerable());
        }

        //Ophalen van 1Company
        
        public async Task<Company> Get(int Id)
        {
            Errors = new List<string>() { Guard.AgainstNull(Id, nameof(Id)) };
            if(Errors.All(x => string.IsNullOrEmpty(x)) == true)
                return await companyRes.GetCompanyWithImageAndEmployees(Id);
            return null;
        }

        //Update van een company

        public async Task<IList<string>>  Update(Company company, byte[] ImageFile)
        {
            company.Building = companyRes.getBuilding();
            Image image = new Image() { ImageFile = ImageFile, ImageName = $"{company.Name}Image" };
            Image addedImage =  await imageRespository.Add(image);
            company.PictureId = addedImage.Id;
            ValidationResult validationResult = validator.Validate(company);
            Errors = Guard.AgainstErrors(validationResult);
            if (Errors.Any() == false)
            {
                var companyAdded  = await companyRes.Update(company);
                logger.LogInformation($"The company has been updated with id {companyAdded.Id}, {companyAdded.Name}");
            }    
                
            return Errors;
        }

        // Creeeren van company

        public async Task<IList<string>> Add(Company company)
        {
            company.Building = companyRes.getBuilding();
            ValidationResult validationResult = validator.Validate(company);
            Errors = Guard.AgainstErrors(validationResult);
            if (Errors.Count() == 0)
            {
                await Task.FromResult(companyRes.Add(company));
                logger.LogInformation($"The company has been added with id {company.Id}, {company.Name}");
                return Errors;
            }
            else
            {
                var errorSummary  = Guard.AllLoggingErrors(Errors);
                logger.LogError(errorSummary);
                return Errors;

            }
           
        }

        //Verwijderen van een company

        public async Task<IList<string>> Delete(int id)
        {
            Errors = new List<string>() { Guard.AgainstNull(id, nameof(id)) };
            if (Errors.All(x => string.IsNullOrEmpty(x)) == true)
            {
                var company = await Task.FromResult(companyRes.Delete(id));
                logger.LogInformation($"The company {company.Result.Id}, {company.Result.Name} has been deleted");
                return Errors;
            }
            else
            {
                var errorSummary = Guard.AllLoggingErrors(Errors);
                logger.LogError(errorSummary);
                return Errors;
            }
            
            
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

        Task<IList<string>> Update(Company company, byte[] ImageFile);

        Task<IList<string>> Add(Company company);

        Task<IList<string>> Delete(int id);

        IEnumerable<Company> SearchByName(string searchTerm);
    }
}
