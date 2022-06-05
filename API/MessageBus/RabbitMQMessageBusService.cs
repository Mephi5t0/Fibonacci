using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using View.Fibonacci;

namespace API.MessageBus
{
    public sealed class RabbitMQMessageBusService : IMessageBusService
    {
        public void SendCalculationResult(FibonacciCalculationResult calculationResult)
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            const string queue = "fibonacci_calculations";
            channel.QueueDeclare(queue, false, false, false, null);

            var json = JsonConvert.SerializeObject(calculationResult);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish("", queue, body: body);
        }
    }
}