using ApacheKafkaWorker.Domain.Models;
using MediatR;

namespace ApacheKafkaWorker.Domain.Commands
{
    public class CreateNaturalPersonCommand : IRequest<string>
    {
        public CreateNaturalPersonCommand(string documentNumber, string name, string lastName, int age, decimal income, Address address)
        {
            Id = Guid.NewGuid().ToString();
            DocumentNumber = documentNumber;
            Name = name;
            LastName = lastName;
            Age = age;
            Income = income;
            Address = address;
        }

        public string Id { get; }
        public string DocumentNumber { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public decimal Income { get; set; }
        public Address Address { get; set; }
    }
}
