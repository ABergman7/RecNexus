# Guidelines 

For this project, we use ASP.NET MVC which contains C#, JavaScript, CSS, and Razor:

## C# Guidelines

* Refer to [this page](https://social.technet.microsoft.com/wiki/contents/articles/34605.c-coding-conventions-rules-best-practices-better-coding-standards.aspx) for best practices and standards that we follow for our code base.  
* Use XML comments for all public methods

## MVC Guidelines

* Always remove any controllers, models, or views that are not being used 
* Domain models are not the same as view models. ViewModels should be designed around the needs of the View itself and not what is returned from the data layer. 
* Validate models and model properties where necessary 

## Javascript

* Use external script files
* Split up script files by functionality where needed. There should not be one Javascript that serves entire project 

## Style ##

* Use external CSS files

## Razor 

* A quick reference guide to Razor can be found [here](https://docs.microsoft.com/en-us/aspnet/web-pages/overview/getting-started/introducing-razor-syntax-c)
* We also use HTML helpers in our Razor views. Please click [here](https://docs.microsoft.com/en-us/dotnet/api/system.web.mvc.htmlhelper?view=aspnet-mvc-5.2) for a list of HTML helper methods 


## Database

* Table names should be singular and should be written in Pascal case. 
* Table primary keys and foreign keys are `<Entity>ID`
* Foreign keys should be the last columns of the table 

## Git 

* Use branches
* Commit often (don't feel like you have to have made major, complete, changes or new features before committing)
* Write good commit messages
* Don't commit code that doesn't compile
* It's OK to work on a separate testing file in your local repository in order to learn something, but don't keep multiple copies of a real file around just to keep some commented out pieces of code.  As long as you have committed often you can always go back anywhere in your history to see what it looked like
* Don't add and commit any files that are auto-generated (i.e. html documentation, .o, .tmp, ...)

## Our Bitbucket repository guidelines

Our repository contains two main branches: `development` and `master`. Here are some guidelines regarding branches in this repository:

* Merges can only be made to `development` branch via pull request
* Pull requests can only be approved and completed by the repository owner 
* Merging `development` into `master` will be done at the end of each sprint by the repository owner 
* Feature branches should be named according to the milestone or sprint we are on as well as the user story or task that is being inmplented. Example: `Milestone4_IndividualUserAccounts`

## Pull requests

Before you start working on a feature, ensure that your local and forked repository are synced with the latest changes in `development` and `master` of the official repository then:

* Create a feature branch from `development`
* Checkout your feature branch and work on your feature committing changes every so often
* When feature is tested and finished, commit all changes
* Before you create a pull request, be sure there are no merge conflicts by updating your `development` branch and merging it into your feature branch 
* If no conflicts, push your feature branch then create a pull request to merge your branch with your feature branch as the source and `development` as the destination 
* The feature branch will be merged into `development` upstream when the repository owners approves and completes the pull request 
* Once approved, you (and all members) would need to update forked branches 

Example command line workflow that follows above scenario: 

```
git checkout development
git branch Milestone4_IndividualUserAccounts
git checkout Milestone4_IndividualUserAccounts

# Do work
git add .
git commit -m "Add individual accounts and update connection string"

# Work done and tested
git checkout development
git pull upstream development
git checkout Milestone4_IndividualUserAccounts
git merge development

# Fix conflicts if any, add/commit them on Milestone4_IndividualUserAccounts

git push origin Milestone4_IndividualUserAccounts

# Do pull request
# If accepted, proceed with below commands to sync forked repo with official repo

git checkout development
git pull upstream development
git push origin development
```

