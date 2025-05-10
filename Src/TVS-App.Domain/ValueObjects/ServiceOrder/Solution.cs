using TVS_App.Domain.Exceptions;

namespace TVS_App.Domain.ValueObjects.ServiceOrder;

public class Solution : ValueObject
{
    protected Solution() { }

    public Solution(string solution)
    {
        ServiceOrderSolution = solution;
    }

    public string ServiceOrderSolution { get; private set; } = string.Empty;
}