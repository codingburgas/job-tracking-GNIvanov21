using System.Text.Json;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace JobTracking.DataAccess
{
    public class FileDataService : IDataService
    {
        private const string FilePath = "data.json";
        private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

        public DatabaseModel GetDatabase()
        {
            if (!File.Exists(FilePath))
            {
                // If the file doesn't exist, return a new empty database
                return new DatabaseModel();
            }

            var json = File.ReadAllText(FilePath);
            if (string.IsNullOrEmpty(json))
            {
                return new DatabaseModel();
            }

            return JsonSerializer.Deserialize<DatabaseModel>(json, Options) ?? new DatabaseModel();
        }

        public void SaveChanges(DatabaseModel database)
        {
            var json = JsonSerializer.Serialize(database, Options);
            File.WriteAllText(FilePath, json);
        }
    }
}