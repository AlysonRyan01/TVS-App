using TVS_App.Domain.Exceptions;

namespace TVS_App.Domain.ValueObjects.ServiceOrder;

public class LaborCost : ValueObject
{
    protected LaborCost() { }

    public LaborCost(decimal amount)
    {
        if (amount < 0)
            throw new ValueObjectException<PartCost>("O valor da mão de obra não pode ser menor que 0");

        ServiceOrderLaborCost = amount;
    }

    public decimal ServiceOrderLaborCost { get; private set; }

    public override string ToString() => ServiceOrderLaborCost.ToString("C");
}