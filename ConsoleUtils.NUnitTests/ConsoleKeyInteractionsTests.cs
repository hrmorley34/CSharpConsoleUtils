using NUnit.Framework;
using System;
using ConsoleUtils.ConsoleKeyInteractions;

namespace ConsoleUtils.NUnitTests
{
    public class ConsoleKeyInteractionsTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void PositiveIntegerKeyHandler_TestKeys()
        {
            var p = new PositiveIntegerKeyHandler();

            p.HandleKey(new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false));

            Assert.That(p.Finished(), Is.False);
            Assert.That(() => p.GetReturnValue(), Throws.TypeOf<NoValueException>());

            p.HandleKey(new ConsoleKeyInfo('1', ConsoleKey.D1, false, false, false));
            p.HandleKey(new ConsoleKeyInfo('2', ConsoleKey.D2, false, false, false));
            p.HandleKey(new ConsoleKeyInfo('3', ConsoleKey.D3, false, false, false));

            Assert.That(p.Finished(), Is.False);
            Assert.That(() => p.GetReturnValue(), Throws.TypeOf<NoValueException>());

            p.HandleKey(new ConsoleKeyInfo('\b', ConsoleKey.Backspace, false, false, false));
            p.HandleKey(new ConsoleKeyInfo('\b', ConsoleKey.Backspace, false, false, false));
            p.HandleKey(new ConsoleKeyInfo('\b', ConsoleKey.Backspace, false, false, false));
            p.HandleKey(new ConsoleKeyInfo('\b', ConsoleKey.Backspace, false, false, false));

            Assert.That(p.Finished(), Is.False);
            Assert.That(() => p.GetReturnValue(), Throws.TypeOf<NoValueException>());

            p.HandleKey(new ConsoleKeyInfo('4', ConsoleKey.D4, false, false, false));
            p.HandleKey(new ConsoleKeyInfo('5', ConsoleKey.D5, false, false, false));

            Assert.That(p.Finished(), Is.False);
            Assert.That(() => p.GetReturnValue(), Throws.TypeOf<NoValueException>());

            p.HandleKey(new ConsoleKeyInfo('\b', ConsoleKey.Backspace, false, false, false));
            p.HandleKey(new ConsoleKeyInfo('6', ConsoleKey.D6, false, false, false));
            p.HandleKey(new ConsoleKeyInfo('\n', ConsoleKey.Enter, false, false, false));

            Assert.That(p.Finished(), Is.True);
            Assert.That(() => p.HandleKey(new ConsoleKeyInfo()), Throws.TypeOf<FinishedException>());
            Assert.That(p.GetReturnValue(), Is.EqualTo(46));
        }
    }
}