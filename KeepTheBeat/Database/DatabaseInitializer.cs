using Microsoft.Data.Sqlite;
using System;
using System.IO;

public class DatabaseInitializer
{
    private static DatabaseInitializer _instance;
    private static readonly object _lock = new object();
    private readonly string _connectionString;
    private bool IsUpdateavailabel = false;

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
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var scriptDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Database", "Scripts");

                ExecuteScript(connection, scriptDirectory, "create_database.sql");

                if (IsUpdateavailabel)
                {
                    ExecuteScript(connection, scriptDirectory, "update_database.sql");
                }

                //Wir haben Song Sample Daten schon manuell hinzugefügt.
                //Das Skript für sample data fügt zwar sample data ein aber fügt keine .mp3 song Dateien ein.
                //ExecuteScript(connection, scriptDirectory, "insert_sample_data.sql");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing database: {ex.Message}");
            throw;
        }
    }

    private void ExecuteScript(SqliteConnection connection, string scriptDirectory, string scriptFileName)
    {
        var scriptPath = Path.Combine(scriptDirectory, scriptFileName);
        if (File.Exists(scriptPath))
        {
            var script = File.ReadAllText(scriptPath);
            Console.WriteLine($"Executing script: {scriptFileName}");
            using (var command = new SqliteCommand(script, connection))
            {
                command.ExecuteNonQuery();
            }
        }
        else
        {
            Console.WriteLine($"Script file not found: {scriptFileName}");
        }
    }
}