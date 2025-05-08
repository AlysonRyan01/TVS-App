using TVS_App.Domain.Enums;

namespace TVS_App.Application.Commands.ServiceOrderCommands;

public class CreateServiceOrderCommand
{
    public long CustomerId { get; set; }

    public long ProductId { get; set; }
    public string ProductModel { get; set; } = string.Empty;
    public string ProductSerialNumber { get; set; } = string.Empty;
    public string ProductDefect { get; set; } = string.Empty;
    public string Accessories { get; set; } = string.Empty;
    public EProduct ProductType { get; set; }

    public EEnterprise Enterprise { get; set; }
}