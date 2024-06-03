using Microsoft.Data.Sqlite;
using Keep_The_Beat.Classes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KeepTheBeat.Database.Services
{
    public class SongService
    {
        private readonly string _connectionString;

        public SongService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddSong(Song song)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText =
                    @"
            INSERT INTO Song (Title, Duration, Album, IsFavorite, ReleaseYear, FileNamen, FileContent)
            VALUES ($Title, $Duration, $Album, $IsFavorite, $ReleaseYear, $FileNamen, $FileContent);
            ";

                    command.Parameters.AddWithValue("$Title", song._titel ?? (object)DBNull.Value); 
                    command.Parameters.AddWithValue("$Duration", song._duration.HasValue ? (object)song._duration.Value : DBNull.Value);
                    command.Parameters.AddWithValue("$Album", song._album ?? (object)DBNull.Value); 
                    command.Parameters.AddWithValue("$IsFavorite", song._isfavorite ? 1 : 0);
                    command.Parameters.AddWithValue("$ReleaseYear", song._releaseyear.HasValue ? (object)song._releaseyear.Value : DBNull.Value);
                    command.Parameters.AddWithValue("$FileNamen", song.FileName ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("$FileContent", song.FileContent ?? (object)DBNull.Value); 

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<Song>> GetSongs()
        {
            var songs = new List<Song>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                SELECT SongId, Title, Duration, Album, IsFavorite, ReleaseYear, FileNamen, FileContent
                FROM Song;
                ";

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var song = new Song(
                            reader.GetString(1),
                            "Unknown Artist",
                            reader.IsDBNull(2) ? (float?)null : reader.GetFloat(2),
                            reader.IsDBNull(3) ? null : reader.GetString(3),
                            reader.GetInt32(4) == 1,
                            reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                            reader.GetString(6),
                            (byte[])reader[7]
                        );

                        songs.Add(song);
                    }
                }
            }

            return songs;
        }
    }
}
