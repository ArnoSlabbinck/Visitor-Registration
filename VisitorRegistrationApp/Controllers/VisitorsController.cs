using AutoMapper;
using BL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data;
using VisitorRegistrationApp.Data.Helper;
using VisitorRegistrationApp.Helper;
using VisitorRegistrationApp.Models;

namespace VisitorRegistrationApp.Controllers
{
    public class VisitorsController : Controller
    {
        private readonly IVisitorService visitorService;
        private readonly IMapper mapper;
        public VisitorsController(IVisitorService visitorService, 
            IMapper mapper)
        {
            this.visitorService = visitorService;
            this.mapper = mapper;
        }

        // GET: VisitorsController
        public async Task<ActionResult> Index()
        {
            var visitors = await visitorService.getAll();
            var visitorsViewModel = mapper.Map<IEnumerable<VisitorViewModel>>(visitors);
            return View(visitorsViewModel);
        }

        // GET: VisitorsController/Details/5
        public async Task<ActionResult> Details(string name)
        {
             Guard.AgainstNull(name, nameof(name));

            var visitor = await Task.FromResult(visitorService.GetUserFromName(name));
            var visitorViewModel = mapper.Map<VisitorViewModel>(visitor);

            return View(visitorViewModel);
        }


        // GET: VisitorsController/Edit/5
        public async Task<ActionResult> Edit(string name)
        {
            Guard.AgainstNull(name, nameof(name));
            var visitor = await Task.FromResult(visitorService.GetUserFromName(name));
            var visitorViewModel = mapper.Map<VisitorViewModel>(visitor);
            visitorViewModel.CompanyName = visitor.VisitingCompany.Name;
            return View(visitorViewModel);
        }

        // POST: VisitorsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string name, VisitorViewModel visitorViewModel)
        {
            if(ModelState.IsValid)
            {
                var visitor = mapper.Map<ApplicationUser>(visitorViewModel);
                await visitorService.Update(visitor, visitorViewModel.CompanyName);
                return RedirectToAction(nameof(Index));

            }
            return View();
            
        }

        // GET: VisitorsController/Delete/5
        public async Task<ActionResult> Delete(string name)
        {
            Guard.AgainstNull(name, nameof(name));
            var visitor = await Task.FromResult(visitorService.GetUserFromName(name));
            var visitorViewModel = mapper.Map<VisitorViewModel>(visitor);
            return View(visitorViewModel);
        }

        // POST: VisitorsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Delete(string name, VisitorViewModel visitorViewModel)
        {
            if(ModelState.IsValid)
            {
                var visitor = mapper.Map<ApplicationUser>(visitorViewModel);
                visitorService.Delete(visitor.Id);
                return RedirectToAction(nameof(Index));
            }
            return View(name);
        }


        public IActionResult SignedIn()
        {
            // In de service laag moet de Query gevraagd worden 
            var visitors =  visitorService.GetSignedInVisitors();
            var visitorsView = mapper.Map<IEnumerable<VisitorViewModel>>(visitors);


            
            return View(visitorsView);
            
        }

        public IActionResult SignedOut()
        {
            var visitors = visitorService.GetSignedOutVisitors();
            var visitorsView = mapper.Map<IEnumerable<VisitorViewModel>>(visitors);
            return View(visitorsView);
        }
    }
}
