CREATE SCHEMA CW2;

GO

CREATE TABLE [CW2].[Users]( 
[Email] [varchar](320) NOT NULL PRIMARY KEY,
[Username] [varchar](100) NOT NULL,
[Password] [varchar](30) NOT NULL,
[About_Me] [varchar](500) DEFAULT NULL,
[Location] [varchar](160) DEFAULT NULL,
[Birthday] [date] DEFAULT NULL,
[Height_cm] DECIMAL(5,2) DEFAULT NULL,
[Weight_kg] DECIMAL(5,2) DEFAULT NULL,
[Pref_Units_Is_Metric] [BIT] DEFAULT 1 NOT NULL,
[Activ_Time_Pref_Is_Speed] [BIT] DEFAULT 1 NOT NULL,
[Marketing_Language] [varchar](30) NOT NULL,
[Is_Archived] [BIT] DEFAULT 0 NOT NULL,
[Is_Admin] [BIT] DEFAULT 0 NOT NULL,
[Last_Updated] [smalldatetime]
);

GO 

CREATE TABLE [CW2].[Activities](
[ActivityID] [INT] IDENTITY NOT NULL PRIMARY KEY,
[Activity_Name] [varchar] (30) DEFAULT NULL
);

CREATE TABLE [CW2].[User-Activities](
[UserActivitiesID] [int] IDENTITY NOT NULL PRIMARY KEY,
[Email] [varchar](320),
[ActivityID] [INT],
CONSTRAINT fk_Email
FOREIGN KEY (Email)
REFERENCES [CW2].[Users](Email)
ON DELETE CASCADE,
CONSTRAINT fk_ActivityID
FOREIGN KEY (ActivityID)
REFERENCES [CW2].[Activities](ActivityID)
ON DELETE CASCADE
);

GO

CREATE TYPE CW2.TempActivityList AS TABLE
(
    ActivityID INT
);
GO


INSERT INTO [CW2].[Users] 

(Email,Username,[Password],About_Me,[Location],Birthday,Height_cm,[Weight_kg],Pref_Units_Is_Metric,Activ_Time_Pref_Is_Speed,Marketing_Language,Is_Archived,Last_Updated) VALUES 

('grace@plymouth.ac.uk','Grace Hopper','ISAD123!','About Me','Plymouth, Devon, England','1990-01-01',150.00,55.50,1,1,'English (UK)',0, GETDATE()),
('tim@plymouth.ac.uk','Tim Berners-Lee','COMP2001!',null,null,null,null,null,1,0,'English (US)',0, GETDATE()),
('ada@plymouth.ac.uk','Ada Lovelace','insecurePassword','My name is ada','Plymouth, Devon, England','1995-11-23',180.80,70.00,0,0,'Dansk (Danmark)',0, GETDATE()),
('adamsmith@gmail.com','Adam Smith','passw0rd','Im me','London, County of London, England','2001-07-17',160.23,6.090,1,1,'English (UK)',1, GETDATE()),
('bethbarron@yahoo.com','Beth Barron','123456789','Im me beth','Plymouth, Devon, England','2010-05-26',225.89,650.67,0,0,'Deutsch (Deutschland)',1, GETDATE());

GO 

INSERT INTO [CW2].[Activities] 

(Activity_Name) VALUES

('Backpacking'),
('Bike touring'),
('Bird watching'),
('Camping'),
('Cross-country skiing'),
('Fishing'),
('Hiking'),
('Horseback riding'),
('Mountain biking'),
('OHV/Off-road driving'),
('Paddle sports'),
('Road biking'),
('Rock climbing'),
('Running'),
('Scenic driving'),
('Skiing'),
('Snowshoeing'),
('Via ferrata'),
('Walking');

GO

INSERT INTO [CW2].[User-Activities]

(Email,ActivityID) VALUES
('grace@plymouth.ac.uk', 1), -- Grace Hopper - Backpacking
('tim@plymouth.ac.uk', 2), -- Tim Berners-Lee - Bike touring
('adamsmith@gmail.com', 4), -- Adam Smith - Camping
('adamsmith@gmail.com', 5), -- Adam Smith - Cross-country skiing
('grace@plymouth.ac.uk',6); -- Grace Hopper - Fishing
GO
