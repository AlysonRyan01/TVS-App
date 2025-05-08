using TVS_App.Application.Exceptions;
using TVS_App.Domain.Enums;

namespace TVS_App.Application.Commands.ServiceOrderCommands;

public class UpdateServiceOrderCommand : ICommand
{
    public long CustomerId { get; set; }
    public long ServiceOrderId { get; set; }

    public string ProductModel { get; set; } = string.Empty;
    public string ProductSerialNumber { get; set; } = string.Empty;
    public string ProductDefect { get; set; } = string.Empty;
    public string Accessories { get; set; } = string.Empty;
    public EProduct ProductType { get; set; }

    public EEnterprise Enterprise { get; set; } = EEnterprise.Particular;

    public void Validate()
    {
        if (ServiceOrderId == 0)
            throw new CommandException<UpdateServiceOrderCommand>("O ServiceOrderId do UpdateServiceOrderCommand não pode ser 0");

        if (CustomerId == 0)
            throw new CommandException<UpdateServiceOrderCommand>("O CustomerId do UpdateServiceOrderCommand não pode ser 0");

        if (string.IsNullOrEmpty(ProductModel))
            throw new CommandException<UpdateServiceOrderCommand>("O ProductModel do UpdateServiceOrderCommand não pode estar vazio");

        if (string.IsNullOrEmpty(ProductSerialNumber))
            throw new CommandException<UpdateServiceOrderCommand>("O ProductSerialNumber do UpdateServiceOrderCommand não pode estar vazio");

        if (!Enum.IsDefined(typeof(EProduct), ProductType))
            throw new CommandException<UpdateServiceOrderCommand>("O tipo de produto no UpdateServiceOrderCommand é inválido.");
            
        if (!Enum.IsDefined(typeof(EEnterprise), Enterprise))
            throw new CommandException<UpdateServiceOrderCommand>("A empresa informada no UpdateServiceOrderCommand é inválida.");

    }
}