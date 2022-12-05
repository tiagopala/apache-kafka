using Confluent.Kafka;
using System.Text;

namespace ApacheKafka.MessageBus.Extensions
{
    public static class HeaderExtensions
    {
        public static void AddHeader(this Headers headers, string key, string value)
        {
            if (!headers.Any(x => x.Key == key))
            {
                headers.Add(key, Encoding.UTF8.GetBytes(value));
            }
        }
    }
}
