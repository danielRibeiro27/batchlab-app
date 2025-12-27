using System.Text.Json;
using BatchLabWorker.Domain;

namespace BatchLabWorker.Infrastructure
{
    public class JsonFileRepository 
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _jsonOptions;

        public JsonFileRepository(string filePath)
        {
            _filePath = filePath;
            _jsonOptions = new JsonSerializerOptions { WriteIndented = true };
            EnsureFileExists();
        }

        public async Task<JobEntity?> GetByIdAsync(string id)
        {
            var items = await ReadAllAsync();
            return items.Find(item => GetId(item) == id);
        }

        public async Task<JobEntity?> UpdateAsync(JobEntity entity)
        {
            var items = await ReadAllAsync();
            var index = items.FindIndex(item => GetId(item) == GetId(entity));
            if (index == -1)
            {
                return null; // Entity not found
            }

            items[index] = entity;
            await WriteAllAsync(items);
            return entity;
        }

        public async Task CreateAsync(JobEntity entity)
        {
            var items = await ReadAllAsync();
            items.Add(entity);
            await WriteAllAsync(items);
        }

        // TODO: Add thread-safety mechanisms (e.g., locking) to protect concurrent read/write operations
        // and prevent race conditions or data corruption in multi-threaded environments.
        private async Task<List<JobEntity>> ReadAllAsync()
        {
            var json = await File.ReadAllTextAsync(_filePath);
            return string.IsNullOrWhiteSpace(json) 
                ? new List<JobEntity>() 
                : JsonSerializer.Deserialize<List<JobEntity>>(json) ?? new List<JobEntity>();
        }

        private async Task WriteAllAsync(List<JobEntity> items)
        {
            var json = JsonSerializer.Serialize(items, _jsonOptions);
            await File.WriteAllTextAsync(_filePath, json);
        }

        private void EnsureFileExists()
        {
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "[]");
            }
        }

        // TODO: This method uses reflection to access the "Id" property, which is fragile and could cause runtime exceptions
        // if the type T doesn't have an "Id" property or if it has a different casing.
        // Consider using a generic constraint or interface to enforce the presence of an Id property.
        private static string GetId(JobEntity item)
        {
            var idProperty = typeof(JobEntity).GetProperty("Id");
            return idProperty?.GetValue(item)?.ToString() ?? string.Empty;
        }
    }
}