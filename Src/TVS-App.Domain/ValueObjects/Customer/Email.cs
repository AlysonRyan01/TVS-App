using TVS_App.Domain.Exceptions;

namespace TVS_App.Domain.ValueObjects.Customer;

public class Email : ValueObject
{
    protected Email() { }

    public Email(string email)
    {
        CustomerEmail = email;
    }

    public string CustomerEmail { get; private set; } = string.Empty;

    public override string ToString() => CustomerEmail;
}