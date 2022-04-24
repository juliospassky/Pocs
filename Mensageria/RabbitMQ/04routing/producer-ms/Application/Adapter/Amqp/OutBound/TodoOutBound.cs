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
            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(todo));

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_logs", type: "direct");

                var severity = new List<string>() { "error", "info", "warning" };
                var message = new List<string>() { "error - erro desconhecido ex", "info - registro atualizado", "warning - falha ao logar no sistema" };

                for (int i = 0; i < severity.Count; i++)
                {
                    channel.BasicPublish(exchange: "direct_logs",
                                     routingKey: severity[i],
                                     basicProperties: null,
                                     body: Encoding.UTF8.GetBytes(message[i]));

                    Console.WriteLine("direct_logs - [x] Sent '{0}':'{1}'", severity[i], message[i]);
                }

                channel.BasicPublish(exchange: "errror_logs",
                                  routingKey: severity[0],
                                  basicProperties: null,
                                  body: body);

                Console.WriteLine("errror_logs - [x] Sent '{0}':'{1}'", severity[0], message[0]);


            }

            return _mapper.Map<TodoResponse>(todo);
        }
    }
}
