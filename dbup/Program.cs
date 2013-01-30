namespace DBUp
{
    using DBUp.Properties;
    using System;

    internal class Program
    {
        private static void Main()
        {
            try
            {
                new Migrate(new ProgramArgs()).Execute();
            }
            catch (ArgumentException ex)
            {
                Logger.WriteLine(ConsoleColor.Red, ex.Message);
                Help();
            }
        }
        
        private static void Help()
        {
            Console.WriteLine(Resources.Instructions);
        }
    }
}
