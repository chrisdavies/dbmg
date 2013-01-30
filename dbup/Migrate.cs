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
        private ProgramArgs args;
        private MigrationVersioning versioning;

        public Migrate(ProgramArgs args)
        {
            this.args = args;
        }

        public void Execute()
        {
            using (var db = DbConnectionFactory.Load(args.Provider))
            {
                db.ConnectionString = args.ConnectionString;
                db.Open();

                versioning = new MigrationVersioning(db, args);
                Execute(db);
            }
        }

        private void Execute(IDbConnection db)
        {
            var lastFile = string.Empty;
            foreach (var file in SqlFiles())
            {
                RunFile(db, file);
                lastFile = file;
            }

            versioning.LatestVersion = Path.GetFileName(lastFile);
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
            var files = Directory.GetFiles(args.MigrationsPath).OrderBy(s => s);
            var latestVersion = versioning.LatestVersion;

            if (!string.IsNullOrEmpty(args.InitialFile))
            {
                return files.Where(f => string.Compare(Path.GetFileName(f), args.InitialFile, System.StringComparison.OrdinalIgnoreCase) >= 0);
            }
            else if (!string.IsNullOrEmpty(latestVersion))
            {
                return files.Where(f => string.Compare(Path.GetFileName(f), latestVersion, System.StringComparison.OrdinalIgnoreCase) > 0);
            }

            return files;
        }
    }
}
