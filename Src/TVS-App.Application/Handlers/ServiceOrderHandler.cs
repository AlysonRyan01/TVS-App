using TVS_App.Application.Commands.ServiceOrderCommands;
using TVS_App.Application.DTOs;
using TVS_App.Application.Exceptions;
using TVS_App.Application.Repositories;
using TVS_App.Domain.Entities;
using TVS_App.Domain.ValueObjects.Product;

namespace TVS_App.Application.Handlers;

public class ServiceOrderHandler
{
    private readonly IServiceOrderRepository _serviceOrderRepository;
    private readonly ICustomerRepository _customerRepository;

    public ServiceOrderHandler(IServiceOrderRepository serviceOrderRepository, ICustomerRepository customerRepository)
    {
        _serviceOrderRepository = serviceOrderRepository;
        _customerRepository = customerRepository;
    }

    public async Task<BaseResponse<ServiceOrder?>> CreateServiceOrderAsync(CreateServiceOrderCommand command)
    {
        try
        {
            command.Validate();

            var model = new Model(command.ProductModel);
            var serialNumber = new SerialNumber(command.ProductSerialNumber);
            var defect = new Defect(command.ProductDefect);
            var accessories = command.Accessories;
            var type = command.ProductType;

            var product = new Product(model, serialNumber, defect, accessories, type);

            var ServiceOrder = new ServiceOrder(command.Enterprise, command.CustomerId, product);

            return await _serviceOrderRepository.CreateAsync(ServiceOrder);
        }
        catch (CommandException<CreateServiceOrderCommand> ex)
        {
            return new BaseResponse<ServiceOrder?>(null, 400, $"Erro de validação: {ex.Message}");
        }
        catch (Exception ex)
        {
            return new BaseResponse<ServiceOrder?>(null, 500, $"Ocorreu um erro ao criar a ordem de serviço: {ex.Message}");
        }
    }

    public async Task<BaseResponse<ServiceOrder?>> UpdateServiceOrderAsync(UpdateServiceOrderCommand command)
    {
        try
        {
            command.Validate();

            var existingCustomer = await _customerRepository.GetByIdAsync(command.CustomerId);
            if (existingCustomer == null || existingCustomer.Data == null)
                return new BaseResponse<ServiceOrder?>(null, 404, $"O cliente com id: {command.CustomerId} não existe");

            var customer = existingCustomer.Data;

            var existingServiceOrder = await _serviceOrderRepository.GetById(command.ServiceOrderId);
            if (existingServiceOrder == null || existingServiceOrder.Data == null)
                return new BaseResponse<ServiceOrder?>(null, 404, $"A ordem de serviço com id: {command.ServiceOrderId} não existe");

            var serviceOrder = existingServiceOrder.Data;

            serviceOrder.UpdateServiceOrder(customer,
                command.ProductModel,
                command.ProductSerialNumber,
                command.ProductDefect,
                command.Accessories,
                command.ProductType,
                command.Enterprise);

            return await _serviceOrderRepository.UpdateAsync(serviceOrder);
        }
        catch (CommandException<UpdateServiceOrderCommand> ex)
        {
            return new BaseResponse<ServiceOrder?>(null, 400, $"Erro de validação: {ex.Message}");
        }
        catch (Exception ex)
        {
            return new BaseResponse<ServiceOrder?>(null, 500, $"Ocorreu um erro ao atualizar a ordem de serviço: {ex.Message}");
        }
    }
}