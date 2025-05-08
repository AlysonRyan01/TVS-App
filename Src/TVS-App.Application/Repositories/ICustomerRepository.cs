using TVS_App.Application.DTOs;
using TVS_App.Domain.Entities;

namespace TVS_App.Application.Repositories;

public interface ICustomerRepository
{
    Task<BaseResponse<Customer?>> CreateAsync(Customer customer);
    Task<BaseResponse<Customer?>> UpdateAsync(Customer customer);
    Task<BaseResponse<Customer?>> GetByIdAsync(long id);
    Task<BaseResponse<IEnumerable<Customer>>> GetAllAsync();
}
