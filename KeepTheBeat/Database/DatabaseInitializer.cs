using Microsoft.Data.Sqlite;
using System.IO;

public class DatabaseInitializer
{
    private static DatabaseInitializer _instance;
    private static readonly object _lock = new object();
    private readonly string _connectionString;

    private DatabaseInitializer(string connectionString)
    {
        _connectionString = connectionString;
    }

    public static DatabaseInitializer GetInstance(string connectionString)
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new DatabaseInitializer(connectionString);
                }
            }
        }
        return _instance;
    }

    public void InitializeDatabase()
    {
        using (var connection = new SqliteConnection(_connectionString))
        {
            connection.Open();

            // Define the path to the scripts directory
            var scriptDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Scripts");

            // Create the database if it does not exist
            var createScriptPath = Path.Combine(scriptDirectory, "create_database.sql");
            if (File.Exists(createScriptPath))
            {
                var createScript = File.ReadAllText(createScriptPath);
                using (var command = new SqliteCommand(createScript, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            // Update the database if an update script exists
            var updateScriptPath = Path.Combine(scriptDirectory, "update_database.sql");
            if (File.Exists(updateScriptPath))
            {
                var updateScript = File.ReadAllText(updateScriptPath);
                using (var command = new SqliteCommand(updateScript, connection))
                {
                    command.ExecuteNonQuery();
                }
            }

            // Insert sample data if the script exists
            var insertScriptPath = Path.Combine(scriptDirectory, "insert_sample_data.sql");
            if (File.Exists(insertScriptPath))
            {
                var insertScript = File.ReadAllText(insertScriptPath);
                using (var command = new SqliteCommand(insertScript, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
