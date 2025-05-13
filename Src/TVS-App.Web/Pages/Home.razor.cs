using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace TVS_App.Web.Pages;

public partial class Home : ComponentBase
{
    protected override async Task OnInitializedAsync()
    {
        try
        {
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro na inicialização: {ex.Message}");
        }
    }
}