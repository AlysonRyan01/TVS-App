using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TVS_App.Infrastructure.Data;

public class ApplicationDataContextFactory : IDesignTimeDbContextFactory<ApplicationDataContext>
{
    public ApplicationDataContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDataContext>();
            optionsBuilder.UseSqlServer(
                "Server=.\\SQLEXPRESS;Database=tvs-database;Trusted_Connection=True;TrustServerCertificate=True;");

            return new ApplicationDataContext(optionsBuilder.Options);
    }
}