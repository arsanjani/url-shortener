# ScissorLink
Powerful URL shortener library in ASP.NET Core WebAPI and C# which contains several features such as click tracking, SQL back-end, internal cache, and etc.  

# Feautures
- Designed by n-tier architecture. 
- Provided click tracking information such as country of users by matching their IP, operatin system, browsers , Date, Time and etc.
- Implemented internal cache, resulting in higher response time on repetitive requests.
- Used Dapper as ORM, resulted in better readability of codes and higher performance compared with EF.
- Focused on independent protection layer, so layers connected to each other by Interfaces and could be easily modified.

# How to use
1. Run [Script](Script.sql) in your target database. It includs two tables, one for short link data and another one for storing statistical tracking of users.
2. Edit ConnectionStrings in [appsettings.json](src/appsettings.json) file
3. Insert your short link records in shortlink table in sql
4. Run the project and have fun ;)

# TODO List
1. Modify repository layer to accept different database type
2. Impelement admin panel so that users would register and manage thier links and statistics.
