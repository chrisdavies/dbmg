namespace dbmg.Tests
{
    using Dapper;
    using dbmg.Tests.Properties;
    using Should;
    using System.IO;
    using System.Linq;

    static class SqlScripts
    {
        public static void Clear()
        {
            if (Directory.Exists("db"))
            {
                Directory.Delete("db", true);
            }

            Directory.CreateDirectory("db");
        }

        public static void WriteInitial()
        {
            Clear();
            File.WriteAllText("db/000.sql", Resources.Sql000);
        }

        public static void VerifyInitial()
        {
            Database.Run(db => {
                var recs = db.Query("SELECT * FROM foo");
                recs.Count().ShouldEqual(1);
                ((string)recs.First().name).ShouldEqual("Chris Davies");
            });
        }
    }
}