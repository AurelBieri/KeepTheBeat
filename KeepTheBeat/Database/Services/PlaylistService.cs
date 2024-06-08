using Microsoft.Data.Sqlite;
using Keep_The_Beat.Classes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KeepTheBeat.Database.Services
{
    public class PlaylistService
    {
        private readonly string _connectionString;

        public PlaylistService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task AddPlaylist(Playlist playlist)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    var command = connection.CreateCommand();
                    command.CommandText =
                    @"
                    INSERT INTO Playlist (Name, SongCount, Duration, FK_Userid)
                    VALUES ($name, $songCount, $duration, $userId);
                ";
                    command.Parameters.AddWithValue("$name", playlist._name);
                    command.Parameters.AddWithValue("$songCount", playlist._songCount);
                    command.Parameters.AddWithValue("$duration", playlist._totalduration);
                    command.Parameters.AddWithValue("$userId", playlist._owner.UserId);

                    await command.ExecuteNonQueryAsync();

                    var playlistIdCommand = connection.CreateCommand();
                    playlistIdCommand.CommandText = "SELECT last_insert_rowid();";
                    var playlistId = (long)await playlistIdCommand.ExecuteScalarAsync();

                    foreach (var song in playlist._songs)
                    {
                        var songCommand = connection.CreateCommand();
                        songCommand.CommandText =
                        @"
                        INSERT INTO PlaylistSong (PlaylistId, SongId)
                        VALUES ($playlistId, $songId);
                    ";
                        songCommand.Parameters.AddWithValue("$playlistId", playlistId);
                        songCommand.Parameters.AddWithValue("$songId", song._songid);

                        await songCommand.ExecuteNonQueryAsync();
                    }

                    await transaction.CommitAsync();
                }
            }
        }

        public async Task<List<Playlist>> GetPlaylists(User user)
        {
            var playlists = new List<Playlist>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                SELECT PlaylistId, Name, SongCount, Duration
                FROM Playlist
                WHERE FK_Userid = $userId;
            ";
                command.Parameters.AddWithValue("$userId", user.UserId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var playlistId = reader.GetInt32(0);
                        var name = reader.GetString(1);
                        var songCount = reader.GetInt32(2);
                        var duration = reader.GetFloat(3);

                        var songs = await GetSongsForPlaylist(playlistId);

                        var playlist = new Playlist(songs, name, songCount, duration, user);
                        playlists.Add(playlist);
                    }
                }
            }

            return playlists;
        }

        private async Task<List<Song>> GetSongsForPlaylist(long playlistId)
        {
            var songs = new List<Song>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                SELECT s.SongId, s.Title, s.Duration, s.Album, s.IsFavorite, s.ReleaseYear
                FROM Song s
                INNER JOIN PlaylistSong ps ON s.SongId = ps.SongId
                WHERE ps.PlaylistId = $playlistId;
            ";
                command.Parameters.AddWithValue("$playlistId", playlistId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var song = new Song
                        {
                            _songid = reader.GetInt32(0),
                            _titel = reader.GetString(1),
                            _duration = reader.GetFloat(2),
                            _album = reader.GetString(3),
                            _isfavorite = reader.GetBoolean(4),
                            _releaseyear = reader.GetInt32(5)
                        };

                        songs.Add(song);
                    }
                }
            }

            return songs;
        }
    }
}
