# dbup

A simple .net database migration tool that allows you to write plain SQL.

## How it works

dbup looks for sql files in the "db" folder (or another folder, if you override the defaults).  These files should be named so that they sequentially sort.  I recommend using the YYYY-MM-DD format, so: "2013-01-30-myfile.sql", but you can use whatever scheme you want.

SQL files can have multiple commands in them, separated by the word "GO" on its own line.

The current version of a database is tracked by a table called "dbup" (again, this name is overridable).  When dbup runs, it stores the last run sql file in this table.  The next time dbup runs, it will only execute files whose name is greater than the max file name stored in the "dbup" table.

## Usage

1. Drop some sql files in the "db" directory
2. Run: dbup -c &lt;your-connection-string&gt;
3. Make a cocktail.

## Down support

Down operations are not supported.  Back up your databse before running migrations, and restore from that backkup, if things get botched.

## Existing databases

If you've inherited an existing, unversioned database, you can still use dbup. Just follow these steps:

1. Generate you database creation script.
2. Drop it into the "db" folder with a low name like "0000-initialdb.sql"
3. Run: dbup -a "0000-initialdb.sql" -c &lt;your-connection-string&gt;

This will tell dbup that this databse already contains the logic found in "0000-initialdb.sql".

## Get dbup

A nuget package is forthcoming.  Let me know if you have any issues.  Happy migrations!
