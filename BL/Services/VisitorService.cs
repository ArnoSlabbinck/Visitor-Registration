using DAL.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data;

namespace BL.Services
{
    public class VisitorService : IVisitorService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IVisitorRepository visitorRepository;
        public VisitorService(UserManager<ApplicationUser> userManager, 
            IVisitorRepository visitorRepository)
        {
            this.userManager = userManager;
            this.visitorRepository = visitorRepository;
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
            var splittedName = GoodNameFormat(name);
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



        public string[] GoodNameFormat(string name)
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
    }

    public interface IVisitorService
    {
        List<ApplicationUser> SearchSpecificUsers(string searchInput);

        Task<bool> CheckOut(string name);

        string[] GoodNameFormat(string name);


    }
}
