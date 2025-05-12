using AutomatizarOs.Api.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TVS_App.Api.Exceptions;
using TVS_App.Application.Commands;
using TVS_App.Application.Commands.CustomerCommands;
using TVS_App.Application.DTOs;
using TVS_App.Application.Handlers;
using TVS_App.Domain.Entities;

namespace TVS_App.Api.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        app.MapPost("/create-customer", async (CustomerHandler handler, CreateCustomerCommand command, IHubContext<ServiceOrderHub> hubContext) =>
        {
            try
            {
                command.Validate();

                var createResult = await handler.CreateCustomerAsync(command);
                if (!createResult.IsSuccess)
                    return Results.Problem(createResult.Message);

                await hubContext.Clients.All.SendAsync("Atualizar", createResult.Message);

                return Results.Ok(new BaseResponse<Customer>(createResult.Data, 200, createResult.Message));
            }
            catch (Exception ex)
            {
                var response = EndpointExceptions.Handle<Customer>(ex);
                return Results.BadRequest(response ?? new BaseResponse<Customer>(null, 500, $"Ocorreu um erro desconhecido: {ex.Message}"));
            }
        }).WithTags("Customer").RequireAuthorization();

        app.MapPut("/update-customer", async (CustomerHandler handler, UpdateCustomerCommand command, IHubContext<ServiceOrderHub> hubContext) =>
        {
            try
            {
                command.Validate();

                var createResult = await handler.UpdateCustomerAsync(command);
                if (!createResult.IsSuccess)
                    return Results.Problem(createResult.Message);

                await hubContext.Clients.All.SendAsync("Atualizar", createResult.Message);

                return Results.Ok(new BaseResponse<Customer>(createResult.Data, 200, createResult.Message));
            }
            catch (Exception ex)
            {
                var response = EndpointExceptions.Handle<Customer>(ex);
                return Results.BadRequest(response ?? new BaseResponse<Customer>(null, 500, $"Ocorreu um erro desconhecido: {ex.Message}"));
            }
        }).WithTags("Customer").RequireAuthorization();

        app.MapGet("/get-customer-by-id/{id}", async (CustomerHandler handler, [FromRoute] long id) =>
        {
            try
            {
                var command = new GetCustomerByIdCommand { Id = id };
                command.Validate();

                var createResult = await handler.GetCustomerByIdAsync(command);
                if (!createResult.IsSuccess)
                    return Results.Problem(createResult.Message);

                return Results.Ok(new BaseResponse<Customer>(createResult.Data, 200, createResult.Message));
            }
            catch (Exception ex)
            {
                var response = EndpointExceptions.Handle<Customer>(ex);
                return Results.BadRequest(response ?? new BaseResponse<Customer>(null, 500, $"Ocorreu um erro desconhecido: {ex.Message}"));
            }
        }).WithTags("Customer").RequireAuthorization();
        
        app.MapGet("/get-all-customers/{pageSize}/{pageNumber}", async (
            CustomerHandler handler,
            [FromRoute] int pageSize,
            [FromRoute] int pageNumber) =>
        {
            try
            {
                var command = new PaginationCommand{pageNumber = pageNumber,  pageSize = pageSize};
                command.Validate();

                var createResult = await handler.GetAllCustomersAsync(command);
                if (!createResult.IsSuccess)
                    return Results.Problem(createResult.Message);

                return Results.Ok(new BaseResponse<PaginatedResult<Customer?>>(createResult.Data, 200, createResult.Message));
            }
            catch (Exception ex)
            {
                var response = EndpointExceptions.Handle<Customer>(ex);
                return Results.BadRequest(response ?? new BaseResponse<Customer>(null, 500, $"Ocorreu um erro desconhecido: {ex.Message}"));
            }
        }).WithTags("Customer").RequireAuthorization();
    }
}