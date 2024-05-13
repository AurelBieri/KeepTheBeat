﻿namespace Keep_The_Beat.Classes
{
    public class Song
    {
        public string _titel { get; set; }
        public string _artist { get; set; }
        public float _duration { get; set; }
        public string _album { get; set; }
        public bool _isfavorite { get; set; }
        public int _releaseyear { get; set; }

        public Song(string titel, string artist, float duration, string album, bool isfavorite, int releaseyear)
        {

            _titel = titel;
            _artist = artist;
            _duration = duration;
            _album = album;
            _isfavorite = isfavorite;
            _releaseyear = releaseyear;
        }
    }
}
