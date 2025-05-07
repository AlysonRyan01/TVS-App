using TVS_App.Domain.Exceptions;

namespace TVS_App.Domain.ValueObjects.Product;

public class Defect : ValueObject
{
    protected Defect() { }

    public Defect(string defect)
    {
        if (string.IsNullOrEmpty(defect))
            throw new ValueObjectException<Model>("O defeito do produto não pode estar vazio");

        ProductDefect = defect;
    }

    public string ProductDefect { get; private set; } = string.Empty;

    public override string ToString() => ProductDefect;
}