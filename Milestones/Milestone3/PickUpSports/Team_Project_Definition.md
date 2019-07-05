2018-19 Team Project Inception: Pick-Up Sports Application
=====================================

## Summary of Our Approach to Software Development

With use of the discipline agile delivery methodology, we will deliver a website that allows users to find local pick-up games and share reviews/comments with their community regarding locations where games are played. 

## Initial Vision Discussion with Stakeholders

An application that allows users to find or host local sports pick-up games. If someone just wants to see what games are available in their area, they can login and enter the sport they wish to play and the website would provide a list of any nearby games happening so that user could join. If there are no games, users can start one! We also want to provide a way for users to be notified of games instead of only searching for one. To accomplish this, users can enter their preferred sports and times they are available tp play then when a game happens around that time, the application would notify the user. 

Users will also be able to see details on locations where pick up games are happpening or could happen. Such details could include times of operation, address, pictures, reviews, whether reservations are required, etc. In addition to seeing other reviews for locations, users will be given the option to leave a review. This application will give communities the opportunity to come together, get outside, and play. 

## Initial Modeling

### Use Case Diagrams

### Other Modeling

## Identify Functional Requirements (User Stories)

E: Epic  
F: Feature  
U: User Story  
T: Task  

1. [E] Create website
    1. [F] Create website with homepage, database access, and login functionality
        1. [U] As a visitor to the site I would like to see a fantastic and modern homepage that tells me how to use the site so I can decide if I want to use this service in the future.
            1. [T] Create starter ASP dot NET MVC 5 Web Application with Individual User Accounts and no unit test project
            2. [T] Switch it over to Bootstrap 4
            3. [T] Create nice homepage: write content, customize navbar
            4. [T] Create SQL Server database on Azure and configure web app to use it. Hide credentials.
        2. [U] Fully enable Individual User Accounts
            1. [T] Copy SQL schema from an existing ASP.NET Identity database and integrate it into our UP script
            2. [T] Configure web app to use our db with Identity tables in it
            3. [T] Create a user table and customize user pages to display additional data
        3. [U] As a robot I would like to be prevented from creating an account on your website so I don't ask millions of my friends to join your website and occupied all sports venues
2. [E] Allow users to search the sports venues match the filters
    1. [F] Allow all users to search the sports venues by distance
        1. [U] As a visitor to the site, I would like to search all the sports venues by the distance I set by myself in the filter so I can see all the sports venues in my expected distance
            1. [T] Create a filter to allow user set the distance before they start searching
            2. [T] Add a reminder to user if they didn't input the distance
            3. [T] Add a reminder to user if there is no sports venues in their expect distance
            4. [T] Display a map to show all the sports venues.
            5. [T] The user can change the map to a list to show all the sports venus
    2. [F] Allow logged in user to save the search history in their accounts
        1. [U] As a logged in user, I would like to save the search history in my own accounts so I can review them again in a quick time
            1. [T] Create SQL Server database to save the history for the user
    3. [F] Allow all users to add filters for the search result
        1. [U] As a visitor, I would like to see if the sports venues available or not after there are sports venues in my expect distance so I can create one or join others.
            1. [T] Set a button to allow user choose "available or all" sports venues
            2. [T] Add a reminder to user if there is no available sports venues when they choose "available"
            3. [T] Use ajax to show the results match the filters
        2. [U] As a visitor, I would like to add my preferred sports filter to the sports venues so I can choose a perfect place to play my sports
            1. [T] Add list box for the searching result page to allow user choose their favorite sports
            2. [T] Add a reminder to user if there is no sports venues match their filter
            3. [T] Use ajax to show the results match the filters
        3. [U] As a visitor, I would like to sort the sports venues by distance or rate so I can choose a sports venue perfect for me
            1. [T] Add a "sort by" list box in the searching result page, sort by rate or distance
            2. [T] The searching results are sort by distance by default
            3. [T] Use ajax to show the results after user change the sort way
            4. [T] Add "rate" box in the sports place detail page
3. [E] Allow users to make an appointment in an available sports venues, and users can edit or delete them
    1. [F] Add ability for users to create a new sports event or join others sports event when the place is available
        1. [U] As a logged in user, I would like to create a new sports event or join others so I can play my preferred sports in my preferred place
            1. [T] Add a create page
            2. [T] Add "category", "people", "time" boxes in the create page
            3. [T] Add "join" button under the existing sports event
    2. [F] Add ability for users to edit or delete their events in limited time
        1. [U] As a logged in user, I would like to edit or delete my event so that there is no collision with my schedule
            1. [T] Add an edit page
            2. [T] Add a delete page
            3. [T] The event can't be deleted in 1 hour before the event start
    3. [F] Add ability for user notifications
        1. [U] As a logged in user, I want to receive a notification when other users join my events so that I know how many people will in my event
            1. [T] Send a notification if others join the user's event
        2. [U] As a logged in user, I want to receive a notification when other users leave my events so that I know how many people will in my event
            1. [T] Send a notification if others leave the user's event
    4. [F] Add ability for users to invite others by email
        1. [U] As a logged in user, I want to invite my friends to join my sports event by email so that I can play with my friends
            1. [T] Add "share" button under the sports event, it's the link to open the email.
            2. [T] Add "copied" button to help user copy the link of the event
4. [E] Create an online clock-in(check-in?) system to make sure everybody who join the event will present
    1. [F] An online clockin system will be generated autmatically when the event start
        1. [U] As a user who in part of an event, I would like there is a clock-in system to make sure everyboday present so that I can play sports as my plan
            1. [T] Use current location to clock-in
    2. [F] The user will lost credit point if he didn't show in his event
        1. [U] As a user who in part of an event, I would like there is a punishment to the user who didn't show in the event so that more people will present on time
            1. [T] Depends on how long time the user late or didn't show, minus the credit point
        2. [U] As a moderator, I would like I have the power to add or minus credit point for users so that the user's credit can be correct if they lost point by mistake.
            1. [T] Send an email to the user who clocked-in the event to verify if the user who didn't clock-in but actually present
            2. [T] Add a support button on webpage to contact the moderator
## Initial Architecture Envisioning

## Agile Data Modeling

## Timeline and Release Plan