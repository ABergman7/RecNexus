CREATE TABLE [dbo].[Contact] (
    [ContactId]            INT IDENTITY(1,1) NOT NULL,
	[Username]             NVARCHAR (128)  NULL,
	[FirstName]            NVARCHAR (128)  NULL,
	[LastName]             NVARCHAR (128)  NULL,
    [Email]                NVARCHAR (256)  NOT NULL,
    [PhoneNumber]          NVARCHAR (128)  NULL,
    [Address1]             NVARCHAR (256)  NULL,
	[Address2]             NVARCHAR (256)  NULL,
	[City]				   NVARCHAR (256)  NULL,
	[State]                NVARCHAR (256)  NULL,
	[ZipCode]              NVARCHAR (256)  NULL,
	[HasPublicProfile]	   BIT			   NULL	
    CONSTRAINT [PK_dbo.Contacts] PRIMARY KEY CLUSTERED ([ContactId] ASC)
);

CREATE TABLE [dbo].[TimePreference](
    [TimePrefID]    INT IDENTITY(1,1) NOT NULL,
    [DayOfWeek]     INT,
    [BeginTime]     TIME,
    [EndTime]       TIME,
    [ContactID]		INT	               NOT NULL,

    CONSTRAINT [PK_dbo.TimePreference] PRIMARY KEY CLUSTERED ([TimePrefID] ASC),
    CONSTRAINT [FK_dbo.Contact] FOREIGN KEY (ContactID) REFERENCES [dbo].[Contact] (ContactId) 

);

CREATE TABLE Venue
(
	VenueID int IDENTITY(1,1) NOT NULL,
	Name nvarchar(max) NOT NULL,
	Phone nvarchar(50) NULL,
	Address1 nvarchar(50) NULL,
	Address2 nvarchar(50) NULL,
	City nvarchar(50) NULL,
	State nvarchar(50) NULL,
	ZipCode nvarchar(50) NULL,
	DateUpdated datetime null,
	GooglePlaceID nvarchar(50) NULL,

	CONSTRAINT PK_Venue PRIMARY KEY (VenueID),
);

CREATE TABLE BusinessHours
(
	BusinessHoursID int IDENTITY(1,1) NOT NULL,
	DayOfWeek int NOT NULL,
	OpenTime time(7) NOT NULL,
	CloseTime time(7) NOT NULL,
	VenueID int NOT NULL,

	CONSTRAINT PK_BusinessHours PRIMARY KEY (BusinessHoursID),
	CONSTRAINT FK_BusinessHours_Venue FOREIGN KEY (VenueID) REFERENCES Venue(VenueID)
);

CREATE TABLE Review
(
	ReviewID int IDENTITY(1,1) NOT NULL,
	Timestamp datetime NOT NULL,
	Rating int NOT NULL,
	Comments nvarchar(max) null,
	IsGoogleReview bit NOT NULL,
	GoogleAuthor nvarchar(50) null,
	ContactID int NULL,
	VenueID int NOT NULL,

	CONSTRAINT PK_Review PRIMARY KEY (ReviewID),
	CONSTRAINT FK_Review_Contact FOREIGN KEY (ContactID) REFERENCES Contact(ContactID),
	CONSTRAINT FK_Review_Venue FOREIGN KEY (VenueID) REFERENCES Venue(VenueID)
);

CREATE TABLE [dbo].[Location]
(
	LocationID int IDENTITY(1,1) NOT NULL,
	Latitude nvarchar(50) NULL,
	Longitude nvarchar(50) NULL,
	VenueID int NOT NULL,
	CONSTRAINT PK_Location PRIMARY KEY (LocationID),
	CONSTRAINT FK_Location_Venue FOREIGN KEY (VenueID) REFERENCES Venue(VenueID)

);

CREATE TABLE [dbo].[Sport](
    [SportID]    INT IDENTITY(1,1) NOT NULL,
    [SportName]     NVARCHAR (256) NOT NULL,
    CONSTRAINT [PK_dbo.Sport] PRIMARY KEY CLUSTERED ([SportID] ASC),
);

INSERT INTO [dbo].[Sport] (SportName) VALUES
('Football'),
('Basketball'),
('Tennis'),
('Soccer'),
('Baseball'),
('Softball'),
('Golf'),
('Volleyball'),
('Fencing'),
('Disc Golf'),
('Hiking'),
('Ultimate Frisbee'),
('Cricket'),
('Field Hockey'),
('Rugby');
GO 

CREATE TABLE [dbo].[SportPreference](
    [SportPrefID]    INT IDENTITY(1,1) NOT NULL,
	[SportID]		INT		NOT NULL,
    [ContactID]		INT	               NOT NULL,

    CONSTRAINT [PK_dbo.SportPreference] PRIMARY KEY CLUSTERED ([SportPrefID] ASC),
    CONSTRAINT [FK1_dbo.Contact] FOREIGN KEY (ContactID) REFERENCES [dbo].[Contact] (ContactId),
	CONSTRAINT [FK2_dbo.Sport] FOREIGN KEY (SportID) REFERENCES [dbo].[Sport] (SportId)
);

CREATE TABLE GameStatus
(
	GameStatusID int IDENTITY(1,1) NOT NULL,
	GameStatus nvarchar(50) NOT NULL,

	CONSTRAINT PK_GameStatus PRIMARY KEY (GameStatusID)
);

INSERT INTO GameStatus (GameStatus) VALUES
('Open'),
('Cancelled'),
('Acccepted'),
('Rejected')
GO

CREATE TABLE Game
(
	GameID int IDENTITY(1,1) NOT NULL,
	StartTime datetime NOT NULL,
	EndTime datetime NOT NULL,
	VenueID int NOT NULL,
	SportID int NOT NULL,
	ContactID int NULL,
	GameStatusID int NOT NULL,

	CONSTRAINT PK_Game PRIMARY KEY (GameID),
	CONSTRAINT FK_Game_Venue FOREIGN KEY (VenueID) REFERENCES Venue(VenueID),
	CONSTRAINT FK_Game_Sport FOREIGN KEY (SportID) REFERENCES Sport(SportID),
	CONSTRAINT FK_Game_Contact FOREIGN KEY (ContactID) REFERENCES Contact(ContactID),
	CONSTRAINT FK_Game_GameStatus FOREIGN KEY (GameStatusID) REFERENCES GameStatus(GameStatusID)
);

CREATE TABLE PickUpGame
(
	PickUpGameID int IDENTITY(1,1) NOT NULL,
	GameID int NOT NULL,
	ContactID int NULL,

	CONSTRAINT PK_PickUpGame PRIMARY KEY (PickUpGameID),
	CONSTRAINT FK_PickUpGame_Game FOREIGN KEY (GameID) REFERENCES Game(GameID),
	CONSTRAINT FK_PickUpGame_Contact FOREIGN KEY (ContactID) REFERENCES Contact(ContactID)
);

CREATE TABLE Friend
(
	FriendID int IDENTITY(1,1) NOT NULL,
	ContactID int NOT NULL,
	FriendContactID int NOT NULL,

	CONSTRAINT PK_Friend PRIMARY KEY (FriendID),
	CONSTRAINT FK_Friend_Contact FOREIGN KEY (ContactID) REFERENCES Contact(ContactID),
	CONSTRAINT FK_Friend_FriendContact FOREIGN KEY (FriendContactID) REFERENCES Contact(ContactID)

);

CREATE TABLE MessageConnection
(
 ConnectionID int IDENTITY(1,1) NOT NULL,
 ContactID int NOT NULL,
 MessageContactID int NOT NULL,
 Connected bool 

 CONSTRAINT PK_Friend PRIMARY KEY (FriendID),
	CONSTRAINT FK_Friend_Contact FOREIGN KEY (ContactID) REFERENCES Contact(ContactID),
	CONSTRAINT FK_Friend_FriendContact FOREIGN KEY (FriendContactID) REFERENCES Contact(ContactID)
);


CREATE TABLE VenueOwner
(
	VenueOwnerID int IDENTITY(1,1) NOT NULL,
	FirstName nvarchar(50) NULL,
	LastName nvarchar(50) NULL,
	Email nvarchar(50) NULL,
	Phone nvarchar(50) NULL,
	CompanyName nvarchar(50) NULL,
	SignUpDate datetime NOT NULL,
	VenueID int NOT NULL,

	CONSTRAINT PK_VenueOwner PRIMARY KEY (VenueOwnerID),
	CONSTRAINT FK_VenueOwner_Venue FOREIGN KEY (VenueID) REFERENCES Venue(VenueID)
);