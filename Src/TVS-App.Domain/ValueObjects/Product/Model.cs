using TVS_App.Domain.Exceptions;

namespace TVS_App.Domain.ValueObjects.Product;

public class Model : ValueObject
{
    protected Model() { }
    
    public Model(string model)
    {
        if (string.IsNullOrEmpty(model))
            throw new ValueObjectException<Model>("O modelo do produto não pode estar vazio");

        ProductModel = model;
    }

    public string ProductModel { get; private set; } = string.Empty;

    public override string ToString() => ProductModel;
}