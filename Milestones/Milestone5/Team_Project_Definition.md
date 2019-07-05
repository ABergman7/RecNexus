2018-19 Team Project Inception: Pick-Up Sports Application
=====================================

## Summary of Our Approach to Software Development

With use of the discipline agile delivery methodology, we will deliver a website that allows users to find local pick-up games and share reviews/comments with their community regarding locations where games are played. 

## Initial Vision Discussion with Stakeholders

An application that allows users to find or host local sports pick-up games. If someone just wants to see what games are available in their area, they can login and enter the sport they wish to play and the website would provide a list of any nearby games happening so that user could join. If there are no games, users can start one! We also want to provide a way for users to be notified of games instead of only searching for one. To accomplish this, users can enter their preferred sports and times they are available tp play then when a game happens around that time, the application would notify the user. 

Users will also be able to see details on locations where pick up games are happpening or could happen. Such details could include times of operation, address, pictures, reviews, whether reservations are required, etc. In addition to seeing other reviews for locations, users will be given the option to leave a review. This application will give communities the opportunity to come together, get outside, and play. 

## List of Needs and Features 
1. A great looking landing page with info to tell the user what our site is all about and how to use it. Include a link to and a page with more info. Needs a page describing our company and our philosophy
2. The ability for any user (logged in or not) to see available and local sports pick-up games
3. User accounts
4. Users that are logged in should be able to set preferences for which sports they are interested and playing and at what times 
5. Users will be sent a notification if a pick-up game is happening during their preferred times
6. A user can choose to join a game if they are logged which will allow other users to see how many people are potentially signed up for a game
7. A user can choose to start a pick-up game at any given venue or time 
8. Information on venues such as availability, address, pictures, etc should be available to users 
9. Give users the ability to review venues and to also request that information displayed (such as address or times) be updated if incorrect

## Initial Modeling

### Use Case Diagrams

### Other Modeling

## Identify Non-Functional Requirements
1. User accounts and data must be stored indefinitely.
2. Site and data must be backed up regularly and have failover redundancy that will allow the site to remain functional in the event of loss of primary web server or primary database. We can live with 1 minute of complete downtime per event and up to 1 hour of read-only functionality before full capacity is restored.
3. Site should never return debug error pages. Web server must never return 404's. All server errors must be logged. Users should receive a custom error page in that case telling them what to do.
4. All sports venues' information pages must have a consistent format.
5. The website can run on any browser without messed up format.
6. The log in password should be encrypted for safety.
7. User has internet.
8. The layout should be self-explanatory enough so that any user needs only access the user manual to understand the functions of the product.

## Identify Functional Requirements (User Stories)

E: Epic  
F: Feature  
U: User Story  
T: Task  

1. [E] Create website with database access
    1. [F] Create website with homepage with database access
        1. [U] As a visitor/user to the site I would like to see a fantastic and modern homepage that tells me how to use the site so I can decide if I want to use this service in the future.
            1. [T] Create starter ASP dot NET MVC 5 Web Application 
            2. [T] Switch it over to Bootstrap 4
            3. [T] Create nice homepage: write content, customize navbar
            4. [T] Create SQL Server database on Azure and configure web app to use it. Hide credentials.
2. [E] Implement user login capability
    1. [F] Implement different login capabilities so user is not limited to one
        1. [U] As a user, I would like to be able to login just using my email and a password I create so that I don't have to have a Google account
            1. [T] Fully enable Individual User Accounts
            2. [T] Copy SQL schema from an existing ASP.NET Identity database and integrate it into our UP script
            3. [T] Configure web app to use our db with Identity tables in it
            4. [T] Create a user table and customize user pages to display additional data
            5. [T] Enable Captcha 
        2. [U] As a user, I would like to be able to login with Google so I don't have to create separate accounts to use the website
        3. [U]As a new logged-in user, I would like to be able to provide a username and other details such as my name and phone number so I do not just have my email address as my only contact information.
        4. [U] As a logged-in user, I would like to be able to modify my account details to ensure it's updated if my information changes
        5. [U] As a logged-in user, I would like the option to delete my account so I can be removed if I decide to stop using the service
3. [E] Create pages for venues 
    1. [F] Create pages where any user or visitor can view information on a venue
        1. [U] As a visitor to the site, I would like to be able to see information on a venue so I can determine whether or not it's a place I wish to play
4. [E] Allow users to browse/search venues
    1. [F] Allow all users to search sports venues by different criteria/filters
        1. [U] As a user, I would like to search all the sports venues by the distance I set by myself in the filter so I can see all the sports venues in my expected distance
        2. [U] As a user, I would like to search all venues by availability so I can see which places are open when I can be available
        3. [U] As a visitor, I would like to search venues by sports so I can choose a perfect place to play 
        4. [U] As a visitor, I would like to sort the sports venues by rating so I can find places that other players enjoyed a lot 
5. [E] Create section for users to post and read venue reviews
    1. [F] Allow users to read and post reviews on venues 
        1. [U] As a user, I would like to look up a venue and be able to leave a review so I can let others know about my experience playing there 
        2. [U] As a user, I would like to be able to read reviews for venues so I can see what others thought about the place 
6. [E] Allow users to set preferences on times and sports they wish to play
    1. [F] Give usuers ability to set preferences on what times they'd like to play and/or what sports they'd like to play
        1. [U] As a user, I would like to pick what sports I'm interested in playing so I can be notified about sports that I like instead of all games that I don't like 
        2. [U] As a user, I would like to pick what times I am available to play so I can receive notifications if a pick-up game is about to happen when I'm free.
7. [E] Allow users to start and modify pick-up games
    1. [F] Add ability for users to create a new pick-up game
        1. [U] As a logged in user, I would like to be able to start a new pick-up game so I can find other users who wish to play in my local area 
    2. [F] Add ability for users to modify or delete their events in limited time
        1. [U] As a logged in user, I would like to delete my event so I can remove it if there is a collision with my schedule
        2. [U] As a logged in user, I would like to be able to modify my event so I can make changes if there is a time or venue change 
    3. [F] Add ability for user notifications
        1. [U] As a logged in user, I want to receive a notification when other users join my events so that I know how many people will in my event
        2. [U] As a logged in user, I want to receive a notification when other users leave my events so that I know how many people will in my event
    4. [F] Add ability for users to invite others
        1. [U] As a logged in user, I want to invite my friends to join my sports event so that I can play with friends or others I was previously grouped with 
8. [E] Create page that display information for existing pick-up games
    1. [F] Create a page where user can see date/time, venue, and sport of existing pick-up games
        1. [U] As a user, I would like to be able to view details of a pick-up game such as the time, sport, and venue on one page so I can review the details before deciding if I wish to join the game
9. [E] Add ability for users to search existing pick-up games
    1. [F] Users should be able to search existing pick-up games by different criteria/filters
        1. [U] As a user, I would like to be able to search existing games by sport so I can only see results for sports I'm interested in playing
        2. [U] As a user, I would like to be able to search existing games by times so I won't see results for games during times that I am not available. 
        3. [U] As a user, I would liek to be able to search existing games by venue so I can pick my favorite spots to play at. 
10. [E] Add ability for users to join existing pick-up games
    1. [F] When user finds a game they like, they should have ability to join the game
        1. [U] As a user, I would like to be able to join a game once I find one that fits my preferences so I can let others know that I will be there to play


## Agile Data Modeling

### Textual Conceptional Diagram:

Contact (**ContactID**, Username, FirstName, LastName, Email, Phone, Address1, Address2, City, State, ZipCode)

Game (**GameID**, DayOfWeek, BeginTime, *VenueID*, *SportID*)

PickUpGame (**PickUpGameID**, *ContactID*, *GameID*)

Review (**ReviewID**, Rating, Review, *VenueID*, *ContactID*)

Sport (**SportID**, Name)

SportPreference (**SportPreferenceID**, *SportID*, *ContactID*)

TimePreference (**TimePreferenceID**, DayOfWeek, BeginTime, EndTime, *ContactID*)

Venue (**VenueID**, Name, Phone, Address1, Address2, City, State, ZipCode)

------

### Table defintions:

**CONTACT**: Individual that can either start or join a pick-up game. Must be a logged-in user. 

__Attributes for CONTACT table__:

Address1 & Address2:  The street address of contact. Necessary to find local games.

ContactID:  Auto number key field to uniquely identify a contact.

City:  The city where a contact is located.

Email:  The email address for a contact.

FirstName:  The first name of a contact.

LastName:  The last name of a contact.

Phone:  The phone number for a contact.

State:  The state where a contact is located.

Username: Unique username chosen by contact.

ZipCode:  The zip code for a contact.

------

**GAME**: A game which consists of a sport, time, and a venue

__Attributes for GAME table__:

BeginTime: Start of date range for game

DayOfWeek: Represents day of week game will occur (Ex: Monday, Tuesday, Wednesday)

GameID: Auto number key field to uniquely identify a game

------

**PICKUPGAME**: A pick up game which constists of a game and a contact

__Attributes for PICKUPGAME table__:

PickUpGameID: Auto number key field to uniquely identify a pick-up game

------

**REVIEW**: A review made by a contact regarding a venue

__Attributes for REVIEW table__:

Rating: Number value rating from 1-5 

Review: The review made by the contact regarding the venue 

ReviewID: Auto number key field to uniquely identify a review

------

**SPORT**: Lookup table for any sport that can be played 

__Attributes for SPORT table__:

Name: Name of sport. Example: “Basketball”

SportID: Auto number key field to uniquely identify a sport 

------

**SPORTPREFERENCE**: Sport preference for contact (sport they are interested in)

__Attributes for SPORTPREFERENCE table__:

SportPreferenceID: Auto number key field to uniquely identify a preference

------

**TIMEPREFERENCE**: Time preference for contact (time range they are available to play)

__Attributes for TIMEPREFERENCE table__:

BeginTime: Start of date range user is available to play a sport

DayOfWeek: Represents day of week user is available (Ex: Monday, Tuesday, Wednesday)

EndTime: End of date range user is available to play a sport 

TimePreferenceID: Auto number key field to uniquely identify a preference

------

**VENUE**: Lookup table for any venue that a sport can be played at

__Attributes for VENUE table__:

Address1 & Address2:  The street address of venue

City:  The city where a venue is located.

Name: Name of venue

Phone:  The phone number for a venue.

State:  The state where a venue is located.

VenueID: Auto number key field to uniquely identify a venue.

ZipCode:  The zip code for a venue.


## Identification of Risks
One major risk is not being able to keep information about venues current. We will likely use Google's Places API to get information but unfortunately it's not easy to find dates that venues are open to the public nor is there an easy way to make reservations at such places. Another roadblock we face is finding an effective way to store dates/times that users are available to play that doesn't require storing date ranges directly in the database.   

## Timeline and Release Plan