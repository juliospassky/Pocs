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
                channel.ExchangeDeclare(exchange: "topic_logs", type: "topic");

                var severity = new List<string>() { "log.auth.info", "log.auth.warning", "log.auth.error", "log.kernel.info", "log.kernel.warning", "log.kernel.error", "log.*.error" };
                var message = new List<string>() { "autherror - erro desconhecido ex", "authinfo - registro atualizado", "authwarning - falha ao logar no sistema",
                                                    "kernelerror - erro desconhecido ex", "kernelinfo - registro atualizado", "kernelwarning - falha ao logar no sistema", "error - erro desconhecido" };

                for (int i = 0; i < severity.Count; i++)
                {
                    channel.BasicPublish(exchange: "topic_logs",
                                     routingKey: severity[i],
                                     basicProperties: null,
                                     body: Encoding.UTF8.GetBytes(message[i]));

                    Console.WriteLine("topic_logs - [x] Sent '{0}':'{1}'", severity[i], message[i]);
                }

                Console.WriteLine("errror_logs - [x] Sent '{0}':'{1}'", severity[0], message[0]);

            }

            return _mapper.Map<TodoResponse>(todo);
        }
    }
}
