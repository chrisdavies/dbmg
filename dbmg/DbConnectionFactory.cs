namespace dbmg
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    public class DbConnectionFactory
    {
        public static IDbConnection Load(string name)
        {
            var providers = new Dictionary<string, Func<IDbConnection>>(StringComparer.OrdinalIgnoreCase)
            {
                { "postgres", () => new Npgsql.NpgsqlConnection() },
                { "mysql", () => new MySql.Data.MySqlClient.MySqlConnection() },
                { "mssql", () => new System.Data.SqlClient.SqlConnection() },
                { "sqlite", () => new System.Data.SQLite.SQLiteConnection() }
            };

            Func<IDbConnection> getConnection;

            if (!providers.TryGetValue(name, out getConnection))
            {
                throw new ArgumentException(string.Format("Couldn't find db provider {0}.", name));
            }

            return getConnection();
        }
    }
}
