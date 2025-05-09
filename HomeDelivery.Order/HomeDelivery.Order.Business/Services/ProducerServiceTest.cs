using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace HomeDelivery.Order.Business.Services
{
   public class ProducerOrderService: IDisposable
   {
      private readonly IProducer<Null, string> _producer;

      public ProducerOrderService(IConfiguration configuration)
      {
         var producerconfig = new ProducerConfig
         {
            BootstrapServers = configuration["Kafka:BootstrapServers"] ?? throw new ArgumentException("Kafka BootstrapServers must be provided.")
         };

         _producer = new ProducerBuilder<Null, string>(producerconfig).Build();
      }
   
      public async Task ProduceAsync(string topic, string message)
      {
         try
         {
            var kafkaMessage = new Message<Null, string> { Value = message };
            await _producer.ProduceAsync(topic, kafkaMessage);
         }
         catch (ProduceException<Null, string> ex)
         {
            
            Console.WriteLine($"Error producing message to topic {topic}: {ex.Message}");
            throw; 
         }
      }
       
      public void Dispose()
      {
         _producer?.Dispose();
      }
   }
}