namespace TVS_App.Api.Requests.AuthRequests;

public class LoginRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}