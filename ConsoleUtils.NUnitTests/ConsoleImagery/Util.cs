using NUnit.Framework;

namespace ConsoleUtils.NUnitTests
{
    static class ConsoleImagery_Utils
    {
        public static void Generic_NullEquality<T>(T value) where T : class
        {
            T? NullOne = null;
            T? NullTwo = null;
            Assert.That(NullOne == NullTwo);
            Assert.That(NullOne != NullTwo, Is.False);
            Assert.That(NullOne == value, Is.False);
            Assert.That(NullOne != value);
            Assert.That(value == NullTwo, Is.False);
            Assert.That(value != NullTwo);
        }
    }
}