using AutoMapper;
using BL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;
using VisitorRegistrationApp.Data.Repository;
using VisitorRegistrationApp.Models;

namespace VisitorRegistrationApp.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly IMapper mapper;
        private readonly CompanyRepository companyRepository;
        private readonly ICompanyService companyService;
        public CompaniesController(IMapper mapper, CompanyRepository companyRepository, 
            ICompanyService companyService)
        {
            this.companyRepository = companyRepository;
            this.mapper = mapper;
            this.companyService = companyService;
        }
        // GET: CompaniesController
        public ActionResult Index()
        {
            //Ophalen van alle companies in Building
            var results =  companyRepository.GetAll().Result;
            
            List<CompanyViewModel> AllCompanies = mapper.Map<List<CompanyViewModel>>(results); 

            return View(AllCompanies);
        }

        // GET: CompaniesController/Details/5
        public ActionResult Details(int id)
        {
            var Company = companyRepository.Get(id);
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
                var results = mapper.Map<Company>(companyViewModel);
                await companyRepository.Add(results);
                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompaniesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CompaniesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompaniesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CompaniesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
