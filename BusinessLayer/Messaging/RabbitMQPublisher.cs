using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace BusinessLayer.Messaging
{
    public class RabbitMQPublisher
    {
        private const string HostName = "localhost";
        private const string ExchangeName = "AddressBookExchange";

        public void Publish<T>(T message, string routingKey)
        {
            var factory = new ConnectionFactory() { HostName = HostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(ExchangeName, ExchangeType.Topic, durable: true);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            channel.BasicPublish(exchange: ExchangeName, routingKey: routingKey, body: body);
        }
    }
}

