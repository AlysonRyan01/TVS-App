using TVS_App.Domain.Enums;
using TVS_App.Domain.ValueObjects.Product;

namespace TVS_App.Domain.Entities;

public class Product : Entity
{
    public Product(Model model,
     SerialNumber serialNumber,
      Defect? defect,
      string? accessories,
      EProduct type)
    {
        Model = model;
        SerialNumber = serialNumber;
        Defect = defect;
        Accessories = accessories;
        Type = type;
    }

    public long ServiceOrderId { get; private set; }
    public ServiceOrder ServiceOrder { get; private set; } = null!;
    public Model Model { get; private set; }
    public SerialNumber SerialNumber { get; private set; }
    public Defect? Defect { get; private set; }
    public string? Accessories { get; private set; }
    public EProduct Type { get; private set; }

    public void UpdateProduct(string model, string serialNumber, string defect, string accessories, EProduct type)
    {
        Model = new Model(model);
        SerialNumber = new SerialNumber(serialNumber);
        Defect = new Defect(defect);
        Accessories = accessories;
        Type = type;
    }
}