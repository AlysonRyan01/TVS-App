using TVS_App.Application.DTOs;
using TVS_App.Domain.Entities;
using TVS_App.Domain.Enums;

namespace TVS_App.Application.Repositories;

public interface IServiceOrderRepository
{
    Task<BaseResponse<ServiceOrder?>> CreateAsync(ServiceOrder serviceOrder);
    Task<BaseResponse<ServiceOrder?>> UpdateAsync();
    Task<BaseResponse<ServiceOrder?>> GetById();
    Task<BaseResponse<IEnumerable<ServiceOrder>>> GetAllAsync();
    Task<BaseResponse<IEnumerable<ServiceOrder>>> GetByStatusAsync(EServiceOrderStatus status);
}