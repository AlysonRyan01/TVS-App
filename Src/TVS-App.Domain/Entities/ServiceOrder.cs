using TVS_App.Domain.Enums;
using TVS_App.Domain.Exceptions;
using TVS_App.Domain.ValueObjects.ServiceOrder;

namespace TVS_App.Domain.Entities;

public class ServiceOrder : Entity
{
    protected ServiceOrder() { }

    public ServiceOrder(EEnterprise enterprise, long customerId, Product product)
    {
        EntryDate = DateTime.UtcNow;
        Enterprise = enterprise;
        CustomerId = customerId;
        ServiceOrderStatus = EServiceOrderStatus.Entered;
        RepairStatus = ERepairStatus.Entered;
        Product = product;
        SecurityCode = GenerateRandomCode();
    }

    public string SecurityCode { get; private set; } = null!;
    public long CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;
    public Product Product { get; private set; } = null!;
    public EEnterprise Enterprise { get; private set; }
    public DateTime EntryDate { get; private set; }
    public DateTime? InspectionDate { get; private set; }
    public DateTime? ResponseDate { get; set; }
    public DateTime? RepairDate { get; private set; }
    public DateTime? PurchasePartDate { get; private set; }
    public DateTime? DeliveryDate { get; private set; }
    public Solution? Solution { get; private set; }
    public Guarantee? Guarantee { get; private set; }
    public PartCost PartCost { get; private set; } = new(0);
    public LaborCost LaborCost { get; private set; } = new(0);
    public decimal TotalAmount => PartCost.ServiceOrderPartCost + LaborCost.ServiceOrderLaborCost;
    public EServiceOrderStatus ServiceOrderStatus { get; private set; }
    public ERepairStatus RepairStatus { get; private set; }
    public ERepairResult? RepairResult { get; set; }

    public void AddEstimate(string solution, string guarantee, decimal partCost, decimal laborCost, ERepairResult repairResult)
    {
        Solution = new Solution(solution);
        Guarantee = new Guarantee(guarantee);
        PartCost = new PartCost(partCost);
        LaborCost = new LaborCost(laborCost);
        ServiceOrderStatus = EServiceOrderStatus.Evaluated;
        RepairStatus = ERepairStatus.Waiting;
        RepairResult = repairResult;
        InspectionDate = DateTime.UtcNow;
    }

    public void ApproveEstimate()
    {
        if (string.IsNullOrEmpty(Solution?.ServiceOrderSolution))
            throw new EntityException<ServiceOrder>("A solução não pode estar nula ao adicionar a aprovação");

        RepairStatus = ERepairStatus.Approved;
        ResponseDate = DateTime.UtcNow;
    }

    public void RejectEstimate()
    {
        if (string.IsNullOrEmpty(Solution?.ServiceOrderSolution))
            throw new EntityException<ServiceOrder>("A solução não pode estar nula ao adicionar a rejeição");

        RepairStatus = ERepairStatus.Disapproved;
        ResponseDate = DateTime.UtcNow;
    }

    public void AddPurchasedPart()
    {
        ServiceOrderStatus = EServiceOrderStatus.OrderPart;
        PurchasePartDate = DateTime.UtcNow;
    }

    public void ExecuteRepair()
    {
        if (RepairStatus == ERepairStatus.Entered || RepairStatus == ERepairStatus.Waiting)
            throw new EntityException<ServiceOrder>("Não podemos executar o conserto pois a ordem de serviço não tem o status de aprovado");

        if (RepairStatus == ERepairStatus.Waiting)
            throw new EntityException<ServiceOrder>("Não podemos executar o conserto pois a estamos aguardando a resposta do cliente");


        ServiceOrderStatus = EServiceOrderStatus.Repaired;
        RepairDate = DateTime.UtcNow;
    }

    public void AddDelivery()
    {
        ServiceOrderStatus = EServiceOrderStatus.Delivered;
        DeliveryDate = DateTime.UtcNow;
    }

    public void UpdateServiceOrder(
        Customer customer,
        string productBrand,
        string productModel,
        string productSerialNumber,
        string productDefect,
        string accessories,
        EProduct productType,
        EEnterprise enterprise)
    {
        CustomerId = customer.Id;
        Customer = customer;
        Product.UpdateProduct(productBrand, productModel,
            productSerialNumber, productDefect, accessories, productType);
        Enterprise = enterprise;
    }

    private string GenerateRandomCode()
    {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        const string digits = "0123456789";

        Random random = new Random();

        char letter1 = letters[random.Next(letters.Length)];
        char digit1 = digits[random.Next(digits.Length)];
        char letter2 = letters[random.Next(letters.Length)];
        char digit2 = digits[random.Next(digits.Length)];

        return $"{letter1}{digit1}{letter2}{digit2}".ToUpper();
    }
}