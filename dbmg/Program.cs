namespace dbmg
{
    using dbmg.Properties;
    using System;

    public class Program
    {
        public static int Main(params string[] args)
        {
            try
            {
                new Migrate(new ProgramArgs(args)).Execute();
                return 0;
            }
            catch (ArgumentException ex)
            {
                Logger.WriteLine(ConsoleColor.Red, ex.Message);
                Help();
                return 1;
            }
        }
        
        private static void Help()
        {
            Console.WriteLine(Resources.Instructions);
        }
    }
}
