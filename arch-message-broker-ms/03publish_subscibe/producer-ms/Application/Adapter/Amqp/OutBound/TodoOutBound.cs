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
            using (var connection = _connFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "hello",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);


                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(todo));

                channel.BasicPublish(exchange: "",
                                     routingKey: "hello",
                                     basicProperties: null,
                                     body: body);
            }

            return _mapper.Map<TodoResponse>(todo);
        }
    }
}
