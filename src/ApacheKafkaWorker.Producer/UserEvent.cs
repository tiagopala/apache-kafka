namespace ApacheKafkaWorker.Producer;

internal class UserEvent
{
    public UserEvent(string documentNumber, string name, string lastName, int age, decimal income, Address address)
    {
        Id = Guid.NewGuid().ToString();
        DocumentNumber = documentNumber;
        Name = name;
        LastName = lastName;
        Age = age;
        Income = income;
        Address = address;
    }

    public string Id { get; set; }
    public string DocumentNumber { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
    public decimal Income { get; set; }
    public Address Address { get; set; }
}

public class Address
{
    public Address(string street, int number)
    {
        Street = street;
        Number = number;
    }

    public string Street { get; set; }
    public int Number { get; set; }
}
