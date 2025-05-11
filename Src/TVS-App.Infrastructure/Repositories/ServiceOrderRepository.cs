using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TVS_App.Application.DTOs;
using TVS_App.Application.Repositories;
using TVS_App.Domain.Entities;
using TVS_App.Domain.Enums;
using TVS_App.Infrastructure.Data;
using TVS_App.Infrastructure.Exceptions;

namespace TVS_App.Infrastructure.Repositories;

public class ServiceOrderRepository : IServiceOrderRepository
{
    private readonly ApplicationDataContext _context;

    public ServiceOrderRepository(ApplicationDataContext context)
    {
        _context = context;
    }

    public async Task<BaseResponse<ServiceOrder?>> CreateAsync(ServiceOrder serviceOrder)
    {
        IDbContextTransaction? transaction = null;
        
        try
        {
            if (serviceOrder == null)
                return new BaseResponse<ServiceOrder?>(null, 400, "A ordem de serviço não pode ser nula");

            if (serviceOrder.Product == null)
                return new BaseResponse<ServiceOrder?>(null, 400, "O produto da ordem de serviço não pode ser nulo");

            transaction = await _context.Database.BeginTransactionAsync();

            await _context.ServiceOrders.AddAsync(serviceOrder);
            await _context.SaveChangesAsync();

            var customer = await _context.Customers
            .FirstOrDefaultAsync(c => c.Id == serviceOrder.CustomerId);

            if (customer == null)
                return new BaseResponse<ServiceOrder?>(null, 404, "Cliente não encontrado");

            serviceOrder.UpdateCustomer(customer);

            await transaction.CommitAsync();

            return new BaseResponse<ServiceOrder?>(serviceOrder, 200, "Ordem de serviço criada com sucesso!");
        }
        catch (Exception ex)
        {
            if (transaction != null)
                await transaction.RollbackAsync();

            return DbExceptionHandler.Handle<ServiceOrder?>(ex);
        }
    }

    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetAllAsync(int pageNumber, int pageSize)
    {
        try
        {
            if (pageNumber < 1)
                return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 400, "O número da página deve ser maior que zero.");
    
            if (pageSize < 1)
                return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 400, "O tamanho da página deve ser maior que zero.");

            var totalCount = await _context.ServiceOrders.CountAsync();

            var serviceOrders = await _context.ServiceOrders
                .AsNoTracking()
                .Include(x => x.Customer)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int? totalPages = totalCount > 0 ? (int?)Math.Ceiling(totalCount / (double)pageSize) : null;

            var result = new PaginatedResult<ServiceOrder?>(serviceOrders, totalCount, pageNumber, pageSize, totalPages);

            return new BaseResponse<PaginatedResult<ServiceOrder?>>(result, 200, "Ordens de serviço retornadas com sucesso!");
        }
        catch (Exception ex)
        {
            return DbExceptionHandler.Handle<PaginatedResult<ServiceOrder?>>(ex);
        }
    }

    public async Task<BaseResponse<ServiceOrder?>> GetById(long id)
    {
        try
        {
            if (id <= 0)
                return new BaseResponse<ServiceOrder?>(null, 400, "O ID não pode ser igual ou menor que 0");

            var serviceOrder = await _context.ServiceOrders
                .Include(x => x.Customer)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if (serviceOrder == null)
                return new BaseResponse<ServiceOrder?>(null, 404, $"A ordem de serviço com ID:{id} não existe");

            return new BaseResponse<ServiceOrder?>(serviceOrder, 200, "Ordem de serviço recuperada com sucesso!");
        }
        catch (Exception ex)
        {
            return DbExceptionHandler.Handle<ServiceOrder?>(ex);
        }
    }

    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetDeliveredAsync(int pageNumber, int pageSize)
    {
        try
        {
            if (pageNumber < 1)
                return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 400, "O número da página deve ser maior que zero.");

            if (pageSize < 1)
                return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 400, "O tamanho da página deve ser maior que zero.");

            var query = _context.ServiceOrders
                .Where(x => x.ServiceOrderStatus == EServiceOrderStatus.Delivered)
                .OrderBy(x => x.Id);

            var totalCount = await query.CountAsync();

            var deliveredServiceOrders = await query
                .Include(x => x.Customer)
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int? totalPages = totalCount > 0 ? (int?)Math.Ceiling(totalCount / (double)pageSize) : null;

            var result = new PaginatedResult<ServiceOrder?>(deliveredServiceOrders, totalCount, pageNumber, pageSize, totalPages);
            return new BaseResponse<PaginatedResult<ServiceOrder?>>(result, 200, "Ordens de serviço entregues recuperadas com sucesso!");
        }
        catch (Exception ex)
        {
            return DbExceptionHandler.Handle<PaginatedResult<ServiceOrder?>>(ex);
        }
    }

    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetPendingEstimatesAsync(int pageNumber, int pageSize)
    {
        try
        {
            if (pageNumber < 1)
                return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 400, "O número da página deve ser maior que zero.");

            if (pageSize < 1)
                return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 400, "O tamanho da página deve ser maior que zero.");

            var query = _context.ServiceOrders
                .Where(x => x.ServiceOrderStatus == EServiceOrderStatus.Entered && x.RepairStatus == ERepairStatus.Entered)
                .OrderBy(x => x.Id);

            var totalCount = await query.CountAsync();

            var deliveredServiceOrders = await query
                .Include(x => x.Customer)
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int? totalPages = totalCount > 0 ? (int?)Math.Ceiling(totalCount / (double)pageSize) : null;

            var result = new PaginatedResult<ServiceOrder?>(deliveredServiceOrders, totalCount, pageNumber, pageSize, totalPages);
            return new BaseResponse<PaginatedResult<ServiceOrder?>>(result, 200, "Ordens de serviço pendentes de orçamento recuperadas com sucesso!");
        }
        catch (Exception ex)
        {
            return DbExceptionHandler.Handle<PaginatedResult<ServiceOrder?>>(ex);
        }
    }

    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetPendingPartPurchase(int pageNumber, int pageSize)
    {
        try
        {
            if (pageNumber < 1)
                return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 400, "O número da página deve ser maior que zero.");

            if (pageSize < 1)
                return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 400, "O tamanho da página deve ser maior que zero.");

            var query = _context.ServiceOrders
                .Where(x => x.ServiceOrderStatus == EServiceOrderStatus.Evaluated && x.RepairStatus == ERepairStatus.Approved && !x.PurchasePartDate.HasValue)
                .OrderBy(x => x.Id);

            var totalCount = await query.CountAsync();

            var deliveredServiceOrders = await query
                .Include(x => x.Customer)
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int? totalPages = totalCount > 0 ? (int?)Math.Ceiling(totalCount / (double)pageSize) : null;

            var result = new PaginatedResult<ServiceOrder?>(deliveredServiceOrders, totalCount, pageNumber, pageSize, totalPages);
            return new BaseResponse<PaginatedResult<ServiceOrder?>>(result, 200, "Ordens de serviço pendentes de compra de peça recuperadas com sucesso!");
        }
        catch (Exception ex)
        {
            return DbExceptionHandler.Handle<PaginatedResult<ServiceOrder?>>(ex);
        }
    }

    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetWaitingPartsAsync(int pageNumber, int pageSize)
    {
        try
        {
            if (pageNumber < 1)
                return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 400, "O número da página deve ser maior que zero.");

            if (pageSize < 1)
                return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 400, "O tamanho da página deve ser maior que zero.");

            var query = _context.ServiceOrders
                .Where(x => x.ServiceOrderStatus == EServiceOrderStatus.OrderPart && x.RepairStatus == ERepairStatus.Approved && x.PurchasePartDate.HasValue)
                .OrderBy(x => x.Id);

            var totalCount = await query.CountAsync();

            var deliveredServiceOrders = await query
                .Include(x => x.Customer)
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int? totalPages = totalCount > 0 ? (int?)Math.Ceiling(totalCount / (double)pageSize) : null;

            var result = new PaginatedResult<ServiceOrder?>(deliveredServiceOrders, totalCount, pageNumber, pageSize, totalPages);
            return new BaseResponse<PaginatedResult<ServiceOrder?>>(result, 200, "Ordens de serviço aguardando peça recuperadas com sucesso!");
        }
        catch (Exception ex)
        {
            return DbExceptionHandler.Handle<PaginatedResult<ServiceOrder?>>(ex);
        }
    }

    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetWaitingPickupAsync(int pageNumber, int pageSize)
    {
        try
        {
            if (pageNumber < 1)
                return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 400, "O número da página deve ser maior que zero.");

            if (pageSize < 1)
                return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 400, "O tamanho da página deve ser maior que zero.");

            var query = _context.ServiceOrders
                .Where(x => x.ServiceOrderStatus == EServiceOrderStatus.Repaired ||
                (x.ServiceOrderStatus == EServiceOrderStatus.Evaluated &&
                (x.RepairStatus == ERepairStatus.Disapproved || x.RepairResult == ERepairResult.Unrepaired || x.RepairResult == ERepairResult.NoDefectFound)))
                .OrderBy(x => x.Id);

            var totalCount = await query.CountAsync();

            var deliveredServiceOrders = await query
                .Include(x => x.Customer)
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int? totalPages = totalCount > 0 ? (int?)Math.Ceiling(totalCount / (double)pageSize) : null;

            var result = new PaginatedResult<ServiceOrder?>(deliveredServiceOrders, totalCount, pageNumber, pageSize, totalPages);
            return new BaseResponse<PaginatedResult<ServiceOrder?>>(result, 200, "Ordens de serviço aguardando coleta recuperadas com sucesso!");
        }
        catch (Exception ex)
        {
            return DbExceptionHandler.Handle<PaginatedResult<ServiceOrder?>>(ex);
        }
    }

    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetWaitingResponseAsync(int pageNumber, int pageSize)
    {
        try
        {
            if (pageNumber < 1)
                return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 400, "O número da página deve ser maior que zero.");

            if (pageSize < 1)
                return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 400, "O tamanho da página deve ser maior que zero.");

            var query = _context.ServiceOrders
                .Where(x => x.ServiceOrderStatus == EServiceOrderStatus.Evaluated && x.RepairStatus == ERepairStatus.Waiting)
                .OrderBy(x => x.Id);

            var totalCount = await query.CountAsync();

            var deliveredServiceOrders = await query
                .Include(x => x.Customer)
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            int? totalPages = totalCount > 0 ? (int?)Math.Ceiling(totalCount / (double)pageSize) : null;

            var result = new PaginatedResult<ServiceOrder?>(deliveredServiceOrders, totalCount, pageNumber, pageSize, totalPages);
            return new BaseResponse<PaginatedResult<ServiceOrder?>>(result, 200, "Ordens de serviço aguardando resposta do cliente recuperadas com sucesso!");
        }
        catch (Exception ex)
        {
            return DbExceptionHandler.Handle<PaginatedResult<ServiceOrder?>>(ex);
        }
    }

    public async Task<BaseResponse<ServiceOrder?>> UpdateAsync(ServiceOrder serviceOrder)
    {
        IDbContextTransaction? transaction = null;
        
        try
        {
            if (serviceOrder == null)
                return new BaseResponse<ServiceOrder?>(null, 400, "A ordem de serviço não pode ser nula");

            if (serviceOrder.Product == null)
                return new BaseResponse<ServiceOrder?>(null, 400, "O produto da ordem de serviço não pode ser nulo");

            transaction = await _context.Database.BeginTransactionAsync();

            _context.ServiceOrders.Update(serviceOrder);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            return new BaseResponse<ServiceOrder?>(serviceOrder, 200, "Ordem de serviço atualizada com sucesso!");
        }
        catch (Exception ex)
        {
            if (transaction != null)
                await transaction.RollbackAsync();

            return DbExceptionHandler.Handle<ServiceOrder?>(ex);
        }
    }
}