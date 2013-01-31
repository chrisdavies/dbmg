namespace dbmg.Tests
{
    using Xunit;

    public class Migrations
    {
        public Migrations()
        {
            Database.Clear();
        }

        [Fact]
        public void Running_migrations_twice_should_do_nothing_the_second_time()
        {
            SqlScripts.WriteInitial();
            App.Run();
            SqlScripts.VerifyInitial();
            App.Run();
            SqlScripts.VerifyInitial();
        }
    }
}
