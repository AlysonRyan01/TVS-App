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
    }

    public long CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;
    public Product Product { get; private set; } = null!;
    public EEnterprise Enterprise { get; private set; }
    public DateTime EntryDate { get; private set; }
    public DateTime? InspectionDate { get; private set; }
    public DateTime? RepairDate { get; private set; }
    public DateTime? DeliveryDate { get; private set; }
    public Solution? Solution { get; private set; }
    public PartCost PartCost { get; private set; } = new(0);
    public LaborCost LaborCost { get; private set; } = new(0);
    public decimal TotalAmount => PartCost.ServiceOrderPartCost + LaborCost.ServiceOrderLaborCost;
    public EServiceOrderStatus ServiceOrderStatus { get; private set; }
    public ERepairStatus RepairStatus { get; private set; }
    public ERepairResult? RepairResult { get; set; }

    public void AddEstimate(string solution, decimal partCost, decimal laborCost, ERepairResult repairResult)
    {
        Solution = new Solution(solution);
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
    }

    public void RejectEstimate()
    {
        if (string.IsNullOrEmpty(Solution?.ServiceOrderSolution))
            throw new EntityException<ServiceOrder>("A solução não pode estar nula ao adicionar a rejeição");

        RepairStatus = ERepairStatus.Disapproved;
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
}