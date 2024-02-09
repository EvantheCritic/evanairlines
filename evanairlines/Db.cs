using Microsoft.EntityFrameworkCore;
using evanairlines.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace evanairlines
{
    public class Db : IdentityDbContext<UserModel>
    {
        public Db(DbContextOptions<Db> options) : base(options) { }
        public DbSet<EmployeeModel> Employees { get; set; }
        public DbSet<LogbookModel> Logbook { get; set; }
        public DbSet<FlightModel> Flights { get; set; }
        public DbSet<CheckoutModel> Checkout { get; set; }
        public DbSet<ConfirmationModel> Confirmation { get; set; }
    }
}
