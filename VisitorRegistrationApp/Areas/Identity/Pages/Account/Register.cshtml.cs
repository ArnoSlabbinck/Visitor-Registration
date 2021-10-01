using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using VisitorRegistrationApp.Data;
using VisitorRegistrationApp.Data.Entities;
using VisitorRegistrationApp.Data.Repository;

namespace VisitorRegistrationApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    [BindProperties]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ICompanyRespository companyRepository;
        private readonly IEmployeeRespository employeeRepository;
        private readonly RoleManager<IdentityRole> roleManager;
       



        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            ICompanyRespository companyRepository,
            IEmployeeRespository employeeRepository, 
            RoleManager<IdentityRole> roleManager 
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
           
            this.companyRepository = companyRepository;
            this.employeeRepository = employeeRepository;
            this.roleManager = roleManager;
            MultipleAppointmentsWith = new List<Employee>();
            Input = new InputModel();
            SeedAllCompanies();
            
        }

        [BindProperty]
        public InputModel Input { get; set; } 

        public string ReturnUrl { get; set; }

        public  List<Employee> MultipleAppointmentsWith { get; set; }

        public static Employee Employee { get; set; }
      

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public JsonResult OnGetEmployees(string selectedCompany)
        {
            Input.AllEmployeesOfCompany = null;
            

            Input.AllEmployeesOfCompany = employeeRepository.GetAll().Result
                .Where(e => e.Company.Name == selectedCompany)
                .Select(p => new SelectListItem
                {
                    Text = p.Name,
                    Value = p.Name
                });

            return new JsonResult(Input.AllEmployeesOfCompany);
            // Return geven van json  and displayen in de 

        }

        public JsonResult  OnGetSetEmployee(string employee)
        {

            Employee = employeeRepository.GetAll().Result.Where(e => e.Name == employee).SingleOrDefault();
            MultipleAppointmentsWith.Add(Employee);
            return new JsonResult(Input.Email);

        }

        public void SeedAllCompanies()
        {
           
            Input.AllCompanies = companyRepository.GetAll().Result.Select(c =>
                 new SelectListItem
                 {
                     Value = c.Name,
                     Text = c.Name,
                     Selected = c.Name.Contains("Allphi")

                 });
        
        }
        
        public JsonResult OnGetReloadPage()
        {
            var AllphiCompany = companyRepository.GetEmployeesFromCompany("Allphi").Result;
            // Ik moet alleen maar de Allphi employees vullen
            Input.AllEmployeesOfCompany = AllphiCompany.Employees.AsEnumerable().Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Name
            });
            return new JsonResult(Input.AllEmployeesOfCompany);
        }


        public InputModel GiveDefaultValueToAppointedEmployeeWhenNotGiven(InputModel input)
        {
            //Kijk naar welke Company dat geselecteerd is 
            // Dan pak de eerste value van die Company via InputModel 
            Input.ApppointmentWith = companyRepository.GetAll().Result.SingleOrDefault(c => c.Name == input.VisitedCompany).Employees.SingleOrDefault();
            return input;
         

        }


        public class InputModel
        {
            

            [Required(ErrorMessage ="You need to fill your Firstname")]
            [StringLength(maximumLength: 50, MinimumLength = 0,
        ErrorMessage = "Your firstname  should have {1} maximum characters and {2} minimum characters")]
            [Display(Name = "Firstname")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "You need to fill your Lastname")]
            [Display(Name = "Lastname")]
            [StringLength(maximumLength: 50, MinimumLength = 0,
        ErrorMessage = "Your firstname  should have {1} maximum characters and {2} minimum characters")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "You need to give an gender")]
            [Display(Name = "Gender")]
            public bool Gender { get; set; } 
          

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]                                                           
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; } = "Cartoon-Duck-14-Coffee-Glvs";  // Password is tijdelijk undefined

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = "Cartoon-Duck-14-Coffee-Glvs";


            [Required]
            [Phone]
            public string PhoneNumber { get; set; }
            
            

            [Required(ErrorMessage = "You need to give a starttime for your visit")]
            [Display(Name = "Visit Starttime")]
            //[Range(typeof(DateTime), DateTime.Now.ToString(), DateTime]
            public string Starttime { get; set; } = DateTime.Now.ToString("HH:mm");
            [Required(ErrorMessage = "You need to give a endTime for your visit")]
            [Display(Name = "Visit Endtime")]
            public string EndTime { get; set; } = DateTime.Now.ToString("HH:mm");
            [Required(ErrorMessage = "You need to give registration day")]
            [Display(Name = "Day Of Visit")]
            //[Range(typeof(DateTime), DateTime.Now.ToString(), DateTime.Today.AddDays(((int)DateTime.Today.DayOfWeek - (int)DayOfWeek.Monday) + 7).ToString(), ErrorMessage = "You can't plan your meeting more than one week in advance" ]

            private DateTime? registrationDate = DateTime.Now.Date;
            public DateTime? RegistrationDay {
                get { return registrationDate; }
                set {
                    registrationDate = value;                  
                } 
            } 

    
  

            [Required(ErrorMessage = "You need to select a company")]
            [BindProperty]
            [Display(Name = "Visit Company")]
            public string VisitedCompany { get; set; } // Get the company visit

      
            public IEnumerable<SelectListItem> AllCompanies { get; set; }
            
            public Employee ApppointmentWith { get; set; }

            private int visitedDifference;
            [Range(0, 300, ErrorMessage = "Your meeting can't be over 5 hours long")]
            public int VisitedDifference { 
                get { return visitedDifference; } 
                set { visitedDifference = (Convert.ToDateTime(EndTime).Minute * Convert.ToDateTime(EndTime).Hour) - (Convert.ToDateTime(Starttime).Minute * Convert.ToDateTime(EndTime).Hour);  } 
               }

            
            

            public IEnumerable<SelectListItem> AllEmployeesOfCompany { get; set; }

            private string CalculateOneWeekFromNow()
            {
                int day = DateTime.Now.Day;
                int month = DateTime.Now.Month;
                int year = DateTime.Now.Year;

                if (day + 7 <= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month))
                    day += 7;
                else
                {
                    day = day + 7 - DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                    month += 1;
                }
                return $"{day}/{month}/{year}";
            }



        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();


            //Invullen van Companies 
            
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            Input.ApppointmentWith = Employee;

            //Checken van dat de opgegeven uren wel correct zijn
            //De startuur mag niet vroeger zijn dan current time checken op de client side
            // Het minuten verschil moet tussen de 30 minuten zijn  => EndTime - StartTime  > 30
            //Je kan geen 
            IdentityResult roleResult;

            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if(Input.Gender == false)
            {
                Input.Gender = false;
            }

            if (ModelState.IsValid)
            {
                
                if(Input.ApppointmentWith == null)// If there's no employee selected from the list
                {
                     Input = GiveDefaultValueToAppointedEmployeeWhenNotGiven(Input);
                }
                
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, FirstName = Input.FirstName,
                    LastName = Input.LastName ,Gender = Input.Gender, VisitDay = Input.RegistrationDay, CheckIn = Convert.ToDateTime(Input.Starttime), CheckOut = Convert.ToDateTime(Input.EndTime), 
                    AppointmenrWith = MultipleAppointmentsWith
                };
        
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    var roleName = "Visitor";
                    // Creeren van nieuwe rol voor de user => Default is dat User
                    var roleExist = await roleManager.RoleExistsAsync(roleName);
                    if (roleExist.Equals(false))
                    {
                        roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                        await _userManager.AddToRoleAsync(user, roleName);
                        _logger.LogInformation("Visitor can now visit the building.");


                        return RedirectToAction("SignIn", "Home");




                    }
                    await _userManager.AddToRoleAsync(user, roleName);
                    _logger.LogInformation("Visitor can now visit the building.");


                    return RedirectToAction("SignIn", "Home");

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            SeedAllCompanies(); 
            return Page();
        }

        //Bereken van 1 Week later

    
    }




}
