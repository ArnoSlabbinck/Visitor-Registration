using BLL.Helper;
using DAL.Repositories;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data;
using VisitorRegistrationApp.Data.Repository;
using VisitorRegistrationApp.Helper;

namespace BL.Services
{
    //Nog toevoegen van Fluentvalidation
    public class VisitorService : IVisitorService
    {
        public readonly UserManager<ApplicationUser> userManager;
        private readonly IVisitorRepository visitorRepository;
        private readonly IValidator<ApplicationUser> validator;
        private readonly ILogger<VisitorService> logger;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ICompanyRespository companyRespository;
        private readonly IEmployeeRespository employeeRespository;
        private IList<string> Errors;



        public VisitorService(UserManager<ApplicationUser> userManager,
            IVisitorRepository visitorRepository,
            IValidator<ApplicationUser> validator, 
            ILogger<VisitorService> logger,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, 
            ICompanyRespository company, 
            IEmployeeRespository employeeRespository
            )
        {
            this.userManager = userManager;
            this.visitorRepository = visitorRepository;
            this.validator = validator;
            this.logger = logger;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            companyRespository = company;
            this.employeeRespository = employeeRespository;

        }
   
        public async Task<List<ApplicationUser>> SearchForSpecificUsers(string searchInput)
        {
            //Ophalen van alle
            searchInput = searchInput.Trim().ToLower();
            List<ApplicationUser> AllUsers = await userManager.Users
                .Include(c => c.VisitingCompany).Include(e => e.Host).ToListAsync();

            List<ApplicationUser> SearchedVisitorUsers = new List<ApplicationUser>();
            // Haal de users eruit die niet behoren bij searchInput
            Regex patternEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Regex patternFullName = new Regex(@"^([a-zA-Z]{2,}\s[a-zA-Z]{1,}'?-?[a-zA-Z]{2,}\s?([a-zA-Z]{1,})?)");
            Regex patternFirstLastCompanyName = new Regex(@"[a-z]");

            if (patternEmail.IsMatch(searchInput))
            {
                return AllUsers.Where(u => u.Email == searchInput).ToList();

            }
            else if (patternFullName.IsMatch(searchInput))
            {
                return AllUsers.Where(u => u.Fullname == searchInput).ToList();
            }
            else if (patternFirstLastCompanyName.IsMatch(searchInput))
            {
                return AllUsers.Where(u => u.FirstName.ToLower().Trim() == searchInput
                || u.LastName.ToLower().Trim() == searchInput || searchInput == u.VisitingCompany.Name.ToLower()).ToList();
            }

            return SearchedVisitorUsers;

        }

        public async Task<bool> ConfirmCheckOutForVisitor(string name)
        {
            var splittedName = SplitFullnameInFirstAndLastNameInLowercase(name);
            var firstName = splittedName[0].Trim();
            var lastName = splittedName[1].Trim();
            var visitor = visitorRepository.getUserByNameWithCompanyAndHosts(firstName, lastName);

            if (visitor.VisitStatus != VisitStatus.CheckOut)
            {
                visitor.VisitStatus = VisitStatus.CheckOut;
                await visitorRepository.Update(visitor);
                return true;
            }
   
            return false;
        }

        public List<ApplicationUser> DeleteAllCheckOutVisitorsFromList(List<ApplicationUser> visitors)
        {
            visitors.RemoveAll(x => x.VisitStatus == VisitStatus.CheckOut);
            return visitors;
        }


        public string[] SplitFullnameInFirstAndLastNameInLowercase(string name)
        {
            string[] splittedName = new string[2]; 
            var Name = name.Trim().ToLower();
            int Startindex = Name.IndexOf(" ");
            // Weten wat de lengte is van lege ruimtes 
            int lengthEmptySpaces = 0; 
            for(int i = Startindex; i < Name.Length; i++)
            {
                if(char.IsLetter(Name[i]))
                {
                    break; 
                }
                lengthEmptySpaces++;
            }
            string SpacesBetweenName = Name.Substring(Startindex, lengthEmptySpaces);
            if(lengthEmptySpaces < 2)
            {
               
                int counter = 0;
                var splitName = Name.Split(SpacesBetweenName);
                foreach (var namePiece in splitName)
                {
                    if(counter == 0)
                    {
                        splittedName[0] = namePiece;
                    }
                    else
                    {            
                        splittedName[1] += namePiece + " ";
                    }
                    counter++;
                }
                return splittedName;

            }
            else
            {
                return Name.Split(SpacesBetweenName);
            }
          
        }

        public async  Task<IEnumerable<ApplicationUser>> getAll()
        {
            return await Task.FromResult(visitorRepository.GetAll().AsEnumerable()); 
        }

        public Task<ApplicationUser> Get(int Id)
        {
            Errors = new List<string>() { Guard.AgainstNull(Id, nameof(Id)) };
            if (Errors.All(x => string.IsNullOrEmpty(x)) == true)
                return visitorRepository.Get(Id);
            return null;
        
        }

        public async Task<ApplicationUser> Update(ApplicationUser applicationUser, string CompanyName)
        {
            ValidationResult validationResult = validator.Validate(applicationUser);
            Errors = Guard.AgainstErrors(validationResult);
            if (Errors == null )
            {
                var Company = await companyRespository.GetCompanyByName(CompanyName);
                applicationUser.VisitingCompany = Company;
                var visitorAdded = await visitorRepository.Update(applicationUser);
                logger.LogInformation($"The visitor has been updated with id {visitorAdded?.Id}, {visitorAdded?.Fullname}");
                return visitorAdded;
            }

            return null;

           
        }

        public async Task<ApplicationUser> Add(ApplicationUser applicationUser)
        {
            ValidationResult validationResult = validator.Validate(applicationUser);
            Errors = Guard.AgainstErrors(validationResult);
            if (Errors == null)
            {
                var visitorAdded = await visitorRepository.Add(applicationUser);
                logger.LogInformation($"The visitor has been updated with id {visitorAdded?.Id}, {visitorAdded?.Fullname}");
                return visitorAdded;
            }
            return null;
            

            
        }

        public Task<bool> Delete(int id)
        {
            var error = Guard.AgainstNullOrWhiteSpace(id, nameof(id)) == null ? false : true;
            if (error)
                return Task.FromResult(false);

            visitorRepository.Delete(id);
            return Task.FromResult(true);
        }

        public ApplicationUser GetUserFromName(string name)
        {
            var error = Guard.AgainstNullOrWhiteSpace(name, nameof(name)) == null ? false : true;
            if (error)
                return null;
            var splitName = SplitFullnameInFirstAndLastNameInLowercase(name);
            var firstName = splitName[0].Trim();
            var lastName = splitName[1].Trim();

            var visitor = visitorRepository.getUserByNameWithCompanyAndHosts(firstName, lastName);
            int? pictureId = visitor.PictureId;
            if(pictureId != null)
                return visitorRepository.GetUserByNameWithImage(firstName, lastName, (int)pictureId);
            return visitor;

        }

        public bool Delete(string userId)
        {
            var error = Guard.AgainstNullOrWhiteSpace(userId, nameof(userId)) == null ? false : true;
            if (error)
                return false;
            visitorRepository.DeleteUserWithUserId(userId);
            return true;
        }

        public async Task<bool> SignIn(ApplicationUser visitor, string imageBase64, string password, string employeeName)
        {
            IdentityResult roleResult = new IdentityResult();
            Image image = new Image() { ImageName = $"{visitor.Fullname}Picture", ImageFile = ImgToByteConverter.Base64StringToByteArray(imageBase64) };
            visitor.Picture = image;
            try
            {

                visitor.SecurityStamp = Guid.NewGuid().ToString();
                visitor.UserName = visitor.Email;
                visitor.VisitingCompany = await companyRespository.GetCompanyByName(visitor.VisitingCompany.Name);
                visitor.Host = await employeeRespository.GetEmployeeByName(employeeName);

                roleResult =  userManager.CreateAsync(visitor, password).GetAwaiter().GetResult();
                if (roleResult.Succeeded)
                {
                    var roleName = "Visitor";
                    // Creeren van nieuwe rol voor de user => Default is dat User
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (roleExist.Equals(false))
                    {
                        roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                        await userManager.AddToRoleAsync(visitor, roleName);
                        logger.LogInformation("Visitor can now visit the building.");


                        return true;

                    }
                    await userManager.AddToRoleAsync(visitor, roleName);
                    logger.LogInformation("Visitor can now visit the building.");

                    return true;


                }

            }
            catch(Exception e)
            {
                Console.WriteLine(roleResult.Errors);
                logger.LogError(e.Message);
                return false;
            }
           

            return false;
        }

        public async Task<IEnumerable<ApplicationUser>> GetSignedInVisitors()
        {
            // Houd vullen vna het ordenen van de logica moet in de service laag gebeuren
            // Het ordenen, filteren En andere logica moet ik terug vinden in de service laag

            return visitorRepository.GetVisitorsWithCompanyAndHots().Where(u => u.VisitStatus == VisitStatus.CheckIn);
           
            
        }

        public async Task<IEnumerable<ApplicationUser>> GetSignedOutVisitors()
        {
            return visitorRepository.GetVisitorsWithCompanyAndHots().Where(u => u.VisitStatus == VisitStatus.CheckOut);

        }

        public async Task<ApplicationUser> GetLatestVisitor()
        {
            return await userManager.Users.OrderByDescending(u => u.CheckIn).FirstOrDefaultAsync();
        }
    }

    public interface IVisitorService
    {
        Task<List<ApplicationUser>> SearchForSpecificUsers(string searchInput);

        Task<bool> ConfirmCheckOutForVisitor(string name);

        string[] SplitFullnameInFirstAndLastNameInLowercase(string name);

        ApplicationUser GetUserFromName(string name);

        Task<IEnumerable<ApplicationUser>> getAll(); 

        Task<ApplicationUser> Get(int Id);

        Task<ApplicationUser> Update(ApplicationUser applicationUser, string CompanyName);

        Task<ApplicationUser> Add(ApplicationUser applicationUser);

        Task<bool> Delete(int id);

        bool Delete(string userId);
        public List<ApplicationUser> DeleteAllCheckOutVisitorsFromList(List<ApplicationUser> visitors);
        Task<bool> SignIn(ApplicationUser visitor, string imageBase64, string password, string employeeName);

        Task<ApplicationUser> GetLatestVisitor();

        Task<IEnumerable<ApplicationUser>> GetSignedInVisitors();
        Task<IEnumerable<ApplicationUser>> GetSignedOutVisitors();




    }
}
