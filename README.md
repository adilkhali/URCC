# United Remote Coding Challenge
This is my entry for the United Remote coding challenge , the Project is a .Net Core application that serves as a backend service for an app that lists the nearby shops .

## Feature completness (Back End)


 - [x] As a User, I can sign up using my email & password
 - [x]  As a User, I can sign in using my email & password
 - [x] As a User, I can display the list of shops sorted by distance
 - [x]  As a User, I can like a shop, so it can be added to my preferred shops

Bonus point (those items are optional):

 - [ ] [BONUS] As a User, I can dislike a shop, so it won’t be displayed within “Nearby Shops” list during the next 2 hours
 - [x] [BONUS] As a User, I can display the list of preferred shops
 - [x] [BONUS] As a User, I can remove a shop from my preferred shops list

## Setup
1.  Install the following:
    - [.NET Core](https://www.microsoft.com/net/core)  and  [.NET Core SDK](https://www.microsoft.com/net/core)
2. Clone the latest version of **URCC** on your local machine by running:

		$ git clone https://github.com/adilkhali/URCC.git
		$ cd URCC

4. Install .NET Core dependencies `$ dotnet restore`
## Build
Run `dotnet build` to build the project.

### Database 
I chose to use the SQL Server database to take advantage of the Geography Data Type for User and Shops location.

To create a database make sure that the connection string in the appsettings.json file if corecct and run the following command:

       $ cd '.\United Remote Coding Challeng\'
       $ dotnet ef database update

## Run the application 
To run the application enter to the Main entry point of the project and execute `dotnet watch run`

       $ cd '.\United Remote Coding Challeng\'
       $ dotnet watch run
Open browser and navigate to  [http://localhost:5000/swagger](http://localhost:5000/swagger). 

> Make sure that every request you enter a valid version number in this case we only have V1
> for example : [https://localhost:5000/api/**v1**/Account/Login](https://localhost:5000/api/v1/Account/Login)

## Running unit tests
Run `dotnet test` to execute the unit tests via [_xUnit_](https://xunit.net/).
Make sure you are in the following dirictory `UnitedRemote.Web.Test`

## Further help
For any additonal help please feel free to contact me anytime by email : adil.khali@gmail.com
