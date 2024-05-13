namespace Keep_The_Beat.Classes
{
    public class Playlist
    {
        public string[] _songs { get; set; }
        public string _name { get; set; }
        public int _songCount { get; set; }
        public float _duration { get; set; }
        public string _owner { get; set; }

        public Playlist(string[] songs, string name, int songCount, float duration, string owner)
        {
            _songs = songs;
            _name = name;
            _songCount = songCount;
            _duration = duration;
            _owner = owner;
        }
        //public string AddSong(){

        //}
        //public string DeleteSong() { 

        //}
    }
}
