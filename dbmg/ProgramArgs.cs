﻿namespace dbmg
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class ProgramArgs
    {
        public string ConnectionString { get; set; }
        public string Provider { get; set; }
        public string MigrationsPath { get; set; }
        public string TableName { get; set; }
        public string InitialFile { get; set; }
        public string AfterFile { get; set; }

        public string InitialFilePath
        {
            get
            {
                return Path.Combine(MigrationsPath, InitialFile);
            }
        }

        public ProgramArgs(params string[] args)
        {
            SetDefaults();
            InitFromCommandLine(args);
            Validate();
        }

        private void SetDefaults()
        {
            Provider = "postgres";
            MigrationsPath = "./db";
            TableName = "dbmg";
        }

        private void InitFromCommandLine(string[] args)
        {
            var options = new Dictionary<string, Action<string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "-c", (val) => ConnectionString = val },
                { "-d", (val) => MigrationsPath = val },
                { "-t", (val) => TableName = val },
                { "-i", (val) => InitialFile = val },
                { "-p", (val) => Provider = val },
                { "-a", (val) => AfterFile = val }
            };
            
            for (var i = 0; i < args.Length - 1; i += 2)
            {
                var arg = args[i];
                Action<string> setProperty;
                if (!options.TryGetValue(arg, out setProperty))
                {
                    throw new ArgumentException(string.Format("Unknown argument {0}.", arg));
                }

                setProperty(args[i + 1]);
            }
        }

        private void Validate()
        {
            if (string.IsNullOrEmpty(ConnectionString))
            {
                throw new ArgumentException("The connection string (-c) must be provided.");
            }

            if (!Directory.Exists(MigrationsPath))
            {
                throw new ArgumentException("Could not find path " + MigrationsPath);
            }

            if (!string.IsNullOrEmpty(InitialFile) && !string.IsNullOrEmpty(AfterFile))
            {
                throw new ArgumentException("You cannot specify both the -a and the -i options.");
            }

            if (!string.IsNullOrEmpty(InitialFile) && !File.Exists(InitialFilePath))
            {
                throw new ArgumentException("Could not find file " + InitialFilePath);
            }
        }
    }
}
