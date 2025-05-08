using System.Text.Json.Serialization;

namespace TVS_App.Application.DTOs;

public class BaseResponse <T>
{
    [JsonConstructor]
    public BaseResponse()
    {
        _code = 200;
    }
    
    public BaseResponse(T? data, int code = 200, string? message = null)
    {
        Data = data;
        Message = message;
        _code = code;
    }

    private readonly int _code;
    
    public T? Data { get; set; }
    public string? Message { get; set; }
    
    [JsonIgnore]
    public bool IsSuccess 
        => _code >= 200 && _code <= 299;
}