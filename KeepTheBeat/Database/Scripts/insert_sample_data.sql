INSERT INTO User (firstname, lastname, username, email, password) VALUES ('John', 'Doe', 'johndoe', 'john.doe@example.com', 'password123');
INSERT INTO Artist (FirstName, LastName) VALUES ('John', 'Lennon');
INSERT INTO Song (Title, Duration, Album, IsFavorite, ReleaseYear) VALUES ('Imagine', 3.1, 'Imagine', 1, 1971);
INSERT INTO Playlist (Name, SongCount, Duration, FK_Userid) VALUES ('Favorites', 1, 3.1, 1);
INSERT INTO PlaylistSong (PlaylistId, SongId) VALUES (1, 1);
INSERT INTO ArtistSong (ArtistId, SongId) VALUES (1, 1);
