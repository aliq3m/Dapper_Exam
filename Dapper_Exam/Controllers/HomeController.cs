using Dapper_Exam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper_Exam.Repositories;

namespace Dapper_Exam.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IBonusRepository _bonusRepository;

        public HomeController(ILogger<HomeController> logger, IBonusRepository bonusRepository)
        {
            _logger = logger;
            _bonusRepository = bonusRepository;
        }

       

        public IActionResult Index()
        {
            return View(_bonusRepository.GetAllCompanyWithEmployees());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult AddRecords()
        {
            Company company = new Company()
            {
                CompanyName = "Test " + Guid.NewGuid().ToString(),
                Address = "test Address",
                City = "Test City",
                PostalCode = "Test Postal Code",
                State = "Test State",
                Employees = new List<Employee>()
            };

            company.Employees.Add(new Employee()
            {
                Name = "Test Name " + Guid.NewGuid().ToString(),
                Email = "Test Email",
                Phone = "Test phone",
                Title = "Test Manager"
            });

            company.Employees.Add(new Employee()
            {
                Name = "Test Name " + Guid.NewGuid().ToString(),
                Email = "Test Email 2",
                Phone = "Test phone 2",
                Title = "Test Manager 2"
            });

            _bonusRepository.AddTestRecordsWithTransAction(company);

            return RedirectToAction("Index");
        }

        public IActionResult DeleteRecords(int id)
        {
            var company = _bonusRepository.GetCompanyWithEmployee(id);
            _bonusRepository.DeleteCompany(company);
            return RedirectToAction("Index");
        }
    }
}
