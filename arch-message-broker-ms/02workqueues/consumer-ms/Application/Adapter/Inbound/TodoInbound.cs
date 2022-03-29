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
            var channel = CreateChannel(connection);
            var queueName = "task_queue";

            channel.BasicQos(0,2,false);

            BuildAndRunWorker(channel, queueName, "Consumer A");
            BuildAndRunWorker(channel, queueName, "Consumer B");
            BuildAndRunWorker(channel, queueName, "Consumer C");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

        }

        private void BuildAndRunWorker(IModel channel, string queueName, string workerName)
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
                Console.WriteLine($" {workerName} Received {0}", messageTodo);
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
