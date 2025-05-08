using TVS_App.Domain.Exceptions;

namespace TVS_App.Domain.ValueObjects.ServiceOrder;

public class Solution : ValueObject
{
    protected Solution() { }

    public Solution(string solution)
    {
        if (string.IsNullOrEmpty(solution))
            throw new ValueObjectException<Solution>("A solução não pode estar vazia");

        ServiceOrderSolution = solution;
    }

    public string ServiceOrderSolution { get; private set; } = string.Empty;
}