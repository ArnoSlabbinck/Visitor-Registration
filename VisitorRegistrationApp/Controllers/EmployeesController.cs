using AutoMapper;
using BL.Services;
using BLL.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitorRegistrationApp.Data.Entities;
using VisitorRegistrationApp.Data.Helper;
using VisitorRegistrationApp.Data.Repository;
using VisitorRegistrationApp.Models;

namespace VisitorRegistrationApp.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IMapper mapper;
        private readonly IEmployeeRespository employeeRepository;
        private readonly ICompanyService companyService;
        private readonly IEmployeeService employeeService;
        private  IList<string> Errors;


        public EmployeesController(IMapper mapper, 
            IEmployeeRespository employeeRepository,
            ICompanyService companyService, 
            IEmployeeService employeeService
          
           )
        {
            this.employeeRepository = employeeRepository;
            this.mapper = mapper;
            this.companyService = companyService;
            this.employeeService = employeeService;
           
    
        }

        // GET: EmployeesController
        public async Task<IActionResult> Index(int? id) // CompanyId
        {
            IEnumerable<EmployeeViewModel> employeeViews;
            if (id == null)
                return NotFound();

            
            TempData["Id"] = (int)id;

            var results = await employeeService.GetEmployeesFromCompany((int)id);
            if (results.Count() == 0)
                return View();

            ViewBag.Index = (int)id;

            employeeViews = mapper.Map<IEnumerable<EmployeeViewModel>>(results);

            return View(employeeViews);
        }

        // GET: EmployeesController/Details/5
        public async Task<IActionResult> Details(int? Id)
        {
            if(Id == null)
            {
                return NotFound();
            }
            var employee = await employeeService.GetEmployeeWithCompanyAndImage((int)Id);
            var employeeView = mapper.Map<EmployeeViewModel>(employee);
            ViewBag.Index = (int)TempData["Id"];
            if (employee.Picture != null)
                employeeView.Base64Image = ImgToByteConverter.byteArrayToImage(employee.Picture.ImageFile);
            return View(employeeView);
        }

        // GET: EmployeesController/Create
        public ActionResult Create(int id)
        {
        
            EmployeeViewModel employeeViewModel = new EmployeeViewModel();
         
            
            return View(employeeViewModel);
        }

        // POST: EmployeesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EmployeeViewModel employeeView)
        {

            var id = (int)TempData["Id"]; // In session based => Need to be empty after requesting it once


            var company = await companyService.Get((id)).ConfigureAwait(false);

            Employee employee = new Employee
            {
               
                BirthDay = employeeView.BirthDay,
                Company = company,
                Name = employeeView.Name,
                Job = employeeView.Job, 
                Salary = employeeView.Salary
            
            };

            
            Errors = await employeeService.Add(employee);


            return RedirectToAction(nameof(Index), new {id = id});
            

        }

        // GET: EmployeesController/Edit/5
        public async Task<IActionResult> Edit(int? Id )
        {
            if(Id == null)
            {
                return NotFound();
            }
            var employee = await employeeService.GetEmployeeWithCompanyAndImage((int)Id);
            var employeeView = mapper.Map<EmployeeViewModel>(employee);
            ViewBag.Index = (int)TempData["Id"]; // Veranderen naar CompanyId
            TempData["Id"] = ViewBag.Index;
            return View(employeeView);
        }

        // POST: EmployeesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeViewModel employeeView)
        {
            try
            {
                var employee = mapper.Map<Employee>(employeeView);
                byte[] ImageFile = ImageConverter.fileToByteArray(employeeView.file);

                employeeService.Update(employee, ImageFile);
                var id = (int)TempData["Id"];


                return RedirectToAction(nameof(Index), new { id = id });
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeesController/Delete/5
        public async Task<ActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return BadRequest();
            }
            var employee = await employeeService.Get((int)Id);
            var employeeView = mapper.Map<EmployeeViewModel>(employee);
            ViewBag.Index = (int)Id;
            return View(employeeView);
        }

        // POST: EmployeesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? Id, IFormCollection collection)
        {
            try
            {
                if(Id == null)
                {
                    return NotFound();
                }

                bool deletedEmployee  = await employeeService.Delete((int)Id);

 
                // Als de employee gedelete is terug opvragen van alle employees van de company
                
                return RedirectToAction(nameof(Index), new { id = (int)TempData["Id"] });
            }
            catch
            {
                return View();
            }
        }
    }
}
