using TodoApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace TodoApi.Services;

public class TodoService
{
    private readonly IMongoCollection<Todo> _tasksCollection;

    public TodoService(IOptions<TodoListDatabaseSettings> todoListDatabaseSettings)
    {
        // Console.WriteLine(Environment.GetEnvironmentVariable("DB_URI") == todoListDatabaseSettings.Value.ConnectionString);
        // Console.WriteLine(Environment.GetEnvironmentVariable("DATABASE_NAME") == todoListDatabaseSettings.Value.DatabaseName);
        // Console.WriteLine(Environment.GetEnvironmentVariable("TASKS_COLLECTION_NAME") == todoListDatabaseSettings.Value.TasksCollectionName);

        // Console.WriteLine($"{Environment.GetEnvironmentVariable("DB_URI")}, {todoListDatabaseSettings.Value.ConnectionString}");
        // Console.WriteLine($"{Environment.GetEnvironmentVariable("DATABASE_NAME")}, {todoListDatabaseSettings.Value.DatabaseName}");
        // Console.WriteLine($"{Environment.GetEnvironmentVariable("TASKS_COLLECTION_NAME")}, {todoListDatabaseSettings.Value.TasksCollectionName}");
       
        var mongoClient = new MongoClient(Environment.GetEnvironmentVariable("DB_URI"));

        var mongoDatabase = mongoClient.GetDatabase(Environment.GetEnvironmentVariable("DATABASE_NAME"));

        _tasksCollection = mongoDatabase.GetCollection<Todo>(Environment.GetEnvironmentVariable("TASKS_COLLECTION_NAME"));

        // var mongoClient = new MongoClient(todoListDatabaseSettings.Value.ConnectionString);

        // var mongoDatabase = mongoClient.GetDatabase(todoListDatabaseSettings.Value.DatabaseName);

        // _tasksCollection = mongoDatabase.GetCollection<Todo>(todoListDatabaseSettings.Value.TasksCollectionName);
    }

    public async Task<List<Todo>> GetAsync() => await _tasksCollection.Find(_ => true).ToListAsync();

    public async Task<Todo?> GetAsync(string id) => await _tasksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Todo newTodo) => await _tasksCollection.InsertOneAsync(newTodo);

    public async Task UpdateAsync(string id, Todo updatedTodo) => await _tasksCollection.ReplaceOneAsync(x => x.Id == id, updatedTodo);

    public async Task RemoveAsync(string id) => await _tasksCollection.DeleteOneAsync(x => x.Id == id);
}