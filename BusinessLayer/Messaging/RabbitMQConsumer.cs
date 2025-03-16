using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class RabbitMQConsumer
{
    private const string HostName = "localhost";
    private const string ExchangeName = "AddressBookExchange";
    private const string QueueName = "EmailQueue";

    public void StartConsuming()
    {
        var factory = new ConnectionFactory() { HostName = HostName };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.ExchangeDeclare(ExchangeName, ExchangeType.Topic, durable: true);
        channel.QueueDeclare(QueueName, durable: true, exclusive: false, autoDelete: false);
        channel.QueueBind(QueueName, ExchangeName, "user.registered");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var userEvent = JsonSerializer.Deserialize<UserRegisteredEvent>(message);

            Console.WriteLine($"Sending email to {userEvent.Email}");

            // Call Email Service (Placeholder)
            Task.Run(() => SendEmail(userEvent.Email, "Welcome to Address Book!"));
        };

        channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
        Console.WriteLine("Listening for messages...");
    }

    private void SendEmail(string email, string message)
    {
        // Simulate email sending
        Console.WriteLine($"Email sent to {email}: {message}");
    }
}
