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
                var args = new ProgramArgs();
            }
            catch (ArgumentException ex)
            {
                Log(ConsoleColor.Red, ex.Message);
            }
        }

        public static void Log(ConsoleColor color, string line, params object[] p)
        {
            var original = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(line, p ?? new object[] { });
            Console.ForegroundColor = original;
        }

        private static void Help()
        {
            Console.WriteLine(Resources.Instructions);
        }
    }
}
