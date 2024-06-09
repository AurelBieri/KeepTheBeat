using Keep_The_Beat.Classes;

namespace KeepTheBeat.Interfaces
{
    public interface IPlaylistService
    {
        Task AddPlaylist(Playlist playlist);
        Task<List<Playlist>> GetPlaylists(User user);
        Task<Playlist> GetPlaylistById(User user, int playlistId);
        Task AddSongToPlaylist(int playlistId, Song song);
        Task RemoveSongFromPlaylist(int playlistId, int songId);
    }
}
