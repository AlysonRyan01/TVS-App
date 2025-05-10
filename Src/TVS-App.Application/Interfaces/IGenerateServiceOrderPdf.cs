using TVS_App.Application.DTOs;
using TVS_App.Domain.Entities;

namespace TVS_App.Application.Interfaces;

public interface IGenerateServiceOrderPdf
{
    Task<BaseResponse<string>> GenerateCheckInDocumentAsync(ServiceOrder serviceOrder);
    Task<BaseResponse<string>> GenerateCheckOutDocumentAsync(ServiceOrder serviceOrder);
    Task<BaseResponse<string>> RegeneratePdfAsync(ServiceOrder serviceOrder);
}