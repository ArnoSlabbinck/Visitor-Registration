using AutoMapper;
using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using VisitorRegistrationApp.Data;
using VisitorRegistrationApp.Models;

namespace VisitorRegistrationApp.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper mapper;
        private readonly IVisitorService visitorService;
        public HomeController(ILogger<HomeController> logger, 
            UserManager<ApplicationUser> userManager, 
            IMapper mapper, 
            IVisitorService visitorService)
        {
            _logger = logger;
            this.mapper = mapper;
            this.visitorService = visitorService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Welcome()
        {
            return View(); 
        }
       
        public IActionResult Service()
        {
            VisitorViewModel visitorViewModel = new VisitorViewModel();

            visitorViewModel.Purpose = new List<SelectListItem> 
            {
                     new SelectListItem { Text = "Visitor", Value = "Visitor" },
                     new SelectListItem { Text = "Making a Delivery", Value = "Delivery" },
                     new SelectListItem { Text = "Siging Out", Value = "SignOut" }
            };

            
            return View(visitorViewModel);
        }
        [Authorize]
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Service( [FromForm] VisitorViewModel visitorViewModel)
        {

            switch (visitorViewModel.ChosenPurpose)
            {
                case "Visitor":
                    return RedirectToPage("/Account/Register", new { area = "Identity" });
                case "SignOut":
                    return RedirectToAction("Index");
                case "Delivery":
                    return RedirectToAction("Index");
            }
            return NotFound();
        }
        public IActionResult SearchVisitor(string Email)
        {
            return View();
        }
        [Authorize(Roles = "Administrator")]
        public IActionResult Remove()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [Authorize(Roles ="Administrator")]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Search([FromForm] string searchInput)
        {
            
            List<VisitorViewModel> VisitorsList = new List<VisitorViewModel>();
            ViewData["Search"] = searchInput;
            if(!string.IsNullOrEmpty(searchInput))
            {

                VisitorsList = MapApplicationUserToUserViewModel(visitorService.SearchSpecificUsers(searchInput));

                return View(VisitorsList);
            }
            else
            {
                return View(VisitorsList);
            }

            

        }



      

       
        private List<VisitorViewModel> MapApplicationUserToUserViewModel(List<ApplicationUser> applicationUsers)
        {

            return mapper.Map<List<VisitorViewModel>>(applicationUsers);
        }
    }

}
