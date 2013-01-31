namespace dbmg.Tests
{
    using Should;
    using System.Linq;

    static class App
    {
        public static void Run(params string[] args) 
        {
            args = new string[] { "-c", Database.ConnStr, "-p", "sqlite" }
                .Union(args ?? new string[] { }).ToArray();
            dbmg.Program.Main(args).ShouldEqual(0);
        }
    }
}
