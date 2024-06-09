using Microsoft.Data.Sqlite;
using Keep_The_Beat.Classes;
using KeepTheBeat.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KeepTheBeat.Database.Services
{
    public class PlaylistService : IPlaylistService
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

                        var playlist = new Playlist(songs, name, songCount, duration, user)
                        {
                            PlaylistId = playlistId // Set the PlaylistId
                        };
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
                SELECT s.SongId, s.Title, s.Duration, s.Album, s.IsFavorite, s.ReleaseYear, s.ArtistName, s.FileNamen, s.FileContent
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
                            _titel = reader.IsDBNull(1) ? null : reader.GetString(1),
                            _duration = reader.IsDBNull(2) ? (float?)null : reader.GetFloat(2),
                            _album = reader.IsDBNull(3) ? null : reader.GetString(3),
                            _isfavorite = reader.GetBoolean(4),
                            _releaseyear = reader.IsDBNull(5) ? (int?)null : reader.GetInt32(5),
                            _artist = reader.IsDBNull(6) ? null : reader.GetString(6),
                            FileName = reader.IsDBNull(7) ? null : reader.GetString(7),
                            FileContent = reader.IsDBNull(8) ? null : (byte[])reader[8]
                        };

                        songs.Add(song);
                    }
                }
            }

            return songs;
        }

        public async Task<Playlist> GetPlaylistById(User user, int playlistId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = @"
                SELECT p.PlaylistId, p.Name, p.SongCount, p.Duration
                FROM Playlist p
                WHERE p.PlaylistId = $playlistId AND p.FK_Userid = $userId;
            ";
                command.Parameters.AddWithValue("$playlistId", playlistId);
                command.Parameters.AddWithValue("$userId", user.UserId);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var playlist = new Playlist(reader.GetInt32(0), await GetSongsForPlaylist(playlistId), reader.GetString(1), reader.GetInt32(2), reader.GetFloat(3), user);
                        return playlist;
                    }
                }
            }
            return null;
        }

        public async Task AddSongToPlaylist(int playlistId, Song song)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = @"
            INSERT INTO PlaylistSong (PlaylistId, SongId)
            VALUES ($playlistId, $songId);
        ";
                command.Parameters.AddWithValue("$playlistId", playlistId);
                command.Parameters.AddWithValue("$songId", song._songid);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task RemoveSongFromPlaylist(int playlistId, int songId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = @"
                DELETE FROM PlaylistSong
                WHERE PlaylistId = $playlistId AND SongId = $songId;
                ";
                command.Parameters.AddWithValue("$playlistId", playlistId);
                command.Parameters.AddWithValue("$songId", songId);

                await command.ExecuteNonQueryAsync();
            }
        }

    }
}
