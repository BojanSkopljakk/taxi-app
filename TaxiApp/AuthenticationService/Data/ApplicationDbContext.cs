using Microsoft.EntityFrameworkCore;
using AuthenticationService.Models;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AuthenticationService.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {
                
        }
        public DbSet<DriverApplication> DriverApplications { get; set; }

    }

    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args) { 

            var optionsbuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsbuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Users2DB;Trusted_Connection=True;TrustServerCertificate=True");

            return new ApplicationDbContext(optionsbuilder.Options);
        }
    }
}
