namespace dbmg.Tests
{
    using Should.Core.Assertions;
    using System;

    static class Extensions
    {
        public static Action CodeBlock(this object obj, Action action)
        {
            return action;
        }

        public static void ShouldThrow<T>(this Action action) where T : Exception
        {
            Assert.Throws<T>(() => action());
        }
    }
}
