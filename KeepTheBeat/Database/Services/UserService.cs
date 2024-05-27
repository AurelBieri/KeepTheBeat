using Microsoft.Data.Sqlite;
using Keep_The_Beat.Classes;
using System;
using System.IO;
using System.Threading.Tasks;

namespace KeepTheBeat.Database.Services
{
    public class UserService
    {
        private readonly string _connectionString;

        public UserService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddUser(User user)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                INSERT INTO User (firstname, lastname, username, email, password)
                VALUES ($firstname, $lastname, $username, $email, $password);
            ";
                command.Parameters.AddWithValue("$firstname", user._firstname);
                command.Parameters.AddWithValue("$lastname", user._lastname);
                command.Parameters.AddWithValue("$username", user._username);
                command.Parameters.AddWithValue("$email", user._email);
                command.Parameters.AddWithValue("$password", user._password);

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
