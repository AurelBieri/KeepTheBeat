﻿using Microsoft.Data.Sqlite;
using Keep_The_Beat.Classes;
using KeepTheBeat.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;
using BCrypt;

namespace KeepTheBeat.Database.Services
{
    public class UserService : IUserService
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
                INSERT INTO User (firstname, lastname, username, email, password, birthday)
                VALUES ($firstname, $lastname, $username, $email, $password, $birthday);
            ";
                command.Parameters.AddWithValue("$firstname", user._firstname);
                command.Parameters.AddWithValue("$lastname", user._lastname);
                command.Parameters.AddWithValue("$username", user._username);
                command.Parameters.AddWithValue("$email", user._email);
                command.Parameters.AddWithValue("$password", BCrypt.Net.BCrypt.HashPassword(user._password));
                command.Parameters.AddWithValue("$birthday", user._birthday.ToString("yyyy-MM-dd"));

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<User>> GetUser()
        {
            var users = new List<User>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                SELECT firstname, lastname, username, email, password, birthday
                FROM User;
            ";

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var user = new User(reader.GetString(2), reader.GetString(4), reader.GetString(3), reader.GetString(0), reader.GetString(1), DateTime.Parse(reader.GetString(5)));

                        users.Add(user);
                    }
                }
            }

            return users;
        }

        public async Task<User> Login(string username, string password)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT Userid, username, password FROM User WHERE username = $username;";
                command.Parameters.AddWithValue("$username", username);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var storedHash = reader.GetString(2);
                        if (BCrypt.Net.BCrypt.Verify(password, storedHash))
                        {
                            return new User(reader.GetInt32(0), reader.GetString(1), "secure", "secure");
                            
                        }
                    }
                }
            }
            return null;
        }

        public async Task<bool> IsEmailTaken(string email)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(1) FROM User WHERE email = $email;";
                command.Parameters.AddWithValue("$email", email);

                var result = (long)await command.ExecuteScalarAsync();
                return result > 0;
            }
        }

        public async Task<bool> IsUsernameTaken(string username)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(1) FROM User WHERE username = $username;";
                command.Parameters.AddWithValue("$username", username);

                var result = (long)await command.ExecuteScalarAsync();
                return result > 0;
            }
        }



    }
}
