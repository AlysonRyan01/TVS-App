using TVS_App.Application.Exceptions;
using TVS_App.Domain.Enums;

namespace TVS_App.Application.Commands.ServiceOrderCommands;

public class CreateServiceOrderCommand : ICommand
{
    public long CustomerId { get; set; }

    public string ProductBrand { get; set; } = string.Empty;
    public string ProductModel { get; set; } = string.Empty;
    public string ProductSerialNumber { get; set; } = string.Empty;
    public string ProductDefect { get; set; } = string.Empty;
    public string Accessories { get; set; } = string.Empty;
    public EProduct ProductType { get; set; } = EProduct.Tv;

    public EEnterprise Enterprise { get; set; } = EEnterprise.Particular;

    public void Validate()
    {
        if (CustomerId == 0)
            throw new CommandException<CreateServiceOrderCommand>("O CustomerId do CreateServiceOrderCommand não pode ser 0");

        if (string.IsNullOrEmpty(ProductModel))
            throw new CommandException<CreateServiceOrderCommand>("O ProductModel do CreateServiceOrderCommand não pode estar vazio");

        if (string.IsNullOrEmpty(ProductSerialNumber))
            throw new CommandException<CreateServiceOrderCommand>("O ProductSerialNumber do CreateServiceOrderCommand não pode estar vazio");

        if (!Enum.IsDefined(typeof(EProduct), ProductType))
            throw new CommandException<CreateServiceOrderCommand>("O ProductType do CreateServiceOrderCommand não tem um valor válido");

        if (!Enum.IsDefined(typeof(EEnterprise), Enterprise))
            throw new CommandException<CreateServiceOrderCommand>("O Enterprise do CreateServiceOrderCommand não tem um valor válido");

    }

    public void Normalize()
    {
        ProductBrand = ProductBrand.Trim().ToUpper();
        ProductModel = ProductModel.Trim().ToUpper();
        ProductSerialNumber = ProductSerialNumber.Trim().ToUpper();
        ProductDefect = ProductDefect.Trim().ToUpper();
        Accessories = Accessories.Trim().ToUpper();
    }
}