using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Web.Infrastructure.Messaging
{
    public class RabbitMqMessageBus : IMessageBus
    {
        private readonly ConnectionFactory _factory;

        public RabbitMqMessageBus()
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost", 
                Port = 5672,           
                UserName = "guest",     
                Password = "guest"  
            };
        }

        public async Task PublishAsync(string queue, object message)
        {
            try
            {
                using var connection = _factory.CreateConnection();
                using var channel = connection.CreateModel();       

                channel.QueueDeclare(queue: queue,
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var jsonMessage = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(jsonMessage);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                                     routingKey: queue,
                                     basicProperties: properties,
                                     body: body);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitMQ Publish Error: {ex.Message}");
            }
        }
    }


}
