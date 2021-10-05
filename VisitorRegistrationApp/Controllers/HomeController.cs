﻿using AutoMapper;
using BL.Services;
using BLL.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data;
using VisitorRegistrationApp.Data.Helper;
using VisitorRegistrationApp.Helper;
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
       
        public IActionResult Picture()
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
        public IActionResult ThankYou()
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
                case "Upcoming":
                    return RedirectToAction(" UpComing");
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

        public IActionResult ConfirmSignOut(string name)
        {
            // terug naar hoofdscherm sturen 
            // Veranderen van status visitor in de database => Veranderen van CheckIn naar CheckOut
            bool result;
            if(!string.IsNullOrEmpty(name))
            {
                result = visitorService.CheckOut(name).Result;
                string VisitorName = User.Identity.Name;
                _logger.LogInformation($"{VisitorName} is now signout from the lobby");
            }


            return RedirectToAction(nameof(ThankYou));
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Search([FromForm] string searchInput, string logout)
        {
            //Custom mapper ==> loskoppelen van data tussen viewmodel en model 
            // functioneel mappen => name convention based

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

        public async Task<IActionResult> SignIn()
        {
            // Uit session halen van object
            // Deserialize
            // Encoded password for security
            
            var visitorView = HttpContext.Session.GetObject<ApplicationUser>("CurrentVisitor");
            var password = HttpContext.Session.GetString("Password");
            var imageBase64 = HttpContext.Session.GetString("ImageBase64");
            var user = mapper.Map<ApplicationUser>(visitorView);

            var checkSignedIn = await Task.FromResult(visitorService.SignIn(user, imageBase64, password));
            
            if(checkSignedIn.Result == true)
            {
                return RedirectToAction("ThankYou ", "Home");
            }

            return RedirectToAction("Index", "Home");
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

        public IActionResult UpComing()
        {
            //Visitors kunnen een planning maken voor in de toekomst
            // Via het kiezen van een calendar kunnen ze kiezen welke afspraak
            // Contact persoon
            // Maken van api call voor google calendar om calendar te kunne zien 
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Delivery()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Summary()
        {
            //Uit session gaan halen van VisitorViewModel
            // Displayen aan de user 
            var visitor = HttpContext.Session.GetObject<ApplicationUser>("CurrentVisitor");
            var visitorView = mapper.Map<VisitorViewModel>(visitor);
            visitorView.ChosenPurpose = "Visitor"; 


            return View(visitorView);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Summary(bool correctUserData)
        {
            
            // Hier krijg ik de bevestiging binnen dat alle gegevens correct zijn
            return RedirectToAction(nameof(SignIn));
        }

        [HttpPost]

        public async Task<IActionResult> RedirectToVisitorProfile([FromBody] RedirectToVisitorProfileData data)
        {

            if (data == null || data.ImageUrl == null || data.ImageUrl.Length == 0)
            {
                return BadRequest();
            }
            HttpContext.Session.SetString("ImageBase64", data.ImageUrl);


            return RedirectToAction(nameof(Summary));
        }

       
        private List<VisitorViewModel> MapApplicationUserToUserViewModel(List<ApplicationUser> applicationUsers)
        {

            return mapper.Map<List<VisitorViewModel>>(applicationUsers);
        }
    }
    public class RedirectToVisitorProfileData
    {
        public string DateImage { get; set; }

        public string ImageUrl { get; set; }
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
