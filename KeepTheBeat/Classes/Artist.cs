using System;

namespace Keep_The_Beat.Classes
{
    public class Artist : Person
    {
        public string[] _songs { get; set; }

        public Artist(string[] songs, string firstname, string lastname) : base(firstname, lastname)
        {
            _songs = songs;
        }
    }
}
