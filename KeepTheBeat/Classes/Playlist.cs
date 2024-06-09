namespace Keep_The_Beat.Classes
{
    public class Playlist
    {
        public int PlaylistId { get; set; }
        public List<Song> _songs { get; set; }
        public string _name { get; set; }
        public int _songCount { get; set; }
        public float _totalduration { get; set; }
        public User _owner { get; set; }

        public Playlist(List<Song> songs, string name, int songCount, float duration, User owner)
        {
            _songs = songs;
            _name = name;
            _songCount = songCount;
            _totalduration = duration;
            _owner = owner;
        }

        public Playlist(int Id,List<Song> songs, string name, int songCount, float duration, User owner)
        {
            _songs = songs;
            _name = name;
            _songCount = songCount;
            _totalduration = duration;
            _owner = owner;
            PlaylistId = Id;
        }


        public Playlist(string name)
        {
            _songs = new List<Song>();
            _name = name;
        }
    }
}
