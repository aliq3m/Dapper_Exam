using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dapper_Exam.Models;
using Dapper_Exam.Repositories;

namespace Dapper_Exam.Controllers
{
    public class EmployeesController : Controller
    {
        private ICompanyRepository _companyRepository;
        private IEmployeeRepository _employeeRepository;
        private IBonusRepository _bonusRepository;

        public EmployeesController(ICompanyRepository companyRepository, IEmployeeRepository employeeRepository, IBonusRepository bonusRepository)
        {
            _companyRepository = companyRepository;
            _employeeRepository = employeeRepository;
            _bonusRepository = bonusRepository;
        }

       

        [BindProperty]
        public Employee Employee { get; set; }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            //Select * From Companies
            return View(_bonusRepository.GetAllEmployeeWhitCompany());
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            ViewBag.CompanyId = new SelectList(_companyRepository.GetAll(), "CompanyId", "CompanyName");
            return View();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> CreatePost()
        {
            if (ModelState.IsValid)
            {
                _employeeRepository.Add(Employee);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CompanyId = new SelectList(_companyRepository.GetAll(), "CompanyId", "CompanyName");
            return View(Employee);
        }

        // GET: Companies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.CompanyId = new SelectList(_companyRepository.GetAll(), "CompanyId", "CompanyName");
            Employee = _employeeRepository.Find(id.Value);
            if (Employee == null)
            {
                return NotFound();
            }
            return View(Employee);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if (id != Employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _employeeRepository.Update(Employee);
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.CompanyId = new SelectList(_companyRepository.GetAll(), "CompanyId", "CompanyName");
            return View(Employee);
        }


        // GET: Companies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee = _employeeRepository.Find(id.Value);
            if (Employee == null)
            {
                return NotFound();
            }

            return View(Employee);
        }





        // GET: Companies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee = _employeeRepository.Find(id.Value);
            if (Employee == null)
            {
                return NotFound();
            }

            return View(Employee);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _employeeRepository.Remove(id);
            return RedirectToAction(nameof(Index));
        }


    }
}
