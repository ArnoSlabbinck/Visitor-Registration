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
        public async Task<IActionResult> Index(int? id)
        {
            IEnumerable<EmployeeViewModel> employeeViews;
            if (id == null)
            {
                return NotFound();

            }
            TempData["Id"] = (int)id;

            var results = await Task.FromResult(employeeService.GetEmployeesFromCompany((int)id));

            employeeViews = mapper.Map<IEnumerable<EmployeeViewModel>>(results);

            return View(employeeViews);
        }

        // GET: EmployeesController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var employee = await employeeService.Get((int)id);
            var employeeView = mapper.Map<EmployeeViewModel>(employee);
            return View(employeeView);
        }

        // GET: EmployeesController/Create
        public ActionResult Create(int id)
        {
        
            EmployeeViewModel employeeViewModel = new EmployeeViewModel();
            // Dan omzetten via automapper en in database steken

           
            
            
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

            
            Errors = await Task.FromResult(await employeeService.Add(employee));


            return RedirectToAction(nameof(Index), new {id = id});
            

        }

        // GET: EmployeesController/Edit/5
        public IActionResult Edit()
        {

            return View();
        }

        // POST: EmployeesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EmployeeViewModel employeeView)
        {
            try
            {
                var employee = mapper.Map<Employee>(employeeView);

                employeeService.Update(employee); 

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeesController/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var employee = mapper.Map<EmployeeViewModel>(employeeService.Get((int)id).Result);
            return View(employee);
        }

        // POST: EmployeesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, IFormCollection collection)
        {
            try
            {
                if(id == null)
                {
                    return NotFound();
                }

                bool deletedEmployee  = await employeeService.Delete((int)id);

 
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
