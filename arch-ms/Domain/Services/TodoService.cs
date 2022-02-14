using Domain.Entities;
using Infrastructure.Database.Config;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Domain.Services
{
    public class TodoService
    {
        private readonly IMongoCollection<Todo> _todoService;
        public TodoService(IOptions<MongoDbConfig> options)
        {
            var mongoDb = new MongoClient(options.Value.ConnectionString);
            _todoService = mongoDb.GetDatabase(options.Value.DatabaseName)
                .GetCollection<Todo>(options.Value.Todo);
        }

        public async Task<List<Todo>> GetAll() =>
            await _todoService.Find(_ => true).ToListAsync();

        public async Task<Todo> Get(string id) =>
            await _todoService.Find(o => o.Id == id).FirstOrDefaultAsync();

        public async Task Create(Todo newTodo) =>
            await _todoService.InsertOneAsync(newTodo);

        public async Task Update(string id, Todo updateTodo) =>
            await _todoService.ReplaceOneAsync(o => o.Id == id, updateTodo);

        public async Task Delete(string id) =>
            await _todoService.DeleteOneAsync(o => o.Id == id);
    }
}
