using TVS_App.Application.Exceptions;

namespace TVS_App.Application.Commands.CustomerCommands;

public class UpdateCustomerCommand : ICommand
{
    public long Id { get; set; }
    
    public string Name { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;
    public string Neighborhood { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public void Validate()
    {
        if (Id == 0)
            throw new CommandException<UpdateCustomerCommand>("O Id do UpdateCustomerCommand não pode ser 0");

        if (string.IsNullOrEmpty(Name))
            throw new CommandException<UpdateCustomerCommand>("O nome do UpdateCustomerCommand não pode estar vazio");

        if (string.IsNullOrEmpty(Phone))
            throw new CommandException<UpdateCustomerCommand>("O telefone do UpdateCustomerCommand não pode estar vazio");
    }
}