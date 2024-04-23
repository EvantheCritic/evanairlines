using evanairlines.Models;
using Microsoft.AspNetCore.Mvc;

namespace evanairlines.Controllers
{
    public class AdminController : Controller
    {
        private readonly Db context;

        public AdminController(Db _context) 
        {
            context = _context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var employees = context.Employees.ToList();
            return View(employees);
        }

        public IActionResult AdminProperties(int id)
        {
            EmployeeModel employee = context.Employees.FirstOrDefault(e => e.id == id);
            return View(employee);
        }

        [HttpGet]
        public IActionResult AddEntry()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitEntry(LogbookModel _entry)
        {
            LogbookModel entry = new LogbookModel
            {
                user_booked = _entry.user_booked,
                pilot = _entry.pilot,
                copilot = _entry.copilot,
                engineer = _entry.engineer,
                fl_attendant = _entry.fl_attendant,
                departure = _entry.departure,
                arrival = _entry.arrival,
                hours = _entry.hours,
                minutes = _entry.minutes,
                date = _entry.date,
                pay = _entry.pay,
                
            };
            context.Logbook.Add(entry);
            context.SaveChanges();
            double time = entry.hours;
            double dec = entry.minutes / 60;
            time += dec;
            var Employees = context.Employees.ToList();
            foreach (var employee in Employees)
            {
                if (entry.pilot == employee.name || entry.copilot == employee.name || entry.engineer == employee.name || entry.fl_attendant == employee.name)
                {
                    employee.hours += time;
                    if (employee.job == "Captain")
                        employee.pay += entry.pay / 2;
                    else if (employee.job == "First Officer")
                        employee.pay += entry.pay / 4;
                    else
                        employee.pay += entry.pay / 8;
                }
            }
            context.SaveChanges();
            return RedirectToAction("Index", "Logbook");
        }

        [HttpGet]
        public IActionResult Addhours(int id)
        {
            EmployeeModel employee = context.Employees.FirstOrDefault(e => e.id == id);
            return View(employee);
        }

        [HttpPost]
        public IActionResult Addhours(int id, double inputPay, double inputHours)
        {
            var employee = context.Employees.FirstOrDefault(e => e.id == id);
            if (employee != null)
            {
                employee.hours += inputHours;
                employee.pay += inputPay;
                context.SaveChanges();
                return RedirectToAction("Detail", "Employees", new { id = employee.id });
            }
            else
            {
                return NotFound();
            }
        }
    }
}
