namespace TVS_App.Domain.ValueObjects.Customer;

public class Address : ValueObject
{
    protected Address() { }

    public Address(string street,
        string neighborhood,
        string city,
        string number,
        string zipCode,
        string state)
    {
        Street = street;
        Neighborhood = neighborhood;
        City = city;
        Number = number;
        ZipCode = zipCode;
        State = state;
    }

    public string Street { get; private set; } = string.Empty;
    public string Neighborhood { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string Number { get; private set; } = string.Empty;
    public string ZipCode { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
}