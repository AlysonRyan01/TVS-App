using TVS_App.Application.Exceptions;

namespace TVS_App.Application.Commands.CustomerCommands;

public class CreateCustomerCommand : ICommand
{
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
        if (string.IsNullOrEmpty(Name))
            throw new CommandException<CreateCustomerCommand>("O nome do CreateCustomerCommand não pode estar vazio");

        if (string.IsNullOrEmpty(Phone))
            throw new CommandException<CreateCustomerCommand>("O telefone do CreateCustomerCommand não pode estar vazio");
    }
}
