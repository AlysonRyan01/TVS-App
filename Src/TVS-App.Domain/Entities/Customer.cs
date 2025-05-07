using TVS_App.Domain.ValueObjects.Customer;

namespace TVS_App.Domain.Entities;

public class Customer : Entity
{
    protected Customer() { }

    public Customer(Address address)
    {
        Address = address;
    }

    public Address Address { get; private set; } = null!;
}