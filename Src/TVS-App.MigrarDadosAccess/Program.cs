using System.Data.OleDb;
using System.Linq;
using Dapper;
using Microsoft.EntityFrameworkCore;
using TVS_App.Infrastructure;
using TVS_App.Infrastructure.Data;
using TVS_App.Infrastructure.Models;
using TVS_App.MigrarDadosAccess;

public class Program
{
    public static async Task Main(string[] args)
    {
        string accessConnStr = @"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=C:\sisos\os.mdb;";

        var options = new DbContextOptionsBuilder<ApplicationDataContext>()
            .UseSqlServer("SuaConnectionStringSQL")
            .Options;

        var dbContext = new ApplicationDataContext(options);

        var migrador = new AccessToSqlMigrator(accessConnStr, dbContext);
        await migrador.MigrarClientesAsync();

        Console.WriteLine("Migração concluída com sucesso!");
    }
}