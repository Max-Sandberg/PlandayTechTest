CREATE TABLE Employees (
   Id INTEGER PRIMARY KEY AUTOINCREMENT,
   Name text NOT NULL
);

CREATE TABLE Shifts (
	Id INTEGER PRIMARY KEY AUTOINCREMENT,
	EmployeeId INTEGER,
	Start TEXT NOT NULL, --SQLite doesn't support DateTime types
	End TEXT NOT NULL, --SQLite doesn't support DateTime types
	FOREIGN KEY(EmployeeId) REFERENCES Employees(Id)
);

INSERT INTO Employees (Name) VALUES ('John Doe');
INSERT INTO Employees (Name) VALUES ('Jane Doe');

INSERT INTO Shifts (EmployeeId, Start, End)
VALUES (1, '2024-10-20 12:00:00', '2024-10-20 17:00:00');
INSERT INTO Shifts (EmployeeId, Start, End)
VALUES (2, '2024-10-20 09:00:00', '2024-10-20 15:00:00');