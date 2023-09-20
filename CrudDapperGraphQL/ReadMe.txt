1. Install package dependecies:
	- Dapper
	- Microsoft.Data.SqlClient
	- HotChocolate
	- HotChocolate.AspNetCore
	- HotChocolate.Server
2. See Migrations/Migration script (TBD - with package FluentMigrator.Runner)
3. Queries:
==============================
    query {
        getBook(id:2){
            id
            title
            releaseDate
            authors{
                name
                surname
            }
        }
        getAuthor(id:4){
            id
            name
            surname
            books{
                id
                title
                releaseDate
            }
        }
    }
==============================
    query($filter: FilterModelInput) {
        getBooks(filter:$filter){
            id
            title
            releaseDate
            authors{
                name
                surname
            }
        }
    }
    -- variable
    {
        "filter":{
            "limit":10,
            "orderBy":"Title",
            "orderDirection":"ASC",
            "searchText":"the"
        }
    }
==============================
    query($filter: FilterModelInput) {
        getAuthors(filter:$filter){
            id
            name
            surname
            books{
                id
                title
                releaseDate
            }
        }
    }
    -- variable
    {
        "filter":{
            "limit":1,
            "offset":0,
            "orderBy":"bookCount",
            "orderDirection":"DESC",
            "searchText":"Doi"
        }
    }
==============================
    mutation($author: AuthorSaveInput!) {
        saveAuthor(author:$author){
            id
            name
            surname
        }
    }
    -- variable
    {
        "author":{
            "name":"Daniel",
            "surname":"Defoe"
        }
    }
==============================
    mutation($book: BookSaveInput!) {
        saveBook(book:$book){
            id
            title
            releaseDate
            authors{
                id
                name
                surname
            }
        }
    }
    -- variable
    {
        "book":{
            "title":"Robinson Crusoe",
            "releaseDate":"1753-04-25T00:00:00.000",
            "authorIds":[6]
        }
    }
