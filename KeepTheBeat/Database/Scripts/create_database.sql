﻿CREATE TABLE IF NOT EXISTS User (
    Userid INTEGER PRIMARY KEY AUTOINCREMENT,
    firstname TEXT NOT NULL,
    lastname TEXT NOT NULL,
    username TEXT NOT NULL UNIQUE,
    email TEXT NOT NULL UNIQUE,
    password TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Artist (
    ArtistId INTEGER PRIMARY KEY AUTOINCREMENT,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Song (
    SongId INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Duration REAL,  
    Album TEXT,
    IsFavorite INTEGER NOT NULL, 
    ReleaseYear INTEGER
);

CREATE TABLE IF NOT EXISTS ArtistSong (
    ArtistId INTEGER NOT NULL,
    SongId INTEGER NOT NULL,
    FOREIGN KEY (ArtistId) REFERENCES Artist(ArtistId),
    FOREIGN KEY (SongId) REFERENCES Song(SongId),
    PRIMARY KEY (ArtistId, SongId)
);

CREATE TABLE IF NOT EXISTS Playlist (
    PlaylistId INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    SongCount INTEGER NOT NULL,
    Duration REAL NOT NULL,  
    FK_Userid INTEGER NOT NULL,
    FOREIGN KEY (FK_Userid) REFERENCES User(Userid)   
);

CREATE TABLE IF NOT EXISTS PlaylistSong (
    PlaylistId INTEGER NOT NULL,
    SongId INTEGER NOT NULL,
    FOREIGN KEY (PlaylistId) REFERENCES Playlist(PlaylistId),
    FOREIGN KEY (SongId) REFERENCES Song(SongId),
    PRIMARY KEY (PlaylistId, SongId)
);
