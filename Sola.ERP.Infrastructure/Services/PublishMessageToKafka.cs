using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Services
{
    public class PublishMessageToKafka
    {
        private readonly IProducer<int, string> _kafkaProducer;
        public PublishMessageToKafka(IProducer<int, string> kafkaProducer)
        {
            _kafkaProducer = kafkaProducer;
        }
        public async Task Publish(string topic)
        {
            try
            {
                var newMessage = new Message<int, string>
                {
                    Key = 1,
                    Value = DateTime.Now.ToString(),
                    Headers = null
                };
                var result = await _kafkaProducer.ProduceAsync(topic, newMessage);
                if (result.Status == PersistenceStatus.Persisted)
                {
                    Console.WriteLine($"Message {newMessage.Value} published to Kafka Topic: {topic}");
                }

            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occured creating topic {e.Message}");
            }

        }
    }
}
