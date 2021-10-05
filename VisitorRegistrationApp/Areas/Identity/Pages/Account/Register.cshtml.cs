using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using VisitorRegistrationApp.Data;
using VisitorRegistrationApp.Data.Entities;
using VisitorRegistrationApp.Data.Repository;
using VisitorRegistrationApp.Helper;
using VisitorRegistrationApp.Models;

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
        private readonly IMapper mapper;
       



        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            ICompanyRespository companyRepository,
            IEmployeeRespository employeeRepository, 
            RoleManager<IdentityRole> roleManager, 
            IMapper mapper
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            this.mapper = mapper;
           
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
            
            

       
    
  

            [Required(ErrorMessage = "You need to select a company")]
            [BindProperty]
            [Display(Name = "Visit Company")]
            public string VisitedCompany { get; set; } // Get the company visit

      
            public IEnumerable<SelectListItem> AllCompanies { get; set; }
            
            public Employee ApppointmentWith { get; set; }


            
            

            public IEnumerable<SelectListItem> AllEmployeesOfCompany { get; set; }

        


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
                    LastName = Input.LastName ,Gender = Input.Gender, 
                    AppointmenrWith = MultipleAppointmentsWith
                };

                var visitor = mapper.Map<VisitorViewModel>(user);
                HttpContext.Session.SetObject("CurrentVisitor", visitor); 

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

    
    }




}
