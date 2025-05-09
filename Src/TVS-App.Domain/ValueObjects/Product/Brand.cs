using TVS_App.Domain.Exceptions;

namespace TVS_App.Domain.ValueObjects.Product;

public class Brand : ValueObject
{
    protected Brand() { }

    public Brand(string brand)
    {
        if (string.IsNullOrEmpty(brand))
            throw new ValueObjectException<Model>("A marca do produto não pode estar vazia");

        ProductBrand = brand;
    }

    public string ProductBrand { get; private set; } = string.Empty;

    public override string ToString() => ProductBrand;
}