using TVS_App.Application.DTOs;
using TVS_App.Domain.Entities;

namespace TVS_App.Application.Repositories;

public interface IServiceOrderRepository
{
    Task<BaseResponse<ServiceOrder?>> CreateAsync(ServiceOrder serviceOrder);
    Task<BaseResponse<ServiceOrder?>> UpdateAsync(ServiceOrder serviceOrder);

    Task<BaseResponse<ServiceOrder?>> GetById(long id);
    Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetAllAsync(int pageNumber, int pageSize);

    Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetPendingEstimatesAsync(int pageNumber, int pageSize);
    Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetWaitingResponseAsync(int pageNumber, int pageSize);
    Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetWaitingPartsAsync(int pageNumber, int pageSize);
    Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetWaitingPickupAsync(int pageNumber, int pageSize);
    Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetDeliveredAsync(int pageNumber, int pageSize);
}