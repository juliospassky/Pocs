using Domain.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Application.Adapter.Inbound
{
    public class TodoInbound : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var connection = factory.CreateConnection();
            var channel1 = CreateChannel(connection);
            var channel2 = CreateChannel(connection);
            var queueName = "task_queue";

            BuildAndRunWorker(channel1, queueName, "Produtor A");
            BuildAndRunWorker(channel2, queueName, "Produtor B");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

        }

        private void BuildAndRunWorker(IModel channel, string queueName, string productoName)
        {
            channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageTodo = JsonConvert.DeserializeObject<Todo>(Encoding.UTF8.GetString(body));
                Console.WriteLine($" {productoName} Received {0}", messageTodo);
                int dots = messageTodo.Name.Split('.').Length - 1;

                Thread.Sleep(dots * 1000);

                channel.BasicAck(ea.DeliveryTag, false);
            };
            channel.BasicConsume(queue: "task_queue",
                                 autoAck: false,
                                 consumer: consumer);
        }

        private IModel CreateChannel(IConnection connection)
        {
            return connection.CreateModel();
        }
    }
}
