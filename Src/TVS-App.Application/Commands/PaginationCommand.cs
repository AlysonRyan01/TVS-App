using TVS_App.Application.Exceptions;

namespace TVS_App.Application.Commands;

public class PaginationCommand : ICommand
{
    public int pageNumber { get; set; }
    public int pageSize { get; set; }

    public void Validate()
    {
        if (pageNumber < 1)
            throw new CommandException<PaginationCommand>("O pageNumber não pode ser menor que 1");

        if (pageSize < 1)
            throw new CommandException<PaginationCommand>("O pageSize não pode ser menor que 1");
    }
}