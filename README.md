# Library API

This project aims to control the borrowing of books from a fictional library.

## Technologies implemented:

- ASP.NET Core 6.0 (Migrated from ASP.NET Core 3.1 to ASP.NET Core 6.0)
- Dapper
- MediatR
- Swagger

Features available for access:
- [List of Students](#list-of-students)
- [List of Books](#list-of-books)
- [Borrow a book](#borrow-a-book)

## Getting Started
To run this project you will need:

1. .Net core sdk 6.0 (https://dotnet.microsoft.com/download)

1. MSSql LocalDb - (https://download.microsoft.com/download/7/c/1/7c14e92e-bdcb-4f89-b7cf-93543e7112d1/SqlLocalDB.msi)

1. NodeJs (https://nodejs.org/en/download/)

With all these tools installed, run the following commands in your operating system's terminal:

## Usage

```
dotnet run --project .\backend\src\Library.Api\Library.Api.csproj -- seed
```
This command will run the site on url "https://localhost:5001" and the database will already be created with some data already inserted. You can check the data in the **DatabaseBoostrp.cs** file.

**Note: Every time you execute this command the data in the tables will be deleted. To not erase the data, just remove the argument "-- seed"**

## Run the tests

```
dotnet test ./backend/src/Library.UnitTests/Library.UnitTests.csproj
```
## Docker Usage

```
docker-compose up
```

url: `http://localhost:5000`

swagger url: `http://localhost:5000/api/swagger/index.html`

## Docker Run the tests

```
docker build --target unittest -t test . 
```

```
docker run -it --rm --name unit-test test
```
## REST API

The REST API to the example app is described below.

## List of Students

### Request

`GET /students/`

### Request Url
    http://localhost:5001/api/students

### Response body

    [
        {
            "name": "Student Name",
            "email": "student@email",
            "courseId": "a2c6f987-d83f-4fb3-9982-68553965b421",
            "id": "1673a9fd-191a-479c-a41f-3dc5611aa98e"
        }
    ]


### Response code

| Code | Description |
|---|---|
| `200` | Success.|


## List of Books

### Request

`GET /books/`

### Request Url
    http://localhost:5001/api/books

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
