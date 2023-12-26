using MassTransit;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Mango.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MessageBus(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishMessage(object message, string topic_queue_Name)
        {
            await _publishEndpoint.Publish(message);
            ////await _publishEndpoint.Publish(message);
            //var jsonMessage = JsonConvert.SerializeObject(message);
            //var factory = new ConnectionFactory { HostName = "localhost" };
            //using var connection = factory.CreateConnection();
            //using var channel = connection.CreateModel();

            //var body = Encoding.UTF8.GetBytes(jsonMessage);
            //channel.BasicPublish(exchange: string.Empty,
            //                     routingKey: topic_queue_Name,
            //                     basicProperties: null,
            //                     body: body);
        }
    }
}
