using System.Net.Http.Json;
using TVS_App.Application.Commands;
using TVS_App.Application.Commands.ServiceOrderCommands;
using TVS_App.Application.DTOs;
using TVS_App.Domain.Entities;
using TVS_App.Domain.Enums;
using TVS_App.Web.Exceptions;

namespace TVS_App.Web.Handlers;

public class ServiceOrderHandler
{
    private readonly HttpClient _httpClient;
    
    public ServiceOrderHandler(IHttpClientFactory clientFactory)
    {
        _httpClient = clientFactory.CreateClient("api");
    }

    public async Task<BaseResponse<byte[]>> CreateServiceOrderAndReturnPdfAsync(CreateServiceOrderCommand command)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("create-service-order", command);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new BaseResponse<byte[]>(null, (int)response.StatusCode, $"Erro: {error}");
            }

            var pdfBytes = await response.Content.ReadAsByteArrayAsync();
            return new BaseResponse<byte[]>(pdfBytes, 200, "PDF gerado com sucesso");
        }
        catch (Exception e)
        {
            return ExceptionHandler.Handle<byte[]>(e);
        }
    }

    public async Task<BaseResponse<ServiceOrder?>> UpdateServiceOrderAsync(UpdateServiceOrderCommand command)
    {
        try
        {
            command.Validate();
            var response = await _httpClient.PutAsJsonAsync("update-service-order-by-id", command);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new BaseResponse<ServiceOrder?>(null, (int)response.StatusCode, 
                    $"Erro na API: {response.StatusCode}. Detalhes: {errorContent}");
            }
            
            var content = await response.Content.ReadFromJsonAsync<BaseResponse<ServiceOrder?>>();
            if (content == null || !content.IsSuccess)
                return new BaseResponse<ServiceOrder?>(null, 500, content?.Message);
            
            return content;
        }
        catch (Exception ex)
        {
            return ExceptionHandler.Handle<ServiceOrder?>(ex);
        }
    }

    public async Task<BaseResponse<ServiceOrder?>> GetServiceOrderById(GetServiceOrderByIdCommand command)
    {
        try
        {
            command.Validate();
            var response = await _httpClient.GetFromJsonAsync<BaseResponse<ServiceOrder?>>(
                $"get-service-order-by-id/{command.Id}");
            
            if (response == null || !response.IsSuccess)
                return new BaseResponse<ServiceOrder?>(null, 500, response.Message ?? "");
            
            return response;
        }
        catch (Exception ex)
        {
            return ExceptionHandler.Handle<ServiceOrder?>(ex);
        }
    }

    public async Task<BaseResponse<List<ServiceOrder>>> GetServiceOrdersByCustomerName(string customerName)
    {
        try
        {
            var encodedName = Uri.EscapeDataString(customerName);
            var response = await _httpClient.GetFromJsonAsync<BaseResponse<List<ServiceOrder>>>(
                $"get-service-orders-by-customer-name?name={encodedName}");
            
            if (response == null || !response.IsSuccess)
                return new BaseResponse<List<ServiceOrder>>(null, 500, response?.Message);
            
            return response;
        }
        catch (Exception ex)
        {
            return ExceptionHandler.Handle<List<ServiceOrder>>(ex);
        }
    }
    
    public async Task<BaseResponse<List<ServiceOrder>>> GetServiceOrdersBySerialNumber(string serialNumber)
    {
        try
        {
            var encodedName = Uri.EscapeDataString(serialNumber);
            var response = await _httpClient.GetFromJsonAsync<BaseResponse<List<ServiceOrder>>>(
                $"get-service-orders-by-serial-number?serialNumber={encodedName}");
            
            if (response == null || !response.IsSuccess)
                return new BaseResponse<List<ServiceOrder>>(null, 500, response?.Message);
            
            return response;
        }
        catch (Exception ex)
        {
            return ExceptionHandler.Handle<List<ServiceOrder>>(ex);
        }
    }
    
    public async Task<BaseResponse<List<ServiceOrder>>> GetServiceOrdersByModel(string model)
    {
        try
        {
            var encodedName = Uri.EscapeDataString(model);
            var response = await _httpClient.GetFromJsonAsync<BaseResponse<List<ServiceOrder>>>(
                $"get-service-orders-by-model?model={encodedName}");
            
            if (response == null || !response.IsSuccess)
                return new BaseResponse<List<ServiceOrder>>(null, 500, response?.Message);
            
            return response;
        }
        catch (Exception ex)
        {
            return ExceptionHandler.Handle<List<ServiceOrder>>(ex);
        }
    }
    
    public async Task<BaseResponse<List<ServiceOrder>>> GetServiceOrdersByEnterprise(EEnterprise enterprise)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<BaseResponse<List<ServiceOrder>>>(
                $"get-service-orders-by-enterprise?enterprise={enterprise}");
            
            if (response == null || !response.IsSuccess)
                return new BaseResponse<List<ServiceOrder>>(null, 500, response?.Message);
            
            return response;
        }
        catch (Exception ex)
        {
            return ExceptionHandler.Handle<List<ServiceOrder>>(ex);
        }
    }
    
    public async Task<BaseResponse<List<ServiceOrder>>> GetServiceOrdersByDate(DateTime startDate, DateTime endDate)
    {
        try
        {
            var start = Uri.EscapeDataString(startDate.ToString("yyyy-MM-dd"));
            var end = Uri.EscapeDataString(endDate.ToString("yyyy-MM-dd"));
            
            var response = await _httpClient.GetFromJsonAsync<BaseResponse<List<ServiceOrder>>>(
                $"get-service-orders-by-date?startDate={start}&endDate={end}");
            
            if (response == null || !response.IsSuccess)
                return new BaseResponse<List<ServiceOrder>>(null, 500, response?.Message);
            
            return response;
        }
        catch (Exception ex)
        {
            return ExceptionHandler.Handle<List<ServiceOrder>>(ex);
        }
    }

    public async Task<BaseResponse<ServiceOrder?>> GetServiceOrderForCustomer(
        GetServiceOrdersForCustomerCommand command)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<BaseResponse<ServiceOrder?>>(
                $"get-service-order-for-customer/{command.ServiceOrderId}/{command.SecurityCode}");
            
            if (response == null || !response.IsSuccess)
                return new BaseResponse<ServiceOrder?>(null, 500, response?.Message);
            
            return response;
        }
        catch (Exception ex)
        {
            return ExceptionHandler.Handle<ServiceOrder?>(ex);
        }
    }

    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetAllServiceOrdersAsync(PaginationCommand command)
    {
        try
        {
            command.Validate();
            var response = await _httpClient.GetFromJsonAsync<BaseResponse<PaginatedResult<ServiceOrder?>>>(
                $"get-all-service-orders/{command.PageNumber}/{command.PageSize}");
            
            if (response == null || !response.IsSuccess)
                return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, response?.Message);
            
            return response;
        }
        catch (Exception ex)
        {
            return ExceptionHandler.Handle<PaginatedResult<ServiceOrder?>>(ex);
        }
    }

    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetPendingEstimatesAsync(PaginationCommand command)
    {
        try
        {
            command.Validate();
            var response = await _httpClient.GetFromJsonAsync<BaseResponse<PaginatedResult<ServiceOrder?>>>(
                $"get-pending-estimates-service-orders/{command.PageNumber}/{command.PageSize}");
            
            if (response == null || !response.IsSuccess)
                return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, response?.Message);
            
            return response;
        }
        catch (Exception ex)
        {
            return ExceptionHandler.Handle<PaginatedResult<ServiceOrder?>>(ex);
        }
    }

    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetWaitingResponseAsync(PaginationCommand command)
    {
        try
        {
            command.Validate();
            var response = await _httpClient.GetFromJsonAsync<BaseResponse<PaginatedResult<ServiceOrder?>>>(
                $"get-waiting-response-service-orders/{command.PageNumber}/{command.PageSize}");
            
            if (response == null || !response.IsSuccess)
                return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, response?.Message);
            
            return response;
        }
        catch (Exception ex)
        {
            return ExceptionHandler.Handle<PaginatedResult<ServiceOrder?>>(ex);
        }
    }
    
    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetPendingPurchasePartAsync(PaginationCommand command)
    {
        try
        {
            command.Validate();
            var response = await _httpClient.GetFromJsonAsync<BaseResponse<PaginatedResult<ServiceOrder?>>>(
                $"get-pending-purchase-service-orders/{command.PageNumber}/{command.PageSize}");
            
            if (response == null || !response.IsSuccess)
                return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, response?.Message);
            
            return response;
        }
        catch (Exception ex)
        {
            return ExceptionHandler.Handle<PaginatedResult<ServiceOrder?>>(ex);
        }
    }
    
    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetWaitingPartsAsync(PaginationCommand command)
    {
        try
        {
            command.Validate();
            var response = await _httpClient.GetFromJsonAsync<BaseResponse<PaginatedResult<ServiceOrder?>>>(
                $"get-waiting-parts-service-orders/{command.PageNumber}/{command.PageSize}");
            
            if (response == null || !response.IsSuccess)
                return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, response?.Message);
            
            return response;
        }
        catch (Exception ex)
        {
            return ExceptionHandler.Handle<PaginatedResult<ServiceOrder?>>(ex);
        }
    }
    
    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetWaitingPickupAsync(PaginationCommand command)
    {
        try
        {
            command.Validate();
            var response = await _httpClient.GetFromJsonAsync<BaseResponse<PaginatedResult<ServiceOrder?>>>(
                $"get-waiting-pickup-service-orders/{command.PageNumber}/{command.PageSize}");
            
            if (response == null || !response.IsSuccess)
                return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, response?.Message);
            
            return response;
        }
        catch (Exception ex)
        {
            return ExceptionHandler.Handle<PaginatedResult<ServiceOrder?>>(ex);
        }
    }
    
    public async Task<BaseResponse<PaginatedResult<ServiceOrder?>>> GetDeliveredAsync(PaginationCommand command)
    {
        try
        {
            command.Validate();
            var response = await _httpClient.GetFromJsonAsync<BaseResponse<PaginatedResult<ServiceOrder?>>>(
                $"get-delivered-service-orders/{command.PageNumber}/{command.PageSize}");
            
            if (response == null || !response.IsSuccess)
                return new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, response?.Message);
            
            return response;
        }
        catch (Exception ex)
        {
            return ExceptionHandler.Handle<PaginatedResult<ServiceOrder?>>(ex);
        }
    }

    public async Task<BaseResponse<ServiceOrder?>> AddProductLocation(AddProductLocationCommand command)
    {
        try
        {
            command.Validate();
            var response = await _httpClient.PutAsJsonAsync("add-product-location", command);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new BaseResponse<ServiceOrder?>(null, (int)response.StatusCode, 
                    $"Erro na API: {response.StatusCode}. Detalhes: {errorContent}");
            }
            
            var content = await response.Content.ReadFromJsonAsync<BaseResponse<ServiceOrder?>>();
            if (content == null || !content.IsSuccess)
                return new BaseResponse<ServiceOrder?>(null, 500, content?.Message);
            
            return content;
        }
        catch (Exception ex)
        {
            return ExceptionHandler.Handle<ServiceOrder?>(ex);
        }
    }

    public async Task<BaseResponse<ServiceOrder?>> AddServiceOrderEstimate(AddServiceOrderEstimateCommand command)
    {
        try
        {
            command.Validate();
            var response = await _httpClient.PutAsJsonAsync("add-service-order-estimate", command);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new BaseResponse<ServiceOrder?>(null, (int)response.StatusCode, 
                    $"Erro na API: {response.StatusCode}. Detalhes: {errorContent}");
            }
            
            var content = await response.Content.ReadFromJsonAsync<BaseResponse<ServiceOrder?>>();
            if (content == null || !content.IsSuccess)
                return new BaseResponse<ServiceOrder?>(null, 500, content?.Message);
            
            return content;
        }
        catch (Exception ex)
        {
            return ExceptionHandler.Handle<ServiceOrder?>(ex);
        }
    }
    
    public async Task<BaseResponse<ServiceOrder?>> AddServiceOrderApproveEstimate(GetServiceOrderByIdCommand command)
    {
        try
        {
            command.Validate();
            var response = await _httpClient.PutAsJsonAsync("add-service-order-approve-estimate", command);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new BaseResponse<ServiceOrder?>(null, (int)response.StatusCode, 
                    $"Erro na API: {response.StatusCode}. Detalhes: {errorContent}");
            }
            
            var content = await response.Content.ReadFromJsonAsync<BaseResponse<ServiceOrder?>>();
            if (content == null || !content.IsSuccess)
                return new BaseResponse<ServiceOrder?>(null, 500, content?.Message);
            
            return content;
        }
        catch (Exception ex)
        {
            return ExceptionHandler.Handle<ServiceOrder?>(ex);
        }
    }
    
    public async Task<BaseResponse<ServiceOrder?>> AddServiceOrderRejectEstimate(GetServiceOrderByIdCommand command)
    {
        try
        {
            command.Validate();
            var response = await _httpClient.PutAsJsonAsync("add-service-order-reject-estimate", command);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new BaseResponse<ServiceOrder?>(null, (int)response.StatusCode, 
                    $"Erro na API: {response.StatusCode}. Detalhes: {errorContent}");
            }
            
            var content = await response.Content.ReadFromJsonAsync<BaseResponse<ServiceOrder?>>();
            if (content == null || !content.IsSuccess)
                return new BaseResponse<ServiceOrder?>(null, 500, content?.Message);
            
            return content;
        }
        catch (Exception ex)
        {
            return ExceptionHandler.Handle<ServiceOrder?>(ex);
        }
    }

    public async Task<BaseResponse<ServiceOrder?>> AddPurchasedPart(GetServiceOrderByIdCommand command)
    {
        try
        {
            command.Validate();
            var response = await _httpClient.PutAsJsonAsync("add-service-order-purchased-part", command);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new BaseResponse<ServiceOrder?>(null, (int)response.StatusCode,
                    $"Erro na API: {response.StatusCode}. Detalhes: {errorContent}");
            }

            var content = await response.Content.ReadFromJsonAsync<BaseResponse<ServiceOrder?>>();
            if (content == null || !content.IsSuccess)
                return new BaseResponse<ServiceOrder?>(null, 500, content?.Message);

            return content;
        }
        catch (Exception ex)
        {
            return ExceptionHandler.Handle<ServiceOrder?>(ex);
        }
    }

    public async Task<BaseResponse<ServiceOrder?>> AddServiceOrderRepair(GetServiceOrderByIdCommand command)
    {
        try
        {
            command.Validate();
            var response = await _httpClient.PutAsJsonAsync("add-service-order-repair", command);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new BaseResponse<ServiceOrder?>(null, (int)response.StatusCode,
                    $"Erro na API: {response.StatusCode}. Detalhes: {errorContent}");
            }

            var content = await response.Content.ReadFromJsonAsync<BaseResponse<ServiceOrder?>>();
            if (content == null || !content.IsSuccess)
                return new BaseResponse<ServiceOrder?>(null, 500, content?.Message);

            return content;
        }
        catch (Exception ex)
        {
            return ExceptionHandler.Handle<ServiceOrder?>(ex);
        }
    }

    public async Task<BaseResponse<byte[]>> AddServiceOrderDeliveryAndReturnPdfAsync(GetServiceOrderByIdCommand command)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync("add-service-order-delivery", command);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new BaseResponse<byte[]>(null, (int)response.StatusCode, $"Erro: {error}");
            }

            var pdfBytes = await response.Content.ReadAsByteArrayAsync();
            return new BaseResponse<byte[]>(pdfBytes, 200, "PDF gerado com sucesso");
        }
        catch (Exception e)
        {
            return ExceptionHandler.Handle<byte[]>(e);
        }
    }

    public async Task<BaseResponse<byte[]>> RegenerateAndReturnPdfAsync(GetServiceOrderByIdCommand command)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("regenerate-service-order-pdf", command);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return new BaseResponse<byte[]>(null, (int)response.StatusCode, $"Erro: {error}");
            }

            var pdfBytes = await response.Content.ReadAsByteArrayAsync();
            return new BaseResponse<byte[]>(pdfBytes, 200, "PDF gerado com sucesso");
        }
        catch (Exception e)
        {
            return ExceptionHandler.Handle<byte[]>(e);
        }
    }
}