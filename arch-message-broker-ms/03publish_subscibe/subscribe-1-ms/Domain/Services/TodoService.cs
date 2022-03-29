using Domain.Entities;
using Domain.Services.Interfaces;
using Infrastructure.Database.Config;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Domain.Services
{
    public class TodoService : ITodoService
    {
        private readonly IMongoCollection<Todo> _mongo;
        private readonly ConnectionFactory _factory;
        public TodoService(IOptions<MongoDbConfig> options)
        {
            var mongoDb = new MongoClient(options.Value.ConnectionString);
            _mongo = mongoDb.GetDatabase(options.Value.DatabaseName)
                .GetCollection<Todo>(options.Value.Todo);

            _factory = new ConnectionFactory() { HostName = "localhost" };
        }

        public async Task<List<Todo>> GetPages(int skipSize, int limitSize)
        {
            return await _mongo
                .Find(o => true)
                .Skip(skipSize)
                .Limit(limitSize)
                .ToListAsync();
        }

        public async Task<Todo> GetById(string id) =>
            await _mongo.Find(o => o.Id == id).FirstOrDefaultAsync();

    }
}
