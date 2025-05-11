using TVS_App.Application.Commands;
using TVS_App.Application.Commands.ServiceOrderCommands;
using TVS_App.Application.DTOs;
using TVS_App.Application.Exceptions;
using TVS_App.Application.Interfaces;
using TVS_App.Application.Repositories;
using TVS_App.Domain.Entities;
using TVS_App.Domain.ValueObjects.ServiceOrder;

namespace TVS_App.Application.Handlers;

public class ServiceOrderHandler
{
    private readonly IServiceOrderRepository _serviceOrderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IGenerateServiceOrderPdf _generateServiceOrderPdf;


    public ServiceOrderHandler(IServiceOrderRepository serviceOrderRepository, ICustomerRepository customerRepository,
    IGenerateServiceOrderPdf generateServiceOrderPdf)
    {
        _serviceOrderRepository = serviceOrderRepository;
        _customerRepository = customerRepository;
        _generateServiceOrderPdf = generateServiceOrderPdf;
    }

    public async Task<BaseResponse<byte[]>> CreateServiceOrderAndReturnPdfAsync(CreateServiceOrderCommand command)
    {
        try
        {
            command.Normalize();
            command.Validate();

            var product = new Product(command.ProductBrand, command.ProductModel,
                    command.ProductSerialNumber, command.ProductDefect, command.Accessories, command.ProductType);

            var serviceOrder = new ServiceOrder(command.Enterprise, command.CustomerId, product);

            var createServiceOrder = await _serviceOrderRepository.CreateAsync(serviceOrder);
            if (!createServiceOrder.IsSuccess)
                return new BaseResponse<byte[]>(null, 500, createServiceOrder.Message);

            var createPdf = await _generateServiceOrderPdf.GenerateCheckInDocumentAsync(serviceOrder);
            if (!createPdf.IsSuccess)
                return new BaseResponse<byte[]>(null, 500, createPdf.Message);

            return createPdf;
        }
        catch (CommandException<CreateServiceOrderCommand> ex)
        {
            return new BaseResponse<byte[]>(null, 400, $"Erro de validação: {ex.Message}");
        }
        catch (Exception ex)
        {
            return new BaseResponse<byte[]>(null, 500, $"Ocorreu um erro ao criar a ordem de serviço: {ex.Message}");
        }
    }

    public async Task<BaseResponse<ServiceOrder?>> UpdateServiceOrderAsync(UpdateServiceOrderCommand command)
    {
        try
        {
            command.Normalize();
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

    public async Task<BaseResponse<ServiceOrder?>> GetServiceOrderForCustomer(GetServiceOrdersForCustomerCommand command)
    {
        try
        {
            command.Normalize();
            command.Validate();

            var orderResult = await _serviceOrderRepository.GetById(command.ServiceOrderId);
            if (!orderResult.IsSuccess)
                return new BaseResponse<ServiceOrder?>(null, 404, "Nenhuma ordem de serviço encontrada");

            var serviceOrder = orderResult.Data;

            if (!string.Equals(serviceOrder?.SecurityCode, command.SecurityCode))
                return new BaseResponse<ServiceOrder?>(null, 404, "O código de segurança está incorreto");

            return new BaseResponse<ServiceOrder?>(serviceOrder, 200, "Ordem de serviço obtida com sucesso!");
        }
        catch (CommandException<GetServiceOrdersForCustomerCommand> ex)
        {
            return new BaseResponse<ServiceOrder?>(null, 400, $"Erro de validação: {ex.Message}");
        }
        catch (Exception ex)
        {
            return new BaseResponse<ServiceOrder?>(null, 500, $"Ocorreu um erro desconhecido ao buscar a ordem de servico: {ex.Message}");
        }
    }

    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetAllServiceOrdersAsync(PaginationCommand command)
    {
        try
        {
            command.Validate();

            return await _serviceOrderRepository.GetAllAsync(command.pageNumber, command.pageSize);
        }
        catch (Exception ex)
        {

            return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, $"Ocorreu um erro desconhecido ao buscar todas as ordens de servico: {ex.Message}");
        }
    }

    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetPendingEstimatesAsync(PaginationCommand command)
    {
        try
        {
            command.Validate();

            return await _serviceOrderRepository.GetPendingEstimatesAsync(command.pageNumber, command.pageSize);
        }
        catch (Exception ex)
        {

            return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, $"Ocorreu um erro desconhecido ao buscar as ordens de servico pendentes de orçamento: {ex.Message}");
        }
    }

    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetWaitingResponseAsync(PaginationCommand command)
    {
        try
        {
            command.Validate();

            return await _serviceOrderRepository.GetWaitingResponseAsync(command.pageNumber, command.pageSize);
        }
        catch (Exception ex)
        {

            return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, $"Ocorreu um erro desconhecido ao buscar as ordens de servico que estão aguardando resposta: {ex.Message}");
        }
    }

    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetWaitingPartsAsync(PaginationCommand command)
    {
        try
        {
            command.Validate();

            return await _serviceOrderRepository.GetWaitingPartsAsync(command.pageNumber, command.pageSize);
        }
        catch (Exception ex)
        {

            return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, $"Ocorreu um erro desconhecido ao buscar as ordens de servico que estão aguardando peça: {ex.Message}");
        }
    }

    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetWaitingPickupAsync(PaginationCommand command)
    {
        try
        {
            command.Validate();
        
            return await _serviceOrderRepository.GetWaitingPickupAsync(command.pageNumber, command.pageSize);
        }
        catch (Exception ex)
        {

            return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, $"Ocorreu um erro desconhecido ao buscar as ordens de servico que estão aguardando coleta: {ex.Message}");
        }
    }

    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetDeliveredAsync(PaginationCommand command)
    {
        try
        {
            command.Validate();

            return await _serviceOrderRepository.GetDeliveredAsync(command.pageNumber, command.pageSize);
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
            command.Normalize();
            command.Validate();

            var result = await _serviceOrderRepository.GetById(command.ServiceOrderId);
            if (result == null || result.Data == null)
                return new BaseResponse<ServiceOrder?>(null, 404, "Essa ordem de serviço não existe");

            var serviceOrder = result.Data;

            serviceOrder.AddEstimate(command.Solution, command.Guarantee, command.PartCost, command.LaborCost, command.RepairResult);

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

    public async Task<BaseResponse<byte[]>> AddServiceOrderDeliveryAndReturnPdfAsync(GetServiceOrderByIdCommand command)
    {
        try
        {
            command.Validate();

            var result = await _serviceOrderRepository.GetById(command.Id);
            if (result == null || result.Data == null)
                return new BaseResponse<byte[]>(null, 404, "Essa ordem de serviço não existe");

            var serviceOrder = result.Data;

            serviceOrder.AddDelivery();

            await _serviceOrderRepository.UpdateAsync(serviceOrder);

            var createPdf = await _generateServiceOrderPdf.GenerateCheckOutDocumentAsync(serviceOrder);
            if (!createPdf.IsSuccess)
                return new BaseResponse<byte[]>(null, 500, createPdf.Message);

            return createPdf;
        }
        catch (CommandException<GetServiceOrderByIdCommand> ex)
        {
            return new BaseResponse<byte[]>(null, 400, $"Erro de validação: {ex.Message}");
        }
        catch (Exception ex)
        {
            return new BaseResponse<byte[]>(null, 500, $"Ocorreu um erro desconhecido ao marcar a entrega na ordem de serviço: {ex.Message}");
        }
    }
    
    public async Task<BaseResponse<byte[]>> RegenerateAndReturnPdfAsync(GetServiceOrderByIdCommand command)
    {
        try
        {
            command.Validate();

            var result = await _serviceOrderRepository.GetById(command.Id);
            if (result == null || result.Data == null)
                return new BaseResponse<byte[]>(null, 404, "Essa ordem de serviço não existe");

            var serviceOrder = result.Data;

            var createPdf = await _generateServiceOrderPdf.RegeneratePdfAsync(serviceOrder);
            if (!createPdf.IsSuccess)
                return new BaseResponse<byte[]>(null, 500, createPdf.Message);

            return createPdf;
        }
        catch (CommandException<GetServiceOrderByIdCommand> ex)
        {
            return new BaseResponse<byte[]>(null, 400, $"Erro de validação: {ex.Message}");
        }
        catch (Exception ex)
        {
            return new BaseResponse<byte[]>(null, 500, $"Ocorreu um erro desconhecido ao gerar o pdf: {ex.Message}");
        }
    }
}