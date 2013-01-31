namespace dbmg.Tests
{
    using Should;

    static class App
    {
        public static void Run() 
        {
            dbmg.Program.Main("-c", Database.ConnStr, "-p", "sqlite").ShouldEqual(0);
        }
    }
}
