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
            SqlScripts.Dir = "db";
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
        public void Option_i_includes_the_specified_file()
        {
            SqlScripts.Write(Resources.Sql000);
            App.Run();
            SqlScripts.Write(Resources.Sql000, Resources.Sql001, Resources.Sql002, Resources.Sql003);
            App.Run("-i", "0002.sql");
            Database.AssertIds(new int[] { 1, 6, 7 });
        }
        
        [Fact]
        public void Option_a_excludes_the_specified_file_and_marks_db_with_proper_version()
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

        [Fact]
        public void Option_d_makes_the_app_use_a_different_directory()
        {
            SqlScripts.Dir = "alt";
            SqlScripts.Clear();
            SqlScripts.Write(Resources.Sql000);
            App.Run("-d", "alt");
            SqlScripts.AssertVersion000();
        }

        /// <summary>
        /// A poor-man's test, since I don't want our test machines to require postgres,
        /// mysql, and mssql instances.
        /// </summary>
        [Fact]
        public void Option_p_changes_the_provider_being_used()
        {
            DbConnectionFactory.Load("mssql").GetType().Name.ShouldEqual("SqlConnection");
            DbConnectionFactory.Load("mysql").GetType().Name.ShouldEqual("MySqlConnection");
            DbConnectionFactory.Load("postgres").GetType().Name.ShouldEqual("NpgsqlConnection");
        }
    }
}
