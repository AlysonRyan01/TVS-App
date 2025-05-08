using TVS_App.Domain.Exceptions;

namespace TVS_App.Domain.ValueObjects.ServiceOrder;

public class PartCost : ValueObject
{
    protected PartCost() { }

    public PartCost(decimal amount)
    {
        if (amount < 0)
            throw new ValueObjectException<PartCost>("O valor da peça não pode ser menor que 0");

        ServiceOrderPartCost = amount;
    }

    public decimal ServiceOrderPartCost { get; private set; }

    public override string ToString() => ServiceOrderPartCost.ToString("C");
}