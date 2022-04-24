using Domain.Entities;

namespace Domain.Services.Interfaces
{
    public interface ITodoService
    {
        Task<List<Todo>> GetPages(int skipSize, int limitSize);

        Task<Todo> GetById(string id);

        Task Create(Todo newTodo);

        Task Update(string id, Todo updateTodo);

        Task Delete(string id);
    }
}
