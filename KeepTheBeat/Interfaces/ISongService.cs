using Keep_The_Beat.Classes;

namespace KeepTheBeat.Interfaces
{
    public interface ISongService
    {
        Task<List<Song>> GetSongs();
        Task AddSong(Song song);
    }
}
