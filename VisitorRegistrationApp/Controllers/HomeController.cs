using AutoMapper;
using BL.Services;
using BLL.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
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
        private readonly IPhotoService photoService;
        public HomeController(ILogger<HomeController> logger, 
            UserManager<ApplicationUser> userManager, 
            IMapper mapper, 
            IVisitorService visitorService, 
            IPhotoService photoService)
        {
            _logger = logger;
            this.mapper = mapper;
            this.visitorService = visitorService;
            this.photoService = photoService;
            
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
                    return RedirectToAction("SignOut");
                case "Delivery":
                    return RedirectToAction("Delivery"); 
                    // Scannen van een package en dan een melding geven naar de persoon in de Building
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

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Search([FromForm] string searchInput, string logout)
        {
            List<VisitorViewModel> VisitorsList = new List<VisitorViewModel>();
            if (!string.IsNullOrEmpty(logout))// When a visitor signs out
            {
                VisitorsList = MapApplicationUserToUserViewModel(visitorService.SearchSpecificUsers(searchInput));
                TempData["VisitorsViews"] = JsonConvert.SerializeObject(VisitorsList);
                
                return RedirectToAction(nameof(SignOut), new { firstTime = false});
                
            }

            
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

        public  IActionResult SignOut(IEnumerable<VisitorViewModel> visitorViews, bool firstTime = true)
        {
            //
            if(firstTime == true)
            {
                TempData["VisitorsViess"] = null;
            }
            else
            {
                var JsonVisitorViews = (string)TempData["VisitorsViews"];
                visitorViews = JsonConvert.DeserializeObject<List<VisitorViewModel>>(JsonVisitorViews, new VisitorViewModelListConverter());

            }

            return View(visitorViews);
        }


      
        public IActionResult Delivery()
        {
            return View();
        }

       
        private List<VisitorViewModel> MapApplicationUserToUserViewModel(List<ApplicationUser> applicationUsers)
        {

            return mapper.Map<List<VisitorViewModel>>(applicationUsers);
        }
    }

    internal class VisitorViewModelListConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new System.NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IEnumerable<VisitorViewModel>).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject item = JObject.Load(reader);

                if (item["visitors"] != null)
                {
                    var users = item["visitors"].ToObject<IList<VisitorViewModel>>(serializer);

                  
                    return new List<VisitorViewModel>(users);
                }
            }
            else
            {
                JArray array = JArray.Load(reader);

                var users = array.ToObject<IList<VisitorViewModel>>();

                return new List<VisitorViewModel>(users);
            }

            // This should not happen. Perhaps better to throw exception at this point?
            return null;
        }
    }


}
