using TVS_App.Domain.Exceptions;

namespace TVS_App.Domain.ValueObjects.Customer;

public class Phone : ValueObject
{
    protected Phone() { }

    public Phone(string phone)
    {
        if (string.IsNullOrEmpty(phone))
            throw new ValueObjectException<Phone>("O telefone do cliente não estar ser vazio");

        CustomerPhone = phone;
    }

    public string CustomerPhone { get; private set; } = string.Empty;

    public override string ToString() => CustomerPhone;
}