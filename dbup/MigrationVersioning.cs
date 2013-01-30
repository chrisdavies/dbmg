namespace DBUp
{
    using Dapper;
    using System.Data;
    using System.Data.Common;
    using System.Linq;

    public class MigrationVersioning
    {
        private ProgramArgs args;
        private IDbConnection db;

        public string LatestVersion 
        {
            get
            {
                var first = db.Query("SELECT MAX(file_name) file_name FROM " + args.TableName).FirstOrDefault();
                return first == null ? null : first.file_name;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    db.Execute("INSERT INTO " + args.TableName + " (file_name) VALUES (@file_name);", new { file_name = value });
                }
            }
        }

        public MigrationVersioning(IDbConnection db, ProgramArgs args)
        {
            this.args = args;
            this.db = db;

            EnsureTableExists();
        }

        private void EnsureTableExists()
        {
            try
            {
                // Only workable cross-provider way to detect table existence...
                db.Query("SELECT * FROM " + args.TableName + ";");
            }
            catch (DbException)
            {
                db.Execute("CREATE TABLE " + args.TableName + @" (  
                    file_name varchar(255) NULL
                )");
            }
        }
    }
}
