using TVS_App.Domain.Exceptions;

namespace TVS_App.Domain.ValueObjects.Customer;

public class Name : ValueObject
{
    protected Name() {}

    public Name(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ValueObjectException<Name>("O nome do cliente não pode ser vazio");

        CustomerName = name;
    }

    public string CustomerName { get; private set; } = string.Empty;

    public override string ToString() => CustomerName;
}