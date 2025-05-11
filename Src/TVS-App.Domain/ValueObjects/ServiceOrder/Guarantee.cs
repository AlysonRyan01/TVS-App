using TVS_App.Domain.Exceptions;

namespace TVS_App.Domain.ValueObjects.ServiceOrder;

public class Guarantee : ValueObject
{
    protected Guarantee() {}

    public Guarantee(string guarantee)
    {
        if (string.IsNullOrEmpty(guarantee))
            throw new ValueObjectException<Guarantee>("A garantia não pode estar vazia");
    }

    public string ServiceOrderGuarantee { get; private set; } = string.Empty;
}