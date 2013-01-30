namespace DBUp
{
    using System;

    public static class Logger
    {
        public static void WriteLine(string line = null, params object[] args)
        {
            WriteLine(Console.ForegroundColor, line, args);
        }

        public static void WriteLine(ConsoleColor color, string line, params object[] args)
        {
            var original = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(line ?? string.Empty, args);
            Console.ForegroundColor = original;
        }
    }
}
