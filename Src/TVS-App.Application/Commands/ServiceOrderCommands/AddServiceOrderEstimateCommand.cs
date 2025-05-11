using TVS_App.Application.Exceptions;
using TVS_App.Domain.Enums;

namespace TVS_App.Application.Commands.ServiceOrderCommands;

public class AddServiceOrderEstimateCommand : ICommand
{
    public long ServiceOrderId { get; set; }
    public string Guarantee { get; set; } = string.Empty;
    public string Solution { get; set; } = string.Empty;
    public decimal PartCost { get; set; }
    public decimal LaborCost { get; set; }
    public ERepairResult RepairResult { get; set; }

    public void Validate()
    {
        if (string.IsNullOrEmpty(Solution))
            throw new CommandException<AddServiceOrderEstimateCommand>("A solução do AddServiceOrderEstimateCommand não estar ser vazia");

        if (string.IsNullOrEmpty(Guarantee))
            throw new CommandException<AddServiceOrderEstimateCommand>("A garantia do AddServiceOrderEstimateCommand não estar ser vazia");

        if (PartCost < 0)
            throw new CommandException<AddServiceOrderEstimateCommand>("o valor da peça do AddServiceOrderEstimateCommand não pode ser menor que 0");

        if (LaborCost < 0)
            throw new CommandException<AddServiceOrderEstimateCommand>("o valor da mão de obra do AddServiceOrderEstimateCommand não pode ser menor que 0");

        if (!Enum.IsDefined(RepairResult))
            throw new CommandException<AddServiceOrderEstimateCommand>("o resultado do orçamento do AddServiceOrderEstimateCommand não pode ser nulo");
    }

    public void Normalize()
    {
        Solution = Solution.Trim().ToUpper();
        Guarantee = Guarantee.Trim().ToUpper();
    }
}