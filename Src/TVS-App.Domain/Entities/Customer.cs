using TVS_App.Domain.Exceptions;
using TVS_App.Domain.ValueObjects.Customer;

namespace TVS_App.Domain.Entities;

public class Customer : Entity
{
    private readonly List<ServiceOrder> _serviceOrders = new();

    protected Customer() { }

    public Customer(Name name, Address address, Phone phone, Email? email)
    {
        Name = name;
        Address = address;
        Phone = phone;
        Email = email;
    }

    public Name Name { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public Phone Phone { get; private set; } = null!;
    public Email? Email { get; private set; }

    public IReadOnlyCollection<ServiceOrder> ServiceOrders => _serviceOrders.AsReadOnly();

    public void AddServiceOrder(ServiceOrder serviceOrder)
    {
        if (serviceOrder != null && serviceOrder.Id != 0)
            _serviceOrders.Add(serviceOrder);
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new EntityException<Customer>("Não podemos atualizar o nome pois está vazio");

        Name = new Name(name);
    }

    public void UpdateAdress(string street, string neighborhood, string city, string number, string zipCode, string state)
    {
        Address = new Address(street, neighborhood, city, number, zipCode, state);
    }

    public void UpdatePhone(string number)
    {
        if (string.IsNullOrEmpty(number))
            throw new EntityException<Customer>("O número de telefone não pode estar vazio");

        Phone = new Phone(number);
    }

    public void UpdateEmail(string email)
    {
        Email = new Email(email);
    }
}