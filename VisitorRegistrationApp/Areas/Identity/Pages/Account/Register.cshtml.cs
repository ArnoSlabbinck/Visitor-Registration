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
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ICompanyRespository companyRepository;
        private readonly IEmployeeRespository employeeRepository;
        private readonly IMapper mapper;
       



        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            ILogger<RegisterModel> logger,
            ICompanyRespository companyRepository,
            IEmployeeRespository employeeRepository, 
            IMapper mapper
            )
        {
            _userManager = userManager;
            _logger = logger;
            this.mapper = mapper;
            this.companyRepository = companyRepository;
            this.employeeRepository = employeeRepository;
           
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
            

            Input.AllEmployeesOfCompany = employeeRepository.GetEmployeesWithCompanies().AsEnumerable()
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

            Employee = employeeRepository.GetAll().Where(e => e.Name == employee).SingleOrDefault();
            MultipleAppointmentsWith.Add(Employee);
            return new JsonResult(Input.Email);

        }

        public void SeedAllCompanies()
        {
           
            Input.AllCompanies = companyRepository.GetAll().Select(c =>
                 new SelectListItem
                 {
                     Value = c.Name,
                     Text = c.Name,
                     Selected = c.Name.Contains("Allphi")

                 });
        
        }

        public async Task<string> GetFirstCompanyInDb()
        {
            return await Task.FromResult(companyRepository.GetAll().FirstOrDefault().Name);
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

            
            public bool UniqueVisitor { get; set; }




            [Required(ErrorMessage = "You need to select a company")]
            [BindProperty]
            [Display(Name = "Visit Company")]
            public string VisitedCompany { get; set; } = "Apphi";

      
            public IEnumerable<SelectListItem> AllCompanies { get; set; }
            
            public Employee ApppointmentWith { get; set; }


            
            

            public IEnumerable<SelectListItem> AllEmployeesOfCompany { get; set; }

        


        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        
            //Invullen van Companies 
            
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            Input.ApppointmentWith = Employee;

        
            if(Input.VisitedCompany == null)
            {
                Input.VisitedCompany = GetFirstCompanyInDb().Result;
            }

            returnUrl ??= Url.Content("~/");
            
           
            if(Input.Gender == false)
            {
                Input.Gender = false;
            }

            if (ModelState.IsValid)
            {

                var company = await companyRepository.GetEmployeesFromCompany(Input.VisitedCompany);
                if (string.IsNullOrWhiteSpace(Input.ApppointmentWith.Name))// If there's no employee selected from the list
                {
                    Input.ApppointmentWith = company.Employees.FirstOrDefault();
                }
              

                MultipleAppointmentsWith.Add(Input.ApppointmentWith);
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, FirstName = Input.FirstName,
                    LastName = Input.LastName, Gender = Input.Gender, VisitingCompany = company,
                    PhoneNumber = Input.PhoneNumber
                };
                
                var visitor = mapper.Map<VisitorViewModel>(user);
                visitor.ChosenPurpose = "Visitor";

                HttpContext.Session.SetObject("CurrentVisitor", visitor );
                HttpContext.Session.SetString("Password", Input.Password);
                HttpContext.Session.SetString("Employee", Input.ApppointmentWith.Name);
           
                return RedirectToAction("Picture", "Home");


            }

            // If we got this far, something failed, redisplay form
            SeedAllCompanies(); 
            return Page();
        }

    
    }




}
