using evanairlines.Interfaces;
using evanairlines.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace evanairlines.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly Db context;
        public CheckoutController(Db _context)
        {
            context = _context;
        }

        public IActionResult Index(string user)
        {
            var cart = context.Checkout
                .Where(entry => entry.username == user)
                .ToList();
            return View(cart);
        }

        public string generateConfirmationCode()
        {
            string confirmation = "";
            for (int i = 0; i < 5; i++)
            {
                Random random = new Random();
                char c = (char)random.Next('A', 'Z' + 1);
                confirmation += c;
            }
            return confirmation;
        }

        [HttpPost]
        public IActionResult ThankYou(string user, string firstName, string lastName)
        {

            string confirmationCode = generateConfirmationCode();
            var entriesToRemove = context.Checkout.Where(e => e.username == user).ToList();
            if (firstName == null || lastName == null || entriesToRemove.Count == 0)
            {
                throw new ApplicationException("XDDCC");
            }
            foreach (var entry in entriesToRemove)
            {
                ConfirmationModel confirmation = new ConfirmationModel
                {
                    confirmationCode = confirmationCode,
                    firstName = firstName,
                    lastName = lastName,
                    route = entry.departure + " - " + entry.arrival,
                    aircraft = entry.aircraft,
                    number = entry.number
                };
                context.Confirmation.Add(confirmation);
            }

            context.Checkout.RemoveRange(entriesToRemove);
            context.SaveChanges();

            // Now, retrieve confirmations for the specific confirmationCode
            var confirmations = context.Confirmation
                .Where(entry => entry.confirmationCode == confirmationCode)
                .ToList();

            return View(confirmations);
        }
    }
}
