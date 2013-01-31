namespace dbmg.Tests
{
    using dbmg.Tests.Properties;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Xunit;
    using Dapper;
    using System.Linq;
    using Should;

    public class MigrationTest
    {
        public MigrationTest()
        {
            Database.Clear();
            SqlScripts.Clear();
        }

        [Fact]
        public void Running_migrations_twice_should_do_nothing_the_second_time()
        {
            SqlScripts.Write(Resources.Sql000);
            App.Run();
            SqlScripts.AssertVersion000();
            App.Run();
            SqlScripts.AssertVersion000();
        }

        [Fact]
        public void Migrations_run_in_order()
        {
            SqlScripts.Write(Resources.Sql000, Resources.Sql001);
            App.Run();
            SqlScripts.AssertVersion001();
        }

        [Fact]
        public void New_versions_are_run()
        {
            SqlScripts.Write(Resources.Sql000);
            App.Run();
            SqlScripts.AssertVersion000();
            SqlScripts.Write(Resources.Sql000, Resources.Sql001);
            App.Run();
            SqlScripts.AssertVersion001();
        }

        [Fact]
        public void Option_initial_file_includes_the_specified_file()
        {
            SqlScripts.Write(Resources.Sql000);
            App.Run();
            SqlScripts.Write(Resources.Sql000, Resources.Sql001, Resources.Sql002, Resources.Sql003);
            App.Run("-i", "0002.sql");
            Database.AssertIds(new int[] { 1, 6, 7 });
        }
        
        [Fact]
        public void Option_after_file_excludes_the_specified_file_and_marks_db_with_proper_version()
        {
            SqlScripts.Write(Resources.Sql000);
            App.Run();
            SqlScripts.Write(Resources.Sql000, Resources.Sql001, Resources.Sql002);
            App.Run("-a", "0002.sql");
            SqlScripts.AssertVersion000();
            App.Run();
            SqlScripts.AssertVersion000();
            SqlScripts.Write(Resources.Sql000, Resources.Sql001, Resources.Sql002, Resources.Sql003);
            App.Run();
            Database.AssertIds(new int[] { 1, 7 });
        }
    }
}
