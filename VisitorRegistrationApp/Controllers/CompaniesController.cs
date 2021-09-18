﻿using AutoMapper;
using BL.Services;
using BLL.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;
using VisitorRegistrationApp.Models;

namespace VisitorRegistrationApp.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly IMapper mapper;
        private readonly ICompanyService companyService;
        private readonly IPhotoService photoService;
        public CompaniesController(IMapper mapper,
            ICompanyService companyService, 
            IPhotoService photoService)
        {
         
            this.mapper = mapper;
            this.companyService = companyService;
            this.photoService = photoService;
        }
        // GET: CompaniesController
        public ActionResult Index()
        {
            //Ophalen van alle companies in Building
            var results = companyService.getAll().Result; 
            
            IEnumerable<CompanyViewModel> AllCompanies = mapper.Map<IEnumerable<CompanyViewModel>>(results); 

            return View(AllCompanies);
        }


       
   
        // GET: CompaniesController/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var Company = companyService.Get((int)id).Result;
            CompanyViewModel company = mapper.Map<CompanyViewModel>(Company);
            return View(company);
        }

        // GET: CompaniesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompaniesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CompanyViewModel companyViewModel)
        {
            try
            {
                companyViewModel.Building = companyService.GetBuilding();
                if (companyViewModel.Photo != null)
                {
                    string uniqueFileName = photoService.UploadPhoto(companyViewModel.Photo);

                   companyViewModel.CompanyPhoto = "/photos/" + uniqueFileName;
                }
                var results = mapper.Map<Company>(companyViewModel);

                companyService.Add(results);
                return RedirectToAction(nameof(Index));

                //Hoe kan ik de foto van IFormFile omzetten in de BLL laag en dat zo meegeven


            }
            catch
            {
                return View();
            }
        }

        // GET: CompaniesController/Edit/5
        public ActionResult Edit(int id)
        {
            CompanyViewModel companyView = mapper.Map<CompanyViewModel>(companyService.Get(id).Result);
            return View(companyView);
        }

        // POST: CompaniesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompanyViewModel companyView)
        {
          
            try
            {
                var result = mapper.Map<Company>(companyView);
                companyService.Update(result);
                
                // Omvormen van View Model naar Company 

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompaniesController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            CompanyViewModel companyView = mapper.Map<CompanyViewModel>(companyService.Get((int)id).Result);
            return View(companyView);
        }

        // POST: CompaniesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                companyService.Delete((int)id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(string searchTerm)
        {
            //Simpel op zoeken van de companies bij naam 
            // Terug geven van naam via Order by ascending or descending
            if (string.IsNullOrEmpty(searchTerm))
            {
                return RedirectToAction(nameof(NotFound));
            }
            var results = companyService.SearchByName(searchTerm);
            IEnumerable<CompanyViewModel> companyViews = mapper.Map<IEnumerable<CompanyViewModel>>(results);
            //Toevoegen met een model 
            return RedirectToAction(nameof(Index), new { companyViews = companyViews});
        }

        
    }
}
