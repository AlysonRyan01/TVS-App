using TVS_App.Application.DTOs;
using TVS_App.Application.Repositories;
using TVS_App.Domain.Entities;

namespace TVS_App.Tests.Mocks;

public class FakeCustomerRepository : ICustomerRepository
{
    private readonly List<Customer> _customers = new();
    private long _idCounter = 1;

    public Task<BaseResponse<Customer?>> CreateAsync(Customer customer)
    {
        customer.Id = _idCounter++;
        _customers.Add(customer);
        return Task.FromResult(new BaseResponse<Customer?>(customer, 200, "Cliente obtido com sucesso!"));
    }

    public Task<BaseResponse<Customer?>> UpdateAsync(Customer customer)
    {
        var index = _customers.FindIndex(c => c.Id == customer.Id);
        if (index == -1)
            return Task.FromResult(new BaseResponse<Customer?>(null, 404, "Cliente não encontrado"));

        _customers[index] = customer;
        return Task.FromResult(new BaseResponse<Customer?>(customer, 200, "Cliente obtido com sucesso!"));
    }

    public Task<BaseResponse<Customer?>> GetByIdAsync(long id)
    {
        var customer = _customers.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(new BaseResponse<Customer?>(customer, 200, "Cliente obtido com sucesso!"));
    }

    public Task<BaseResponse<PaginatedResult<Customer?>>> GetAllAsync(int pageNumber, int pageSize)
    {
        int totalCount = _customers.Count;

        var paged = _customers
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var result = new PaginatedResult<Customer?>(paged, totalCount, pageNumber, pageSize);

        return Task.FromResult(new BaseResponse<PaginatedResult<Customer?>>(result, 200, "Todos os clientes foram obtidos com sucesso!"));
    }
}
