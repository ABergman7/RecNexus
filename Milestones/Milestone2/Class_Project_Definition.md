2018-19 Class Project Inception: Discussion Hub
=====================================

## Summary of Our Approach to Software Development

[What processes are we following?  What are we choosing to do and at what level of detail or extent?]

## Initial Vision Discussion with Stakeholders

People like to talk; people like to discuss and argue.  People like to state their opinions and read thoughts of others.  But there are real problems with how it is happening on the Web.  Just take a look at YouTube or Twitter comments.  That's not a discussion and it's not often useful, productive or even civil.  Social media is not the place to talk about things.  The Internet should be a place where people can *communicate*.  Let's make a site where people can do that!

Places like Reddit and Kialo already have a handle on general topic discussions, so we won't try to do that.  Enthusiasts often have their own sites with discussion and comment sections (e.g. Slashdot) and have formed close-knit communities that work well.  We don't want to do that either.  Here's our idea, using an example:

Let's say I just read this article on CNN, [American endurance athlete becomes the first person to cross Antarctica solo](https://www.cnn.com/2018/12/27/world/colin-obrady-antarctica-solo-trip-trnd/index.html), about a guy from Oregon who skied *across* Antarctica.  The thing is, is that he didn't and he wasn't the first.  His claims should be challenged and the article/journalist is disingenuous by not at least bringing this up.  People reading this article should know there are serious questions about it.  Someone should comment on this to point it out and spark further discussion.   There is no way to do this.  CNN does not have a comment section.  Even if it did, do I want to create an account there just so I can make a quick comment?  I'd have to do that everywhere on the web where I had a question or wanted to comment.  How would I know if my question was answered?  

We want a centralized discussion site that can be found easily and where an individual can maintain an account, build a history, expertise, level of trust, etc.  It should make it easy to create or find a discussion page about any news article, post or web site.  It should provide features for the user to follow their discussions without ever going back to the original website.  It should allow them to create and maintain their own identity that is separate from any social media identity.

## Initial Requirements Elaboration and Elicitation

### Questions

1. How do we link a discussion on the site to one or more articles/pages?

    We were thinking via URL. Each discussion post will have the original poster's article link as well as their original comment regarding the article.

2. How will users find out that there is a discussion on the site for the article/page they're currently viewing?
   
    How about a browser plug-in?  It could send the URL of the current page to our API to see if a discussion page exists and provide an easy way for them to navigate to our page.

     (Like the working principle of "honey", when you are in a shopping website, it will automatically find the coupons for you, and when you in the checkout page, it will pop out and apply the avaliable coupon.) We can make an extension of the browser like it, so when the user is in the news page, it will automatically running to search the discussion about news, when the user drag to the bottom, the extension will pop out and show all the discussion and the comment place .
    
     Or the user can copy the URL and paste it into a search bar on our site.

3. Clearly we need accounts and logins.  Our own, or do we allow logging in via 3rd party, i.e. "Log in with Google" or ...?  
    
    It depends on the different browser, if the user downloads extension from Google, they can use the Google account log in and comment, if they use Microsoft Edge, they can use Microsoft account to log in. Because most time where the user downloads the extension shows which browser they prefer to use and stay for a long time, it's convenient to update and send new message to the user.

    There are also different things to consider for accounts/logins. We could use Azure AD or Google login to authenticate a user but would also need a database table to keep statistics and information stored. 
    
4. Do we allow people to comment anonymously?  Read anonymously? 

    Users should not be able to comment anonymously, because they should take responsibility to their comment.
    In China, there is a very popular social media application named "Weibo", similar to Twitter, users can document their life, publish their views about something and comment others' views. If one user's view is forward more than 500 times, then the user will be liable for any economic and legal liability incurred herefrom.

    Another reason users should not be able to comment anonymously is because one of the main points of the discussion site is to build history and a level of trust which can not be done through anonymity. Users should be able to read discussions anonymously however they just would not be able to comment. 

5. Do we allow people to sign up with a pseudonym or will we demand/enforce real names?
    
    Yes, we think the pseudonym can encourage users publish their own opinions. However, we should also give the option to use either a username or real name.

6. What is it important to know about our users?  What data should we collect?

    Preference for news/topics such as what kinds of news they like so we can recommend more for them. We should collect the tags of news which users always scan. This can also be found out through their comment history if we track what type of articles they comment on the most. We could also collect data on what sites users post articles from. 
    
7. If there are news articles on multiple sites that are about the same topic should we have separate discussion pages or just one?

    We should keep one discussion area but don't know how this can be achieved without knowing if articles are going to be completely about the same topic. Because of this, we believe that there should be separete dicussion pages for each article so discussions can take place specifically around that news article alone as each article may have separate details.
   
8. What kind of discussion do we want to create? Linear traditional, chronological, ranked, or ?

    Ranked would be preferrable. The hottest comment can always attract more comments and make the news interesting.
   
9. Should we allow image/video uploads and host them ourselves?

    Images are fine, for example, sometims memes or images can help express feelings a lot. Unless videos are already embedded in the original article posted by the user, we should not allow videos for copyright purposes. 
   
10. Should we allow user check others' comment history?

    I don't think we should allow them to do that, it's like the part of the social media application, we just want to focus on the comment for news, also we not allow the user can follow other users. 

11. How can users record their loved comment?

    They can view the comments which they voted in the history or they can star the comment they like. 

### Interviews

Q: Will there be any moderators to monitor or control discussions?

A: Yes and no. They should make sure that users aren't harassed but we don't want it to be so controlled that people aren't allowed to speak their minds. The discussions should be open but also constructive. 

Q: Do you want every article and original discussion post to be reviewed by a moderator before being posted to the site?

A: No but we would like to provide a way for users to flag posts or comments as in appropriate. 

Q: Would you like posting articles and commenting to be a user-only feature or should anyone be allowed to do so whether or not they have an account?

A: Only users should be able to post articles and comment on discussions however if they're not a user, they should still be able to read posts and comments.

## List of Needs and Features

1. A great looking landing page with info to tell the user what our site is all about and how to use it.  Include a link to and a page with more info.  Needs a page describing our company and our philosophy.
2. The ability to create a new discussion page about a given article/URL.  This discussion page needs to allow users to write comments.
3. The ability to find a discussion page.
4. User accounts
5. A user needs to be able to keep track of things they've commented on and easily go back to those discussion pages.  If someone rates or responds to their comment we need to alert them.
6. Allow users to identify fundamental questions and potential answers about the topic under discussion.  Users can then vote on answers.
7. A user can delete or edit his comment.
8. Once a user edit his comment, the vote for the original comment will be clear.
9. Once the article/news deleted by authors, all the comments will disappear but can save in users' own comment history.
10. The ability to set certain users as moderators who can help keep discussions civil. 

## Initial Modeling


### Use Case Diagrams

### Other Modeling

## Identify Non-Functional Requirements

1. User accounts and data must be stored indefinitely.
2. Site and data must be backed up regularly and have failover redundancy that will allow the site to remain functional in the event of loss of primary web server or primary database.  We can live with 1 minute of complete downtime per event and up to 1 hour of read-only functionality before full capacity is restored.
3. Site should never return debug error pages.  Web server must never return 404's.  All server errors must be logged.  Users should receive a custom error page in that case telling them what to do.
4. Must work in all languages and countries.  English will be the default language but users can comment in their own language and we may translate it.
5. Comments on discussion pages should be updated in real time to alow for up to date discussions. 
6. Accuracy checks will be needed to ensure valid URLs for articles are provided and that they are posted by verified users on the site. 
7. Users may choose whether to display their username or their real name. 

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
        3. [U] As a robot I would like to be prevented from creating an account on your website so I don't ask millions of my friends to join your website and add comments about male enhancement drugs.

2. [E] Allow users to create, read, update, or delete articles
    1. [F] Allow logged in user to create new discussion page
        1. [U] As a logged in user, I want to be able to post an article along with my assessment of it so that I can share my viewpoint with others. 
            1. [T] Create page for user to create a new post
        2. [U] As a logged in user, I want to be able to view the actual posted news article so I can make my own judgement on the news. 
            1. [T] Require link to article in new discussion page
    2. [F] Allow any user to search for and find an existing discussion page
        1. [U] As a logged in user, I want to be able to search for an existing discussion so I can find articles that are relevant to my interests
        2. [U] As a visitor (not logged in), I want to be able to browse and search existing discussions so I can find articles that are relevant to my interests
            1. [T] Visitors should be able to search for discussions without ability to comment
    3. [F] Notify user when another user comments on their article
        1. [U] As a logged in user, I want to receive a notification when other users reply to my article so I can view their responses 
            1. [T] Send notification to user via when receives a response on article 
            2. [T] Put link to article in email
    4. [F] Allow tagging capabilities to articles for categories
        1. [U] As a logged in user, I want to be able to select the categories of articles that appears on my queue so I don’t view any discussions that do not interest me. 
            1. [T] Add capability for users to filter categories such as checkboxes
    5. [F] Moderators should be able to delete discussion posts 
        1. [U] As a moderator, I want to be able to remove discussion posts so I can ensure that there are no inappropriate or offensive posts.
            1. [T] Add Remove links to all posts if user is moderator
 
3. [E] Allow users to create, read, update, or delete comments on articles
    1. [F] Add ability for users to comment on an existing article
        1. [U] As a logged in user, I want to be able to comment on an existing article/discussion so I can share my views and opinions. 
            1. [T] Add Comment links to articles 
            2. [T] Add Comment/Reply links to comments
        2. [F] Notify user when someone replies to their comment
            1. [U] As a logged in user, I want to receive a notification when other users reply to my comment on the discussion board so I can engage in a conversation with them.
                1. [T] Send a notification to the user via email whenever there is response to their comment
                2. [T] Put link to discussion thread in email 
    2. [F] Add ability for users to edit or remove their comments 
        1. [U] As a logged in user, I want to be able edit or remove my comment so if it receives a certain amount of down votes so I know it is an unpopular opinion amongst other users. 
            1. [T] Send a notification if the user’s comment receives a certain amount of down votes
            2. [T] Add link/page to delete a comment
            3. [T] Add link/page to edit a comment
    3. [F] Add ability for user notifications
        1. [U] As a logged in user, I want to receive a notification when my comment receives a certain amount of upvotes so that I know my opinions are popular
            1. [T] Send a notification if the user’s comment receives a certain amount of upvotes
        2. [U] As a logged in user, I want to receive a notification when my comment receives a certain amount of downvotes so I know when my opinions are disagreed with  
            1. [T] Send a notification if the user’s comment receives a certain amount of up votes
    4. [F] Moderators should be able to delete discussion posts 
        1. [U] As a moderator, I want to be able to remove discussion posts so I can ensure that there are no inappropriate or offensive posts.
            1. [T] Add Remove links to all posts if user is moderator
    5. [F] Adding a menu/tab to ordering the discussion by popularity
        1. [U] As a logged in user, I want to see the most popular discussions on the top of the queue so I can see which news are relevant to other users 
            1. [T] Add an up vote / down vote functionality and order the discussions by the votes. 
    6. [F] Adding a menu/tab to ordering the discussion by most recent 
        1. [U] As a logged in user, I want to see the most recent discussions on the top of the queue so I can be up to date with the news. 
            1. [T] Add a timestamp to the discussions, and order the discussions by the timestamp. 


4. [E] Website should have a reporting cability for inappropriate/offensive posts or comments
    1. [F] Give user ability to report a post or comment
        3. [U] As a logged in user, I want to be able to report a post or comment if I find it offensive so appropriate action can be taken against the post
            1. [T] Add Report link
            2. [T] Add Report page with reason for reporting
            3. [T] Store report 
    2. [F] Temporarily/Permanently banning users that have a certain amount of reported comments
        1. [U] As a frequent user, I do not want to see the same user repeatedly posting offensive comments so I can engage in discussion with users that respect other’s opinion
        2. [U] As a news critic, I want to be able to given a couple of warnings before I get permanently banned so I can learn how to comment without offending other users. 
            1. [T] Temporarily locking the commenting feature for users that have received a certain number of reports on their account/ comments
            2. [T] Permanently banning the account if the user reaches a certain number of reports
        3. [U] As a moderator, I want the ability to remove users if they've been given too many warnings so that they can no longer engage in discussions. 
    3. [F] Allow moderators to view and handle reports made by users
        1. [U] As a moderator, I want to be able to view any reports that users submit regarding inappropriate/offensive posts or comments so I can review them, respond, and take action accordingly.
            1. [T] Create Moderator-only page
            2. [T] Display all open requests where users reported a post or comment 
            3. [T] Allow moderators to respond or complete report requests 

## Initial Architecture Envisioning

## Agile Data Modeling

## Timeline and Release Plan