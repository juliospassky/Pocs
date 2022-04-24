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

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "logs", type: ExchangeType.Fanout);

                var message = todo.Name;
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "logs",
                                     routingKey: "",
                                     basicProperties: null,
                                     body: body);

                Console.WriteLine(" [x] Sent {0}", message);
            }

            return _mapper.Map<TodoResponse>(todo);
        }
    }
}
