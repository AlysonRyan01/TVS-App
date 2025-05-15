using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using TVS_App.Application.Commands.ServiceOrderCommands;
using TVS_App.Domain.Entities;
using TVS_App.Domain.Enums;
using TVS_App.Web.Handlers;

namespace TVS_App.Web.Pages;

public partial class SearchPage : ComponentBase
{
    public List<ServiceOrder> FilteredServiceOrders { get; set; } = new();
    private int _filteredServiceOrderCount => FilteredServiceOrders.Count;
    private Customer _customerFilter;
    private string _numberFilter;
    private string _serialNumberFilter;
    private string _modelFilter;
    private EEnterprise _enterpriseFilter;
    private DateTime? _startDateFilter;
    private DateTime? _endDateFilter;
    private bool _isLoading = false;
    
    [Inject] public CustomerHandler CustomerHandler { get; set; } = null!;
    [Inject] public ServiceOrderHandler ServiceOrderHandler { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; set; } = null!;
    
    private async Task<IEnumerable<Customer>> SearchCustomers(string? searchTerm, CancellationToken cancellationToken)
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

            var normalizedTerm = Normalize(searchTerm);

            return result.Data
                .Where(c => Normalize(c.Name.CustomerName).Contains(normalizedTerm))
                .Take(20);
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

    public async Task SearchByCustomerNameAsync()
    {
        try
        {
            FilteredServiceOrders = new();
            
            if (_customerFilter.Id == 0)
            {
                Snackbar.Add("Voce precisa digitar o nome de algum cliente", Severity.Error);
                return;
            }
               
            var searchResult = await ServiceOrderHandler.
                GetServiceOrdersByCustomerName(_customerFilter.Name.CustomerName);

            if (searchResult.IsSuccess)
            {
                FilteredServiceOrders = searchResult.Data!;
                StateHasChanged();
                Snackbar.Add("Ordens de serviço filtradas com sucesso!", Severity.Success);
            }
            else
            {
                Snackbar.Add($"Erro: {searchResult.Message}", Severity.Error);
            }
        }
        catch (Exception e)
        {
            Snackbar.Add($"Erro: {e.Message}", Severity.Error);
        }
    }
    
    public async Task SearchByNumberAsync()
    {
        try
        {
            FilteredServiceOrders = new();
            
            if (string.IsNullOrEmpty(_numberFilter))
            {
                Snackbar.Add("Voce precisa digitar o número da ordem", Severity.Error);
                return;
            }
            
            var normalizeNumber = new string(_numberFilter.Where(char.IsDigit).ToArray());
            if (!long.TryParse(normalizeNumber, out var longNumber) || longNumber <= 0)
            {
                Snackbar.Add("Número da OS inválido", Severity.Error);
                return;
            }
               
            var searchResult = await ServiceOrderHandler.
                GetServiceOrderById(new GetServiceOrderByIdCommand { Id = longNumber });

            if (searchResult.IsSuccess)
            {
                if (searchResult.Data!.Id != 0)
                {
                    FilteredServiceOrders.Add(searchResult.Data!);
                    StateHasChanged();
                    Snackbar.Add("Ordens de serviço filtradas com sucesso!", Severity.Success);
                }
                else
                {
                    Snackbar.Add($"Nenhuma ordem de serviço foi encontrada com o número: {longNumber}", Severity.Error);
                }
            }
            else
            {
                Snackbar.Add($"Erro: {searchResult.Message}", Severity.Error);
            }
        }
        catch (Exception e)
        {
            Snackbar.Add($"Erro: {e.Message}", Severity.Error);
        }
    }
    
    public async Task SearchBySerialNumberAsync()
    {
        try
        {
            FilteredServiceOrders = new();
            
            if (string.IsNullOrEmpty(_serialNumberFilter))
            {
                Snackbar.Add("Voce precisa digitar o número de série", Severity.Error);
                return;
            }
               
            var searchResult = await ServiceOrderHandler.
                GetServiceOrdersBySerialNumber(_serialNumberFilter);

            if (searchResult.IsSuccess)
            {
                FilteredServiceOrders = searchResult.Data!;
                StateHasChanged();
                Snackbar.Add(searchResult.Message ?? "Ordens de serviço filtradas com sucesso!", Severity.Success);
            }
            else
            {
                Snackbar.Add($"Erro: {searchResult.Message}", Severity.Error);
            }
        }
        catch (Exception e)
        {
            Snackbar.Add($"Erro: {e.Message}", Severity.Error);
        }
    }
    
    public async Task SearchByModelAsync()
    {
        try
        {
            FilteredServiceOrders = new();
            
            if (string.IsNullOrEmpty(_modelFilter))
            {
                Snackbar.Add("Voce precisa digitar o modelo do produto", Severity.Error);
                return;
            }
               
            var searchResult = await ServiceOrderHandler.
                GetServiceOrdersByModel(_modelFilter);

            if (searchResult.IsSuccess)
            {
                FilteredServiceOrders = searchResult.Data!;
                StateHasChanged();
                Snackbar.Add(searchResult.Message ?? "Ordens de serviço filtradas com sucesso!", Severity.Success);
            }
            else
            {
                Snackbar.Add($"Erro: {searchResult.Message}", Severity.Error);
            }
        }
        catch (Exception e)
        {
            Snackbar.Add($"Erro: {e.Message}", Severity.Error);
        }
    }
    
    public async Task SearchByEnterpriseAsync()
    {
        try
        {
            FilteredServiceOrders = new();
            
            if (!Enum.IsDefined(_enterpriseFilter))
            {
                Snackbar.Add("Voce precisa selecionar a empresa", Severity.Error);
                return;
            }
               
            var searchResult = await ServiceOrderHandler.
                GetServiceOrdersByEnterprise(_enterpriseFilter);

            if (searchResult.IsSuccess)
            {
                FilteredServiceOrders = searchResult.Data!;
                StateHasChanged();
                Snackbar.Add(searchResult.Message ?? "Ordens de serviço filtradas com sucesso!", Severity.Success);
            }
            else
            {
                Snackbar.Add($"Erro: {searchResult.Message}", Severity.Error);
            }
        }
        catch (Exception e)
        {
            Snackbar.Add($"Erro: {e.Message}", Severity.Error);
        }
    }
    
    public async Task SearchByDateAsync()
    {
        try
        {
            DateTime nonNullableStartDate;
            DateTime nonNullableEndDate;
            
            FilteredServiceOrders = new();

            if (_startDateFilter == null || _endDateFilter == null)
            {
                Snackbar.Add("Voce precisa definir as datas", Severity.Error);
                return;
            }
            
            nonNullableStartDate = _startDateFilter.Value;
            nonNullableEndDate = _endDateFilter.Value;
               
            var searchResult = await ServiceOrderHandler.
                GetServiceOrdersByDate(nonNullableStartDate,  nonNullableEndDate);

            if (searchResult.IsSuccess)
            {
                FilteredServiceOrders = searchResult.Data!;
                StateHasChanged();
                Snackbar.Add(searchResult.Message ?? "Ordens de serviço filtradas com sucesso!", Severity.Success);
            }
            else
            {
                Snackbar.Add($"Erro: {searchResult.Message}", Severity.Error);
            }
        }
        catch (Exception e)
        {
            Snackbar.Add($"Erro: {e.Message}", Severity.Error);
        }
    }
    
    private static string Normalize(string text)
    {
        return string.Concat(text.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
            .ToLowerInvariant();
    }
}