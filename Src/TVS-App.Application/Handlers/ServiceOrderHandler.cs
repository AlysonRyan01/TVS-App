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

            var productBrand = new Brand(command.ProductBrand);
            var model = new Model(command.ProductModel);
            var serialNumber = new SerialNumber(command.ProductSerialNumber);
            var defect = new Defect(command.ProductDefect);
            var accessories = command.Accessories;
            var type = command.ProductType;

            var product = new Product(productBrand, model, serialNumber, defect, accessories, type);

            var serviceOrder = new ServiceOrder(command.Enterprise, command.CustomerId, product);

            return await _serviceOrderRepository.CreateAsync(serviceOrder);
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
                command.ProductBrand,
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

    public async Task<BaseResponse<ServiceOrder?>> GetServiceOrderById(GetServiceOrderByIdCommand command)
    {
        try
        {
            command.Validate();

            return await _serviceOrderRepository.GetById(command.Id);
        }
        catch (CommandException<GetServiceOrderByIdCommand> ex)
        {
            return new BaseResponse<ServiceOrder?>(null, 400, $"Erro de validação: {ex.Message}");
        }
        catch (Exception ex)
        {
            return new BaseResponse<ServiceOrder?>(null, 500, $"Ocorreu um erro desconhecido ao buscar a ordem de servico: {ex.Message}");
        }
    }

    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetAllServiceOrdersAsync(int pageNumber, int pageSize)
    {
        try
        {
            return await _serviceOrderRepository.GetAllAsync(pageNumber, pageSize);
        }
        catch (Exception ex)
        {

            return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, $"Ocorreu um erro desconhecido ao buscar todas as ordens de servico: {ex.Message}");
        }
    }

    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetPendingEstimatesAsync(int pageNumber, int pageSize)
    {
        try
        {
            return await _serviceOrderRepository.GetPendingEstimatesAsync(pageNumber, pageSize);
        }
        catch (Exception ex)
        {

            return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, $"Ocorreu um erro desconhecido ao buscar as ordens de servico pendentes de orçamento: {ex.Message}");
        }
    }

    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetWaitingResponseAsync(int pageNumber, int pageSize)
    {
        try
        {
            return await _serviceOrderRepository.GetWaitingResponseAsync(pageNumber, pageSize);
        }
        catch (Exception ex)
        {

            return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, $"Ocorreu um erro desconhecido ao buscar as ordens de servico que estão aguardando resposta: {ex.Message}");
        }
    }

    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetWaitingPartsAsync(int pageNumber, int pageSize)
    {
        try
        {
            return await _serviceOrderRepository.GetWaitingPartsAsync(pageNumber, pageSize);
        }
        catch (Exception ex)
        {

            return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, $"Ocorreu um erro desconhecido ao buscar as ordens de servico que estão aguardando peça: {ex.Message}");
        }
    }

    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetWaitingPickupAsync(int pageNumber, int pageSize)
    {
        try
        {
            return await _serviceOrderRepository.GetWaitingPickupAsync(pageNumber, pageSize);
        }
        catch (Exception ex)
        {

            return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, $"Ocorreu um erro desconhecido ao buscar as ordens de servico que estão aguardando coleta: {ex.Message}");
        }
    }

    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetDeliveredAsync(int pageNumber, int pageSize)
    {
        try
        {
            return await _serviceOrderRepository.GetDeliveredAsync(pageNumber, pageSize);
        }
        catch (Exception ex)
        {

            return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, $"Ocorreu um erro desconhecido ao buscar as ordens de servico que estão entregues: {ex.Message}");
        }
    }

    public async Task<BaseResponse<ServiceOrder?>> AddServiceOrderEstimate(AddServiceOrderEstimateCommand command)
    {
        try
        {
            command.Validate();

            var result = await _serviceOrderRepository.GetById(command.ServiceOrderId);
            if (result == null || result.Data == null)
                return new BaseResponse<ServiceOrder?>(null, 404, "Essa ordem de serviço não existe");

            var serviceOrder = result.Data;

            serviceOrder.AddEstimate(command.Solution, command.PartCost, command.LaborCost, command.RepairResult);

            await _serviceOrderRepository.UpdateAsync(serviceOrder);

            return new BaseResponse<ServiceOrder?>(serviceOrder, 200, "Orçamento adicionado com sucesso!");
        }
        catch (CommandException<AddServiceOrderEstimateCommand> ex)
        {
            return new BaseResponse<ServiceOrder?>(null, 400, $"Erro de validação: {ex.Message}");
        }
        catch (Exception ex)
        {
            return new BaseResponse<ServiceOrder?>(null, 500, $"Ocorreu um erro desconhecido ao adicionar o orçamento na ordem de serviço: {ex.Message}");
        }
    }

    public async Task<BaseResponse<ServiceOrder?>> AddServiceOrderApproveEstimate(GetServiceOrderByIdCommand command)
    {
        try
        {
            command.Validate();

            var result = await _serviceOrderRepository.GetById(command.Id);
            if (result == null || result.Data == null)
                return new BaseResponse<ServiceOrder?>(null, 404, "Essa ordem de serviço não existe");

            var serviceOrder = result.Data;

            serviceOrder.ApproveEstimate();

            await _serviceOrderRepository.UpdateAsync(serviceOrder);

            return new BaseResponse<ServiceOrder?>(serviceOrder, 200, "Orçamento aprovado com sucesso!");
        }
        catch (CommandException<GetServiceOrderByIdCommand> ex)
        {
            return new BaseResponse<ServiceOrder?>(null, 400, $"Erro de validação: {ex.Message}");
        }
        catch (Exception ex)
        {
            return new BaseResponse<ServiceOrder?>(null, 500, $"Ocorreu um erro desconhecido ao aprovar o orçamento na ordem de serviço: {ex.Message}");
        }
    }

    public async Task<BaseResponse<ServiceOrder?>> AddServiceOrderRejectEstimate(GetServiceOrderByIdCommand command)
    {
        try
        {
            command.Validate();

            var result = await _serviceOrderRepository.GetById(command.Id);
            if (result == null || result.Data == null)
                return new BaseResponse<ServiceOrder?>(null, 404, "Essa ordem de serviço não existe");

            var serviceOrder = result.Data;

            serviceOrder.RejectEstimate();

            await _serviceOrderRepository.UpdateAsync(serviceOrder);

            return new BaseResponse<ServiceOrder?>(serviceOrder, 200, "Orçamento reprovado com sucesso!");
        }
        catch (CommandException<GetServiceOrderByIdCommand> ex)
        {
            return new BaseResponse<ServiceOrder?>(null, 400, $"Erro de validação: {ex.Message}");
        }
        catch (Exception ex)
        {
            return new BaseResponse<ServiceOrder?>(null, 500, $"Ocorreu um erro desconhecido ao reprovar o orçamento na ordem de serviço: {ex.Message}");
        }
    }

    public async Task<BaseResponse<ServiceOrder?>> AddServiceOrderRepair(GetServiceOrderByIdCommand command)
    {
        try
        {
            command.Validate();

            var result = await _serviceOrderRepository.GetById(command.Id);
            if (result == null || result.Data == null)
                return new BaseResponse<ServiceOrder?>(null, 404, "Essa ordem de serviço não existe");

            var serviceOrder = result.Data;

            serviceOrder.ExecuteRepair();

            await _serviceOrderRepository.UpdateAsync(serviceOrder);

            return new BaseResponse<ServiceOrder?>(serviceOrder, 200, "Ordem de serviço marcada como consetada com sucesso!");
        }
        catch (CommandException<GetServiceOrderByIdCommand> ex)
        {
            return new BaseResponse<ServiceOrder?>(null, 400, $"Erro de validação: {ex.Message}");
        }
        catch (Exception ex)
        {
            return new BaseResponse<ServiceOrder?>(null, 500, $"Ocorreu um erro desconhecido ao marcar o conserto na ordem de serviço: {ex.Message}");
        }
    }

    public async Task<BaseResponse<ServiceOrder?>> AddServiceOrderDelivery(GetServiceOrderByIdCommand command)
    {
        try
        {
            command.Validate();

            var result = await _serviceOrderRepository.GetById(command.Id);
            if (result == null || result.Data == null)
                return new BaseResponse<ServiceOrder?>(null, 404, "Essa ordem de serviço não existe");

            var serviceOrder = result.Data;

            serviceOrder.AddDelivery();

            await _serviceOrderRepository.UpdateAsync(serviceOrder);

            return new BaseResponse<ServiceOrder?>(serviceOrder, 200, "Ordem de serviço marcada como entregue com sucesso!");
        }
        catch (CommandException<GetServiceOrderByIdCommand> ex)
        {
            return new BaseResponse<ServiceOrder?>(null, 400, $"Erro de validação: {ex.Message}");
        }
        catch (Exception ex)
        {
            return new BaseResponse<ServiceOrder?>(null, 500, $"Ocorreu um erro desconhecido ao marcar a entrega na ordem de serviço: {ex.Message}");
        }
    }
}