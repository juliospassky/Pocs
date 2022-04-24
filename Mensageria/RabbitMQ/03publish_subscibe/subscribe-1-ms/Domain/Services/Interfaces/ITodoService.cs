using Domain.Entities;
using MongoDB.Bson;

namespace Domain.Services.Interfaces
{
    public interface ITodoService
    {
        Task<List<Todo>> GetPages(int skipSize, int limitSize);

        Task<Todo> GetById(string id);

    }
}
