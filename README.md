# Library API

This project aims to control the borrowing of books from a fictional library.

Features available for access:
* List of Books
* Borrow a book

## Technologies implemented:

- ASP.NET 6.0
- Dapper
- MediatR
- Swagger

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


## List of Books

### Request

`GET /books/`

    curl -i -H 'Accept: application/json' http://localhost:5001/api/books

### Response body

    [
        {
            "title": "Title of book",
            "author": "Author",
            "pages": 0,
            "publisher": "Blucher",
            "bookCategoryId": "20efaba1-64bd-4b7f-82f4-c1d05550e305",
            "id": "4c008a16-6725-42c6-83fc-289a1f230b38"
        }
    ]


### Response code

| Code | Description |
|---|---|
| `200` | Success.|


## Borrow a Book

### Request

`Post /books/borrow`

### Request Url
    http://localhost:5001/api/books/{bookId}/student/{studentEmail}/borrow

### Response code

| Code | Description |
|---|---|
| `200` | Success.|
| `400` | Email cannot be empty.|
| `403` | The book does not belong to the course category.|
| `404` | Student not found in database by email.|
