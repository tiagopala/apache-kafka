namespace ApacheKafkaWorker.Domain.Models
{
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
}
