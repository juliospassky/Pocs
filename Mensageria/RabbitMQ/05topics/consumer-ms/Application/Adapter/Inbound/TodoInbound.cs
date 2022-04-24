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
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: "topic_logs", type: "topic");

            var severity = new List<string>() { "log.auth.info", "log.auth.warning", "log.auth.error", "log.kernel.info", "log.kernel.warning", "log.kernel.error", "log.*.error" };
            var queueName = new List<string>() { "x.topic_logs.q.auth.info", "x.topic_logs.q.auth.warning", "x.topic_logs.q.auth.error",
                                                  "x.topic_logs.q.kernel.info", "x.topic_logs.q.kernel.warning", "x.topic_logs.q.kernel.error", "x.topic_logs.q.error" };

            for (int i = 0; i < severity.Count; i++)
            {
                channel.QueueDeclare(queue: queueName[i],
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);


                channel.QueueBind(queue: queueName[i],
                                  exchange: "topic_logs",
                                  routingKey: severity[i]);
            }

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                Console.WriteLine(" [x] Received '{0}':'{1}'", routingKey, message);
            };

            for (int i = 0; i < severity.Count; i++)
            {

                channel.BasicConsume(queue: queueName[i],
                                     autoAck: true,
                                     consumer: consumer);
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }

        }
    }
}
