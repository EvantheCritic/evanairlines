using Microsoft.AspNetCore.Mvc;
using evanairlines.Models;
using System;

namespace evanairlines.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly Db context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly ILogger<EmployeesController> logger;

        public EmployeesController(Db _context, IWebHostEnvironment _webHostEnvironment, ILogger<EmployeesController> _logger)
        {
            context = _context;
            webHostEnvironment = _webHostEnvironment;
            logger = _logger;
        }

        public IActionResult Index()
        {
            var employees = context.Employees.ToList();
            return View(employees);
        }

        public IActionResult Detail(int id)
        {
            EmployeeModel employee = context.Employees.FirstOrDefault(e => e.id == id);
            return View(employee);
        }
    }
}
