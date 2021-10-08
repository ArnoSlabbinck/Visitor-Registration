using BLL.Helper;
using DAL.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data;
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
        private IList<string> Errors;



        public VisitorService(UserManager<ApplicationUser> userManager,
            IVisitorRepository visitorRepository,
            IValidator<ApplicationUser> validator, 
            ILogger<VisitorService> logger,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            this.userManager = userManager;
            this.visitorRepository = visitorRepository;
            this.validator = validator;
            this.logger = logger;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;

        }
   
        public List<ApplicationUser> SearchSpecificUsers(string searchInput)
        {
            searchInput = searchInput.Trim().ToLower();
            List<ApplicationUser> AllUsers = userManager.Users.ToList();

            List<ApplicationUser> SearchedVisitorUsers = new List<ApplicationUser>();
            // Haal de users eruit die niet behoren bij searchInput
            Regex patternEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Regex patternName = new Regex(@"^([a-zA-Z]{2,}\s[a-zA-Z]{1,}'?-?[a-zA-Z]{2,}\s?([a-zA-Z]{1,})?)");
            Regex patternCompanyFirstName = new Regex(@"[a-z]");

            if (patternEmail.IsMatch(searchInput))
            {
                return AllUsers.Where(u => u.Email == searchInput).ToList();

            }
            else if (patternName.IsMatch(searchInput))
            {
                return AllUsers.Where(u => u.Fullname == searchInput).ToList();
            }
            else if (patternCompanyFirstName.IsMatch(searchInput))
            {
                return AllUsers.Where(u => u.FirstName.ToLower() == searchInput
                || u.LastName.ToLower() == searchInput).ToList();
            }

            return SearchedVisitorUsers;

        }

        public async Task<bool> CheckOut(string name)
        {
            var splittedName = SplitFullnameInFirstAndLast(name);
            var firstName = splittedName[0].Trim();
            var lastName = splittedName[1].Trim();
            var visitor = visitorRepository.getUserByName(firstName, lastName);

            if (visitor.VisitStatus != VisitStatus.CheckOut)
            {
                visitor.VisitStatus = VisitStatus.CheckOut;
                await visitorRepository.Update(visitor);
            }
   
            return true;
        }



        public string[] SplitFullnameInFirstAndLast(string name)
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

        public Task<IEnumerable<ApplicationUser>> getAll()
        {
            return visitorRepository.GetAll(); 
        }

        public Task<ApplicationUser> Get(int Id)
        {
            Errors = new List<string>() { Guard.AgainstNull(Id, nameof(Id)) };
            if (Errors.All(x => string.IsNullOrEmpty(x)) == true)
                return visitorRepository.Get(Id);
            return null;
        
        }

        public Task<ApplicationUser> Update(ApplicationUser applicationUser)
        {
            return visitorRepository.Update(applicationUser); 
        }

        public Task<ApplicationUser> Add(ApplicationUser applicationUser)
        {
            return visitorRepository.Add(applicationUser);
        }

        public Task<bool> Delete(int id)
        {
            
            visitorRepository.Delete(id);
            return Task.FromResult(true);
        }

        public ApplicationUser GetUserFromName(string name)
        {
            var splitName = SplitFullnameInFirstAndLast(name);
            var firstName = splitName[0].Trim();
            var lastName = splitName[1].Trim();

            return visitorRepository.getUserByName(firstName, lastName);

        }

        public void Delete(string userId)
        {
            visitorRepository.DeleteUserWithUserId(userId);
        }

        public async Task<bool> SignIn(ApplicationUser visitor, string imageBase64, string password)
        {
            IdentityResult roleResult;
            visitor.Picture = new Model.Image() { ImageName = $"{visitor.Fullname}Picture", ImageFile = ImgToByteConverter.Base64StringToByteArray(imageBase64) }; 
            var result = await userManager.CreateAsync(visitor, password);
            if (result.Succeeded)
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

            return false;
        }
    }

    public interface IVisitorService
    {
        List<ApplicationUser> SearchSpecificUsers(string searchInput);

        Task<bool> CheckOut(string name);

        string[] SplitFullnameInFirstAndLast(string name);

        ApplicationUser GetUserFromName(string name);

        Task<IEnumerable<ApplicationUser>> getAll(); 

        Task<ApplicationUser> Get(int Id);

        Task<ApplicationUser> Update(ApplicationUser applicationUser);

        Task<ApplicationUser> Add(ApplicationUser applicationUser);

        Task<bool> Delete(int id);

        void Delete(string userId);

        Task<bool> SignIn(ApplicationUser visitor, string imageBase64, string password);





    }
}
