# dbmg

A simple .net database migration tool that allows you to write plain SQL.

## How it works

dbmg looks for sql files in the "db" folder (or another folder, if you override the defaults).  These files should be named so that they sequentially sort.  I recommend using the YYYY-MM-DD format, so: "2013-01-30-myfile.sql", but you can use whatever scheme you want.

SQL files can have multiple commands in them, separated by the word "GO" on its own line.

The current version of a database is tracked by a table called "dbmg" (again, this name is overridable).  When dbmg runs, it stores the last run sql file in this table.  The next time dbmg runs, it will only execute files whose name is greater than the max file name stored in the "dbmg" table.

## Usage

1. Drop some sql files in the "db" directory
2. Run: dbmg -c &lt;your-connection-string&gt;
3. Make a cocktail.

## Supported databases

The default database is postgres, but you can also specify the -p commandline option.  Possible values are "mssql" for Microsoft sql server, and "mysql" for my sql.

Most defaults can be overriden. Run dbmg with no parameters in order to see the possible commandline options.

## Down support

Down operations are not supported.  Back up your databse before running migrations, and restore from that backkup, if things get botched.

## Existing databases

If you've inherited an existing, unversioned database, you can still use dbmg. Just follow these steps:

1. Generate you database creation script.
2. Drop it into the "db" folder with a low name like "0000-initialdb.sql"
3. Run: dbmg -a "0000-initialdb.sql" -c &lt;your-connection-string&gt;

This will tell dbmg that this databse already contains the logic found in "0000-initialdb.sql".

## Get dbmg

Install-Package dbmg

Let me know if you have any issues.  Happy migrations!
