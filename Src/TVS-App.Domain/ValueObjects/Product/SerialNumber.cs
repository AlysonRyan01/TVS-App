using TVS_App.Domain.Exceptions;

namespace TVS_App.Domain.ValueObjects.Product;

public class SerialNumber : ValueObject
{
    protected SerialNumber() { }

    public SerialNumber(string serialNumber)
    {
        if (string.IsNullOrEmpty(serialNumber))
            throw new ValueObjectException<Model>("O número de série do produto não pode estar vazio");

        ProductSerialNumber = serialNumber;
    }

    public string ProductSerialNumber { get; private set; } = string.Empty;

    public override string ToString() => ProductSerialNumber;
}