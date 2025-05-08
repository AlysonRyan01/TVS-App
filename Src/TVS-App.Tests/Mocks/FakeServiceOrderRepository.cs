using TVS_App.Application.DTOs;
using TVS_App.Application.Repositories;
using TVS_App.Domain.Entities;

namespace TVS_App.Tests.Mocks;

public class FakeServiceOrderRepository : IServiceOrderRepository
{
    private readonly List<ServiceOrder?> _serviceOrders = new();
    private long _idCounter = 1;

    public Task<BaseResponse<ServiceOrder?>> CreateAsync(ServiceOrder serviceOrder)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<PaginatedResult<ServiceOrder>>> GetAllAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<ServiceOrder?>> GetById(long id)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<PaginatedResult<ServiceOrder>>> GetDeliveredAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<PaginatedResult<ServiceOrder>>> GetPendingEstimatesAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<PaginatedResult<ServiceOrder>>> GetWaitingPartsAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<PaginatedResult<ServiceOrder>>> GetWaitingPickupAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<PaginatedResult<ServiceOrder>>> GetWaitingResponseAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<ServiceOrder?>> UpdateAsync(ServiceOrder serviceOrder)
    {
        throw new NotImplementedException();
    }
}