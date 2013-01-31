namespace dbmg.Tests
{
    using Dapper;
    using System;
    using System.Data;
    using System.Data.SQLite;

    static class Database
    {
        public static string ConnStr
        {
            get
            {
                return "Data Source=./dbmg.sqlite; Version=3;";
            }
        }

        public static void Run(Action<IDbConnection> action)
        {
            using (var conn = new SQLiteConnection(ConnStr))
            {
                conn.Open();
                action(conn);
            }
        }

        public static void Clear()
        {
            Run(db => db.Execute(@"
                DROP TABLE IF EXISTS foo; 
                DROP TABLE IF EXISTS dbmg;"));
        }
    }
}
