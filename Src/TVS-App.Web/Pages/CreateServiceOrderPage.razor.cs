using Microsoft.AspNetCore.Components;
using MudBlazor;
using TVS_App.Application.Commands.CustomerCommands;
using TVS_App.Application.Commands.ServiceOrderCommands;
using TVS_App.Domain.Entities;
using TVS_App.Web.Handlers;

namespace TVS_App.Web.Pages;

public partial class CreateServiceOrderPage : ComponentBase
{
    public CreateCustomerCommand CreateCustomerCommand { get; set; } = new();
    public CreateServiceOrderCommand CreateServiceOrderCommand { get; set; } = new();
    public Customer SelectedCustomer { get; set; } = new();
    private bool _isLoading = false;

    [Inject] public CustomerHandler CustomerHandler { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;

    private async Task<IEnumerable<Customer>> SearchCustomers(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm.Length < 3)
            return Enumerable.Empty<Customer>();

        try
        {
            _isLoading = true;
            var result = await CustomerHandler.GetCustomerByNameAsync(searchTerm);

            if (!result.IsSuccess || result.Data == null)
            {
                Snackbar.Add(result.Message ?? "Erro ao buscar clientes", Severity.Error);
                return Enumerable.Empty<Customer>();
            }

            return result.Data.Take(20);
        }
        catch (Exception e)
        {
            Snackbar.Add($"Erro: {e.Message}", Severity.Error);
            return Enumerable.Empty<Customer>();
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void SelectCustomer(Customer customer)
    {
        SelectedCustomer = customer;
        CreateServiceOrderCommand.CustomerId = customer.Id;
    }
}