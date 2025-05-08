using TVS_App.Application.Commands.ServiceOrderCommands;
using TVS_App.Application.DTOs;
using TVS_App.Application.Repositories;
using TVS_App.Domain.Entities;
using TVS_App.Domain.ValueObjects.Product;

namespace TVS_App.Application.Handlers;

public class ServiceOrderHandler
{
    private readonly IServiceOrderRepository _serviceOrderRepository;

    public ServiceOrderHandler(IServiceOrderRepository serviceOrderRepository)
    {
        _serviceOrderRepository = serviceOrderRepository;
    }

    public async Task<BaseResponse<ServiceOrder?>> CreateServiceOrderAsync(CreateServiceOrderCommand command)
    {
        try
        {
            var model = new Model(command.ProductModel);
            var serialNumber = new SerialNumber(command.ProductSerialNumber);
            var defect = new Defect(command.ProductDefect);
            var accessories = command.Accessories;
            var type = command.ProductType;

            var product = new Product(model, serialNumber, defect, accessories, type);

            var ServiceOrder = new ServiceOrder(command.Enterprise, command.CustomerId, product);

            return await _serviceOrderRepository.CreateAsync(ServiceOrder);
        }
        catch (Exception ex)
        {
            return new BaseResponse<ServiceOrder?>(null, 500, $"Ocorreu um erro ao criar a ordem de serviço: {ex.Message}");
        }
    }
}