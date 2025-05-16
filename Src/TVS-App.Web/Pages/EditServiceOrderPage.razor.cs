using Microsoft.AspNetCore.Components;
using MudBlazor;
using TVS_App.Application.Commands.ServiceOrderCommands;
using TVS_App.Domain.Entities;
using TVS_App.Web.Components.Dialogs;
using TVS_App.Web.Handlers;

namespace TVS_App.Web.Pages;

public partial class EditServiceOrderPage : ComponentBase
{
    private string _serviceOrderId = string.Empty;
    
    [Inject] public ISnackbar Snackbar { get; set; } = null!;
    [Inject] public IDialogService DialogService { get; set; } = null!;
    [Inject] public ServiceOrderHandler ServiceOrderHandler { get; set; } = null!;

    public async Task OpenEditServiceOrderDialog()
    {
        var normalizeNumber = new string(_serviceOrderId.Where(char.IsDigit).ToArray());
        if (!long.TryParse(normalizeNumber, out var longNumber) || longNumber <= 0)
        {
            Snackbar.Add("Número da OS inválido", Severity.Error);
            return;
        }
        
        var serviceOrderResult = await ServiceOrderHandler.GetServiceOrderById(
            new GetServiceOrderByIdCommand{Id = longNumber});
        if (serviceOrderResult.IsSuccess)
        {
            if (serviceOrderResult.Data != null)
            {
                await OpenEditDialog(serviceOrderResult.Data);
            }
            else
            {
                Snackbar.Add("Ocorreu um erro ao tentar buscar a ordem de serviço");
            }
        }
        else
        {
            Snackbar.Add(serviceOrderResult.Message ?? "Ocorreu um erro ao tentar buscar a ordem de serviço", Severity.Error);
        }
    }
    
    public async Task OpenEditDialog(ServiceOrder order)
    {
        var parameters = new DialogParameters
        {
            ["ServiceOrder"] = order
        };

        var options = new DialogOptions
        {
            CloseButton = true,
            FullWidth = true,
            MaxWidth = MaxWidth.Large
        };

        await DialogService.ShowAsync<EditServiceOrderDialog>("Editar ordem de serviço", parameters, options);
    }
    
}