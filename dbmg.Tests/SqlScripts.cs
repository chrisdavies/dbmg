namespace dbmg.Tests
{
    using Dapper;
    using dbmg.Tests.Properties;
    using Should;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    static class SqlScripts
    {
        private static string dir = "db";

        public static string Dir
        {
            get { return dir; }
            set { dir = value; }
        }

        public static void Clear()
        {
            if (Directory.Exists(Dir))
            {
                Directory.Delete(Dir, true);
            }

            Directory.CreateDirectory(Dir);
        }

        public static void Write(params string[] files)
        {
            for (var i = 0; i < files.Length; ++i)
            {
                File.WriteAllText(Path.Combine(Dir, i.ToString("0000") + ".sql"), files[i]);
            }
        }

        public static void AssertVersion000()
        {
            Database.AssertIds(new int[] { 1 });
        }

        public static void AssertVersion001()
        {
            Database.AssertIds(new int[] { 1, 2, 3, 4, 5 });
        }
    }
}