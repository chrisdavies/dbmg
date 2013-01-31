namespace dbmg.Tests
{
    using Dapper;
    using Should;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SQLite;
    using System.Linq;

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
        
        public static int[] AllIds()
        {
            int[] ids = null;
            Database.Run(db =>
            {
                ids = db.Query("SELECT * FROM foo ORDER BY id")
                    .Select(r => (int)r.id).ToArray();
            });

            return ids;
        }
        
        public static void AssertIds(IEnumerable<int> ids)
        {
            var existingIds = AllIds();
            existingIds.Length.ShouldEqual(ids.Count());
            existingIds.Except(ids).Count().ShouldEqual(0);
        }
    }
}
