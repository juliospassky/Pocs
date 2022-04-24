using Application.Adapter.Contract.Request;
using Application.Adapter.Contract.Response;
using Domain.Entities;

namespace Application.Adapter.Amqp.Interfaces
{
    public interface ITodoOutBound
    {
        Task<TodoResponse> TodoSend(TodoRequest todoRequest);
    }
}
