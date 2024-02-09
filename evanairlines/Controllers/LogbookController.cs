using evanairlines.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace evanairlines.Controllers
{
    public class LogbookController : Controller
    {
        private readonly Db context;
        public LogbookController(Db _context)
        {
            context = _context;
        }

        public IActionResult Index()
        {
            var flights = context.Logbook.ToList();
            return View(flights);
        }

        public IActionResult Flown(string name)
        {
            var filteredFlights = context.Logbook.Where(flight => flight.pilot == name || flight.copilot == name || flight.engineer == name || flight.fl_attendant == name).ToList();                
            return View(filteredFlights); // Pass the filtered flights to the view
        }
    }
}
