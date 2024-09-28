using Microsoft.EntityFrameworkCore;
using PatientScreeningSystem.Models;

namespace PatientScreeningSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Patient> Patients { get; set; }


    }
}
