using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using TVS_App.Web.Handlers;
using TVS_App.Web.Services;
using ServiceOrderHandler = TVS_App.Web.Handlers.ServiceOrderHandler;

namespace TVS_App.Web.Common;

public static class BuilderExtension
{
    public static void AddServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddTransient<CustomerHandler>();
        builder.Services.AddTransient<AuthorizationMessageHandler>();
        builder.Services.AddTransient<ServiceOrderHandler>();
        builder.Services.AddScoped<CustomAuthStateProvider>();
        builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthStateProvider>());
        builder.Services.AddSingleton<HubConnection>(sp =>
        {
            return new HubConnectionBuilder()
                .WithUrl(new Uri($"http://localhost:5119/osHub"), options =>
                {
                    options.SkipNegotiation = true;
                    options.Transports = HttpTransportType.WebSockets;
                })
                .WithAutomaticReconnect()
                .Build();
        });
    }
    
    public static void AddHttpClient(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddHttpClient("api", client =>
        {
            client.BaseAddress = new Uri("http://localhost:5119/");
            client.Timeout = TimeSpan.FromSeconds(60);
        }).AddHttpMessageHandler<AuthorizationMessageHandler>();
    }
}