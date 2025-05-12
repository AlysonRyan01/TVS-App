using Microsoft.EntityFrameworkCore;
using TVS_App.Infrastructure.Data;
using TVS_App.MigrarDadosAccess;

public class Program
{
    public static async Task Main(string[] args)
    {
        string accessConnStr = @"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=C:\sisos\os.mdb;";

        var options = new DbContextOptionsBuilder<ApplicationDataContext>()
            .UseSqlServer("Server=localhost,1433;Database=tvs;User ID=sa;Password=1q2w3e4r@#$;Trusted_Connection=False; TrustServerCertificate=True;")
            .Options;

        var dbContext = new ApplicationDataContext(options);

        var migrador = new AccessToSqlMigrator(accessConnStr, dbContext);
        await migrador.MigrarClientesAsync();
        await migrador.MigrarOrdensDeServicoAsync();

        Console.WriteLine("Migração concluída com sucesso!");
    }
}