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
    public Defect? Defect { get; set; }
    public string? Accessories { get; private set; }
    public EProduct Type { get; set; }
}