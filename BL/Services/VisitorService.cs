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
        public VisitorService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
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

       


    }

    public interface IVisitorService
    {
        List<ApplicationUser> SearchSpecificUsers(string searchInput);


    }
}
