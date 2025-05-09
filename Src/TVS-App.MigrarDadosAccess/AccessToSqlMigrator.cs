using System.Data.OleDb;
using TVS_App.Domain.Entities;
using TVS_App.Domain.Enums;
using TVS_App.Domain.ValueObjects.Customer;
using TVS_App.Domain.ValueObjects.Product;
using TVS_App.Infrastructure.Data;

namespace TVS_App.MigrarDadosAccess;

public class AccessToSqlMigrator
{
    private readonly string _accessConnStr;
    private readonly ApplicationDataContext _sqlDbContext;

    public AccessToSqlMigrator(string accessConnStr, ApplicationDataContext sqlDbContext)
    {
        _accessConnStr = accessConnStr;
        _sqlDbContext = sqlDbContext;
    }

    public async Task MigrarClientesAsync()
    {
        var customers = new List<Customer>();

        using var connection = new OleDbConnection(_accessConnStr);
        await connection.OpenAsync();

        const string query = @"SELECT 
                            cli_codigo as Code,
                            cli_nome as Name,
                            cli_endereco as Street,
                            cli_bairro as Neighborhood,
                            cli_cidade as City,
                            cli_numero as Number,
                            cli_cep as ZipCode,
                            cli_uf as State,
                            cli_telefone as Phone,
                            cli_celular as CellPhone,
                            cli_email as Email
                        FROM cliente";

        using var command = new OleDbCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            try
            {
                var name = new Name(reader["Name"] as string ?? "Não tem");
                var address = new Address(
                    reader["Street"] as string,
                    reader["Neighborhood"] as string,
                    reader["City"] as string,
                    reader["Number"] as string,
                    reader["ZipCode"] as string,
                    reader["State"] as string);

                var phone = new Phone(reader["CellPhone"] as string ?? "Não tem");
                var phone2 = new Phone(reader["Phone"] as string ?? "Não tem");;


                var emailStr = reader["Email"] as string ?? "Não tem";
                var email = new Email(emailStr);

                var customer = new Customer(name, address, phone, phone2, email)
                {
                    Id = reader["Code"] != DBNull.Value ? Convert.ToInt32(reader["Code"]) : 0
                };

                customers.Add(customer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao migrar cliente: {ex.Message}");
            }
        }

        await _sqlDbContext.Customers.AddRangeAsync(customers);
        await _sqlDbContext.SaveChangesAsync();
        Console.WriteLine("Migracao feita com sucesso!");
    }

    public async Task MigrarOrdensDeServicoAsync()
    {
        var serviceOrders = new List<ServiceOrder>();

        using var connection = new OleDbConnection(_accessConnStr);
        await connection.OpenAsync();

        const string query = @"SELECT 
            os.os_codigo AS Id,
            os.os_situacao AS eServiceOrderStatus,
            os.emp_codigo AS eEnterprise,
            os.pro_codigo AS productType,
            os.os_marca AS productBrand,
            os.os_modelo AS productModel,
            os.os_ns AS productSerialNumber,
            os.os_defeito AS productDefect,
            os.os_solucao AS solution,
            os.os_valor AS amount,
            os.os_data_entrada AS entryDate,
            os.os_data_vistoria AS inspectionDate,
            os.os_data_concerto AS repairDate,
            os.os_data_entrega AS deliveryDate,
            os.cli_codigo AS customerId,
            os.os_concerto AS eRepair,
            os.os_semconserto AS eUnrepaired,
            os.os_valpeca AS partCost,
            os.os_valmo AS laborCost
            FROM os ORDER BY os.os_codigo DESC";

        using var command = new OleDbCommand(query, connection);
        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            try
            {
                var customerId = Convert.ToInt64(reader["customerId"]);
                var productType = (EProduct)Convert.ToInt32(reader["productType"]);
                var enterprise = (EEnterprise)Convert.ToInt32(reader["eEnterprise"]);

                var model = reader["productModel"] as string ?? "Não tem";
                var serial = reader["productSerialNumber"] as string ?? "Não tem";
                var defect = reader["productDefect"] as string ?? "Não tem";
                var brand = reader["productBrand"] as string ?? "Não tem";
                var accessories = "Não tem";

                var product = new Product(new Brand(brand), new Model(model), new SerialNumber(serial), new Defect(defect), accessories, productType);

                var serviceOrder = new ServiceOrder(enterprise, customerId, product);

                if (reader["solution"] != DBNull.Value)
                {
                    var solution = reader["solution"] as string ?? "Não tem";
                    var partCost = reader["partCost"] != DBNull.Value ? Convert.ToDecimal(reader["partCost"]) : 0;
                    var laborCost = reader["laborCost"] != DBNull.Value ? Convert.ToDecimal(reader["laborCost"]) : 0;

                    var repaired = Convert.ToBoolean(reader["eRepair"]);
                    var unrepaired = Convert.ToBoolean(reader["eUnrepaired"]);
                    var repairResult = repaired
                        ? ERepairResult.Repair
                        : unrepaired ? ERepairResult.Unrepaired : ERepairResult.Unrepaired;

                    serviceOrder.AddEstimate(solution, partCost, laborCost, repairResult);
                }

                var status = (EServiceOrderStatus)Convert.ToInt32(reader["eServiceOrderStatus"]);
                var repairDate = reader["repairDate"] != DBNull.Value ? Convert.ToDateTime(reader["repairDate"]) : (DateTime?)null;
                var deliveryDate = reader["deliveryDate"] != DBNull.Value ? Convert.ToDateTime(reader["deliveryDate"]) : (DateTime?)null;

                if (status == EServiceOrderStatus.Repaired || repairDate.HasValue)
                {
                    serviceOrder.ApproveEstimate();
                    serviceOrder.ExecuteRepair();
                }

                if (status == EServiceOrderStatus.Delivered || deliveryDate.HasValue)
                {
                    serviceOrder.AddDelivery();
                }

                serviceOrders.Add(serviceOrder);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao migrar OS: {ex.Message}");
            }
        }

        await _sqlDbContext.ServiceOrders.AddRangeAsync(serviceOrders);
        await _sqlDbContext.SaveChangesAsync();

        Console.WriteLine("Migração de ordens de serviço concluída!");
    }
}