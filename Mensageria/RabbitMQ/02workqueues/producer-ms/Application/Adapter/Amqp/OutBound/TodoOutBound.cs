using Application.Adapter.Amqp.Interfaces;
using Application.Adapter.Contract.Request;
using Application.Adapter.Contract.Response;
using AutoMapper;
using Domain.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Application.Adapter.Amqp.OutBound
{
    public class TodoOutBound : ITodoOutBound
    {

        private readonly IMapper _mapper;
        private readonly ConnectionFactory _connFactory;


        public TodoOutBound(IMapper mapper)
        {
            _mapper = mapper;
            _connFactory = new ConnectionFactory() { HostName = "localhost" };
        }

        public async Task<TodoResponse> TodoSend(TodoRequest todoRequest)
        {
            var todo = _mapper.Map<Todo>(todoRequest);
            todo.SearchId = Guid.NewGuid();
            var connection = _connFactory.CreateConnection();


            var channel1 = CreateChannel(connection);
            var channel2 = CreateChannel(connection);
            var queueName = "task_queue";

            BuildPublishers(channel1, queueName, "Produtor A", todo);
            BuildPublishers(channel2, queueName, "Produtor B", todo);


            return _mapper.Map<TodoResponse>(todo);
        }

        private IModel CreateChannel(IConnection connection)
        {
            return connection.CreateModel();
        }

        private void BuildPublishers(IModel channel, string queueName, string productorName, Todo todo)
        {
            channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);


            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(todo));

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(exchange: "",
                                 routingKey: "task_queue",
                                 basicProperties: properties,
                                 body: body);

            Console.WriteLine($" {productorName} Received {0}", todo);

        }


    }
}
