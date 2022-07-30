# Library .Net Core 6

This project aims to control the borrowing of books from a fictional library.

Some features are already developed:
* Login
* Book listing
* Borrow a book


## Tools
To run this project you will need:

1. .Net core sdk 6.0 (https://dotnet.microsoft.com/download)

1. MSSql LocalDb - (https://download.microsoft.com/download/7/c/1/7c14e92e-bdcb-4f89-b7cf-93543e7112d1/SqlLocalDB.msi)

1. NodeJs (https://nodejs.org/en/download/)

With all these tools installed, run the following commands in your operating system's terminal:

### Backend:

```
dotnet run --project .\backend\src\Library.Api\Library.Api.csproj -- seed
```
This command will run the site on url "https://localhost:5001" and the database will already be created with some data already inserted. You can check the data in the **DatabaseBoostrp.cs** file.

**Note: Every time you execute this command the data in the tables will be deleted. To not erase the data, just remove the argument "-- seed"**

### Frontend:

```
frontend cd
npm install
npm run serves
```

This command will run the frontend on url "http://localhost:8080"