using TVS_App.Api.Exceptions;
using TVS_App.Application.Commands;
using TVS_App.Application.Commands.ServiceOrderCommands;
using TVS_App.Application.DTOs;
using TVS_App.Application.Handlers;
using TVS_App.Domain.Entities;

namespace TVS_App.Api.Endpoints;

public static class ServiceOrderEndpoints
{
    public static void MapServiceOrderEndpoints(this WebApplication app)
    {
        app.MapPost("/create-service-order", async (ServiceOrderHandler handler, CreateServiceOrderCommand command) =>
        {
            try
            {
                command.Validate();

                var createOrderResult = await handler.CreateServiceOrderAndReturnPdfAsync(command);
                if (!createOrderResult.IsSuccess)
                    return Results.BadRequest(new BaseResponse<ServiceOrder>(null, 404, createOrderResult.Message));

                return Results.File(createOrderResult.Data!, "application/pdf", "ordem_servico.pdf");
            }
            catch (Exception ex)
            {
                var response = EndpointExceptions.Handle<ServiceOrder>(ex);
                return Results.BadRequest(response ?? new BaseResponse<ServiceOrder>(null, 500, $"Ocorreu um erro desconhecido: {ex.Message}"));
            }
        });

        app.MapPut("/update-service-order-by-id", async (ServiceOrderHandler handler, UpdateServiceOrderCommand command) =>
        {
            try
            {
                command.Validate();

                var createOrderResult = await handler.UpdateServiceOrderAsync(command);
                if (!createOrderResult.IsSuccess)
                    return Results.BadRequest(new BaseResponse<ServiceOrder>(null, 404, createOrderResult.Message));

                return Results.Ok(createOrderResult);
            }
            catch (Exception ex)
            {
                var response = EndpointExceptions.Handle<ServiceOrder>(ex);
                return Results.BadRequest(response ?? new BaseResponse<ServiceOrder>(null, 500, $"Ocorreu um erro desconhecido: {ex.Message}"));
            }
        });

        app.MapGet("/get-service-order-by-id/{id}", async (ServiceOrderHandler handler, long id) =>
        {
            try
            {
                var command = new GetServiceOrderByIdCommand { Id = id };
                command.Validate();

                var createOrderResult = await handler.GetServiceOrderById(command);
                if (!createOrderResult.IsSuccess)
                    return Results.BadRequest(new BaseResponse<ServiceOrder>(null, 404, createOrderResult.Message));

                return Results.Ok(createOrderResult);
            }
            catch (Exception ex)
            {
                var response = EndpointExceptions.Handle<ServiceOrder>(ex);
                return Results.BadRequest(response ?? new BaseResponse<ServiceOrder>(null, 500, $"Ocorreu um erro desconhecido: {ex.Message}"));
            }
        });

        app.MapGet("/get-service-order-for-customer/{id}/{code}", async (ServiceOrderHandler handler, long id, string code) =>
        {
            try
            {
                var command = new GetServiceOrdersForCustomerCommand { ServiceOrderId = id, SecurityCode = code };
                command.Validate();

                var createOrderResult = await handler.GetServiceOrderForCustomer(command);
                if (!createOrderResult.IsSuccess)
                    return Results.BadRequest(new BaseResponse<ServiceOrder>(null, 404, createOrderResult.Message));

                return Results.Ok(createOrderResult);
            }
            catch (Exception ex)
            {
                var response = EndpointExceptions.Handle<ServiceOrder>(ex);
                return Results.BadRequest(response ?? new BaseResponse<ServiceOrder>(null, 500, $"Ocorreu um erro desconhecido: {ex.Message}"));
            }
        });

        app.MapGet("/get-all-service-orders/{pageNumber}/{pageSize}", async (ServiceOrderHandler handler, int pageNumber, int pageSize) =>
        {
            try
            {
                var command = new PaginationCommand { pageNumber = pageNumber, pageSize = pageSize };
                command.Validate();

                var createOrderResult = await handler.GetAllServiceOrdersAsync(command);
                if (!createOrderResult.IsSuccess)
                    return Results.BadRequest(new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 404, createOrderResult.Message));

                return Results.Ok(createOrderResult);
            }
            catch (Exception ex)
            {
                var response = EndpointExceptions.Handle<PaginatedResult<ServiceOrder?>>(ex);
                return Results.BadRequest(response ?? new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, $"Ocorreu um erro desconhecido: {ex.Message}"));
            }
        });

        app.MapGet("/get-pending-estimates-service-orders/{pageNumber}/{pageSize}", async (ServiceOrderHandler handler, int pageNumber, int pageSize) =>
        {
            try
            {
                var command = new PaginationCommand { pageNumber = pageNumber, pageSize = pageSize };
                command.Validate();

                var createOrderResult = await handler.GetPendingEstimatesAsync(command);
                if (!createOrderResult.IsSuccess)
                    return Results.BadRequest(new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 404, createOrderResult.Message));

                return Results.Ok(createOrderResult);
            }
            catch (Exception ex)
            {
                var response = EndpointExceptions.Handle<PaginatedResult<ServiceOrder?>>(ex);
                return Results.BadRequest(response ?? new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, $"Ocorreu um erro desconhecido: {ex.Message}"));
            }
        });

        app.MapGet("/get-waiting-response-service-orders/{pageNumber}/{pageSize}", async (ServiceOrderHandler handler, int pageNumber, int pageSize) =>
        {
            try
            {
                var command = new PaginationCommand { pageNumber = pageNumber, pageSize = pageSize };
                command.Validate();

                var createOrderResult = await handler.GetWaitingResponseAsync(command);
                if (!createOrderResult.IsSuccess)
                    return Results.BadRequest(new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 404, createOrderResult.Message));

                return Results.Ok(createOrderResult);
            }
            catch (Exception ex)
            {
                var response = EndpointExceptions.Handle<PaginatedResult<ServiceOrder?>>(ex);
                return Results.BadRequest(response ?? new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, $"Ocorreu um erro desconhecido: {ex.Message}"));
            }
        });

        app.MapGet("/get-pending-purchase-service-orders/{pageNumber}/{pageSize}", async (ServiceOrderHandler handler, int pageNumber, int pageSize) =>
        {
            try
            {
                var command = new PaginationCommand { pageNumber = pageNumber, pageSize = pageSize };
                command.Validate();

                var createOrderResult = await handler.GetPendingPurchasePartAsync(command);
                if (!createOrderResult.IsSuccess)
                    return Results.BadRequest(new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 404, createOrderResult.Message));

                return Results.Ok(createOrderResult);
            }
            catch (Exception ex)
            {
                var response = EndpointExceptions.Handle<PaginatedResult<ServiceOrder?>>(ex);
                return Results.BadRequest(response ?? new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, $"Ocorreu um erro desconhecido: {ex.Message}"));
            }
        });

        app.MapGet("/get-waiting-parts-service-orders/{pageNumber}/{pageSize}", async (ServiceOrderHandler handler, int pageNumber, int pageSize) =>
        {
            try
            {
                var command = new PaginationCommand { pageNumber = pageNumber, pageSize = pageSize };
                command.Validate();

                var createOrderResult = await handler.GetWaitingPartsAsync(command);
                if (!createOrderResult.IsSuccess)
                    return Results.BadRequest(new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 404, createOrderResult.Message));

                return Results.Ok(createOrderResult);
            }
            catch (Exception ex)
            {
                var response = EndpointExceptions.Handle<PaginatedResult<ServiceOrder?>>(ex);
                return Results.BadRequest(response ?? new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, $"Ocorreu um erro desconhecido: {ex.Message}"));
            }
        });

        app.MapGet("/get-waiting-pickup-service-orders/{pageNumber}/{pageSize}", async (ServiceOrderHandler handler, int pageNumber, int pageSize) =>
        {
            try
            {
                var command = new PaginationCommand { pageNumber = pageNumber, pageSize = pageSize };
                command.Validate();

                var createOrderResult = await handler.GetWaitingPickupAsync(command);
                if (!createOrderResult.IsSuccess)
                    return Results.BadRequest(new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 404, createOrderResult.Message));

                return Results.Ok(createOrderResult);
            }
            catch (Exception ex)
            {
                var response = EndpointExceptions.Handle<PaginatedResult<ServiceOrder?>>(ex);
                return Results.BadRequest(response ?? new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, $"Ocorreu um erro desconhecido: {ex.Message}"));
            }
        });

        app.MapGet("/get-delivered-service-orders/{pageNumber}/{pageSize}", async (ServiceOrderHandler handler, int pageNumber, int pageSize) =>
        {
            try
            {
                var command = new PaginationCommand { pageNumber = pageNumber, pageSize = pageSize };
                command.Validate();

                var createOrderResult = await handler.GetDeliveredAsync(command);
                if (!createOrderResult.IsSuccess)
                    return Results.BadRequest(new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 404, createOrderResult.Message));

                return Results.Ok(createOrderResult);
            }
            catch (Exception ex)
            {
                var response = EndpointExceptions.Handle<PaginatedResult<ServiceOrder?>>(ex);
                return Results.BadRequest(response ?? new BaseResponse<PaginatedResult<ServiceOrder?>>(null, 500, $"Ocorreu um erro desconhecido: {ex.Message}"));
            }
        });

        app.MapPut("/add-service-order-estimate", async (ServiceOrderHandler handler, AddServiceOrderEstimateCommand command) =>
        {
            try
            {
                command.Validate();

                var createOrderResult = await handler.AddServiceOrderEstimate(command);
                if (!createOrderResult.IsSuccess)
                    return Results.BadRequest(new BaseResponse<ServiceOrder>(null, 404, createOrderResult.Message));

                return Results.Ok(createOrderResult);
            }
            catch (Exception ex)
            {
                var response = EndpointExceptions.Handle<ServiceOrder>(ex);
                return Results.BadRequest(response ?? new BaseResponse<ServiceOrder>(null, 500, $"Ocorreu um erro desconhecido: {ex.Message}"));
            }
        });

        app.MapPut("/add-service-order-approve-estimate", async (ServiceOrderHandler handler, GetServiceOrderByIdCommand command) =>
        {
            try
            {
                command.Validate();

                var createOrderResult = await handler.AddServiceOrderApproveEstimate(command);
                if (!createOrderResult.IsSuccess)
                    return Results.BadRequest(new BaseResponse<ServiceOrder>(null, 404, createOrderResult.Message));

                return Results.Ok(createOrderResult);
            }
            catch (Exception ex)
            {
                var response = EndpointExceptions.Handle<ServiceOrder>(ex);
                return Results.BadRequest(response ?? new BaseResponse<ServiceOrder>(null, 500, $"Ocorreu um erro desconhecido: {ex.Message}"));
            }
        });

        app.MapPut("/add-service-order-reject-estimate", async (ServiceOrderHandler handler, GetServiceOrderByIdCommand command) =>
        {
            try
            {
                command.Validate();

                var createOrderResult = await handler.AddServiceOrderRejectEstimate(command);
                if (!createOrderResult.IsSuccess)
                    return Results.BadRequest(new BaseResponse<ServiceOrder>(null, 404, createOrderResult.Message));

                return Results.Ok(createOrderResult);
            }
            catch (Exception ex)
            {
                var response = EndpointExceptions.Handle<ServiceOrder>(ex);
                return Results.BadRequest(response ?? new BaseResponse<ServiceOrder>(null, 500, $"Ocorreu um erro desconhecido: {ex.Message}"));
            }
        });

        app.MapPut("/add-service-order-purchased-part", async (ServiceOrderHandler handler, GetServiceOrderByIdCommand command) =>
        {
            try
            {
                command.Validate();

                var createOrderResult = await handler.AddPurchasedPart(command);
                if (!createOrderResult.IsSuccess)
                    return Results.BadRequest(new BaseResponse<ServiceOrder>(null, 404, createOrderResult.Message));

                return Results.Ok(createOrderResult);
            }
            catch (Exception ex)
            {
                var response = EndpointExceptions.Handle<ServiceOrder>(ex);
                return Results.BadRequest(response ?? new BaseResponse<ServiceOrder>(null, 500, $"Ocorreu um erro desconhecido: {ex.Message}"));
            }
        });

        app.MapPut("/add-service-order-repair", async (ServiceOrderHandler handler, GetServiceOrderByIdCommand command) =>
        {
            try
            {
                command.Validate();

                var createOrderResult = await handler.AddServiceOrderRepair(command);
                if (!createOrderResult.IsSuccess)
                    return Results.BadRequest(new BaseResponse<ServiceOrder>(null, 404, createOrderResult.Message));

                return Results.Ok(createOrderResult);
            }
            catch (Exception ex)
            {
                var response = EndpointExceptions.Handle<ServiceOrder>(ex);
                return Results.BadRequest(response ?? new BaseResponse<ServiceOrder>(null, 500, $"Ocorreu um erro desconhecido: {ex.Message}"));
            }
        });

        app.MapPut("/add-service-order-delivery", async (ServiceOrderHandler handler, GetServiceOrderByIdCommand command) =>
        {
            try
            {
                command.Validate();

                var createOrderResult = await handler.AddServiceOrderDeliveryAndReturnPdfAsync(command);
                if (!createOrderResult.IsSuccess)
                    return Results.BadRequest(new BaseResponse<ServiceOrder>(null, 404, createOrderResult.Message));

                return Results.File(createOrderResult.Data!, "application/pdf", "ordem_servico.pdf");
            }
            catch (Exception ex)
            {
                var response = EndpointExceptions.Handle<ServiceOrder>(ex);
                return Results.BadRequest(response ?? new BaseResponse<ServiceOrder>(null, 500, $"Ocorreu um erro desconhecido: {ex.Message}"));
            }
        });

        app.MapPut("/regenerate-service-order-pdf", async (ServiceOrderHandler handler, GetServiceOrderByIdCommand command) =>
        {
            try
            {
                command.Validate();

                var createOrderResult = await handler.RegenerateAndReturnPdfAsync(command);
                if (!createOrderResult.IsSuccess)
                    return Results.BadRequest(new BaseResponse<ServiceOrder>(null, 404, createOrderResult.Message));

                return Results.File(createOrderResult.Data!, "application/pdf", "ordem_servico.pdf");
            }
            catch (Exception ex)
            {
                var response = EndpointExceptions.Handle<ServiceOrder>(ex);
                return Results.BadRequest(response ?? new BaseResponse<ServiceOrder>(null, 500, $"Ocorreu um erro desconhecido: {ex.Message}"));
            }
        });
    }
}