namespace DBUp
{
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;

    public class Migrate
    {
        public ProgramArgs Args { get; set; }

        public Migrate(ProgramArgs args)
        {
            this.Args = args;
        }

        public void Execute()
        {
            using (var db = DbConnectionFactory.Load(Args.Provider))
            {
                db.ConnectionString = Args.ConnectionString;
                db.Open();

                Execute(db);
            }
        }

        private void Execute(IDbConnection db)
        {
            foreach (var file in SqlFiles())
            {
                RunFile(db, file);
            }
        }

        private void RunFile(IDbConnection db, string file)
        {
            foreach (var command in File.ReadAllText(file).Split(new string[] { Environment.NewLine + "GO" }, StringSplitOptions.RemoveEmptyEntries))
            {
                try
                {
                    db.Execute(command);
                    Logger.WriteLine(ConsoleColor.Green, command);
                    Logger.WriteLine();
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(ConsoleColor.Red, command);
                    Logger.WriteLine();
                    Logger.WriteLine(ConsoleColor.Red, ex.Message);
                    Logger.WriteLine();
                    throw;
                }
            }
        }

        private IEnumerable<string> SqlFiles()
        {
            var files = Directory.GetFiles(Args.MigrationsPath);
            return string.IsNullOrEmpty(Args.InitialFile) ? files : files.Where(f => string.Compare(Path.GetFileName(f), Args.InitialFile, System.StringComparison.OrdinalIgnoreCase) >= 0);
        }
    }
}
