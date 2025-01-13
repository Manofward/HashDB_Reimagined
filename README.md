# HashDB_Reimagined_

## Used Repos:

This Project Uses the lil_Hash Repository:  
[Lil Hash](https://github.com/Manofward/lil_hash)

## Usage of the Project:

This Project is only to learn how someone might use a DB with a Hashing on a website.
Because of this it is not optimized for usage in a work enviroment.

When the Repository has been cloned you dont have to build the whole Project you can go to the following Folder:  
**HashDB\BlazorApp1\bin\Release\net9.0\publish**

There will be an BlazorApp1.exe and this is the File you want to execute.  
As of the time I am writing this there is only a Windows build and I want to add the other Systems.

If you have any Issues or have a good Idea which can also be added to add more to learn for all feel free to make an Issue.

## Special things for Developers:

### Git Submodule Usage:
> Everything command here has to be used in the project folder where you can go into lil_hash and BlazorApp1

1. initializing submodule

```bash
git submodule init
```
This will initialize the submodule for usage

2. When pushing a change to lil_hash you have to update the submodule:

```bash
git submodule update
```

that will update 

### Shoutouts:
The DB I used to create this Project was DuckDB, because of this I did have to use DuckDB Repository for .Net.