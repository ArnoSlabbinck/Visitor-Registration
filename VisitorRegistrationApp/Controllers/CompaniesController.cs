using AutoMapper;
using BL.Services;
using BLL.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;
using VisitorRegistrationApp.Data.Helper;
using VisitorRegistrationApp.Helper;
using VisitorRegistrationApp.Models;

namespace VisitorRegistrationApp.Controllers
{
    public class CompaniesController : Controller
    {
        //Niet naar buiten brengen van models vanuit services. View kent Models niet

        private readonly IMapper mapper;
        private readonly ICompanyService companyService;
       
        private IList<string> Errors;
        public CompaniesController(IMapper mapper,
            ICompanyService companyService
           )
        {
         
            this.mapper = mapper;
            this.companyService = companyService;
          
           
        }
        // GET: CompaniesController
        public async Task<ActionResult> Index()
        {
            var JsonCompaniesViews = (string)TempData["Companies"];
          
            if (JsonCompaniesViews != null)
            {
                var companies = JsonConvert.DeserializeObject<IEnumerable<CompanyViewModel>>(JsonCompaniesViews, new CompanyViewModelListConverter());
                return View(companies);
            }


            //Ophalen van alle companies in Building
            var results = await companyService.getAll();
            
            IEnumerable<CompanyViewModel> AllCompanies = mapper.Map<IEnumerable<CompanyViewModel>>(results); 

            return View(AllCompanies);
        }


       
   
        // GET: CompaniesController/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var Company = await companyService.Get((int)id);
            CompanyViewModel company = mapper.Map<CompanyViewModel>(Company);
            if(company.Picture !=  null)
                company.Base64Image = ImgToByteConverter.byteArrayToImage(company.Picture.ImageFile);
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
            if(FileChecker.CheckUploadedFileIsImage(companyViewModel.file))
            {
                if (ModelState.IsValid)
                {
                    companyViewModel.Picture = new Model.Image();
                    //Builder pattern maken voor aanmaken vna image
                    //Omzetten van file naar byte array
                    companyViewModel.Picture.ImageFile = ImageConverter.fileToByteArray(companyViewModel.file);
                    companyViewModel.Picture.ImageName = $"{companyViewModel.Name} picture";
                    companyViewModel.Picture.OriginalFormat = companyViewModel.file.Length.ToString();

                    // Validation on viewmocel
                    companyViewModel.Building = companyService.GetBuilding();

                    var results = mapper.Map<Company>(companyViewModel);

                    Errors = await companyService.Add(results);
                    
                    

                   
                    return RedirectToAction(nameof(Index));
                }
            

            }

            
            return View();
            
        }

        // GET: CompaniesController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            CompanyViewModel companyView = mapper.Map<CompanyViewModel>(await Task.FromResult(companyService.Get(id).Result));
            return View(companyView);
        }

        // POST: CompaniesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async  Task<ActionResult> Edit(CompanyViewModel companyView)
        {
          
            try
            {

                var company = mapper.Map<Company>(companyView);
                byte[] ImageFile = ImageConverter.fileToByteArray(companyView.file);
                Errors = await companyService.Update(company, ImageFile);

                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompaniesController/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var company = await Task.FromResult(companyService.Get((int)id).Result);
            CompanyViewModel companyView = mapper.Map<CompanyViewModel>(company); 
            return View(companyView);
        }

        // POST: CompaniesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await Task.FromResult(companyService.Delete((int)id));
               
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
            
            if (string.IsNullOrEmpty(searchTerm))
            {
                return RedirectToAction(nameof(NotFound));
            }
            var companies = companyService.SearchByName(searchTerm);
            IEnumerable<CompanyViewModel> companyViews = mapper.Map<IEnumerable<CompanyViewModel>>(companies);
            //Toevoegen met een model 
            TempData["Companies"] = JsonConvert.SerializeObject(companyViews);
            return RedirectToAction(nameof(Index));
        }



        
        
    }
}
