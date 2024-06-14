INSERT INTO User (firstname, lastname, username, email, password, birthday)
VALUES 
('John', 'Doe', 'johndoe', 'john.doe@example.com', 'hashedpassword1', '1990-01-01'),
('Jane', 'Smith', 'janesmith', 'jane.smith@example.com', 'hashedpassword2', '1992-05-15');

INSERT INTO Artist (FirstName, LastName)
VALUES 
('Ludwig', 'Beethoven'),
('Freddie', 'Mercury');


INSERT INTO Playlist (Name, SongCount, Duration, FK_Userid)
VALUES 
('Rock Classics', 10, 120.5, 1),
('Classical Favorites', 8, 95.2, 1);

