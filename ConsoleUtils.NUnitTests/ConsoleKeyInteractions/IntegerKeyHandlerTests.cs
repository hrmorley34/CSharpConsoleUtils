using NUnit.Framework;
using System;
using System.Linq;
using ConsoleUtils.ConsoleKeyInteractions;

namespace ConsoleUtils.NUnitTests
{
    public class SilentPositiveIntegerKeyHandler : PositiveIntegerKeyHandler
    {
        public override void Print(byte digit) { }
        public override void PrintBackspace() { }
        public override void PrintNewline() { }
    }

    public class ConsoleKeyInteractions_IntegerKeyHandlerTests
    {
        public static readonly (ConsoleKeyInfo, int?)[] PositiveIntegerKeyHandler_TestKeys_KeySequence = {
            (new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false), null),
            (new ConsoleKeyInfo('1', ConsoleKey.D1, false, false, false), 1),
            (new ConsoleKeyInfo('2', ConsoleKey.D2, false, false, false), 12),
            (new ConsoleKeyInfo('3', ConsoleKey.D3, false, false, false), 123),
            (new ConsoleKeyInfo('\b', ConsoleKey.Backspace, false, false, false), 12),
            (new ConsoleKeyInfo('\b', ConsoleKey.Backspace, false, false, false), 1),
            (new ConsoleKeyInfo('\b', ConsoleKey.Backspace, false, false, false), null),
            (new ConsoleKeyInfo('\b', ConsoleKey.Backspace, false, false, false), null),
            (new ConsoleKeyInfo('4', ConsoleKey.D4, false, false, false), 4),
            (new ConsoleKeyInfo('5', ConsoleKey.D5, false, false, false), 45),
            (new ConsoleKeyInfo('\b', ConsoleKey.Backspace, false, false, false), 4),
            (new ConsoleKeyInfo('6', ConsoleKey.D6, false, false, false), 46),
        };
        public static readonly ConsoleKeyInfo PositiveIntegerKeyHandler_TestKeys_Submit = new ConsoleKeyInfo('\n', ConsoleKey.Enter, false, false, false);

        [Test]
        public void PositiveIntegerKeyHandler_TestBoth()
        {
            ConsoleKeyInteractions_Utils.KeyHandler_TestBothSequence<int, int?>(
                () => new SilentPositiveIntegerKeyHandler(),
                PositiveIntegerKeyHandler_TestKeys_KeySequence,
                PositiveIntegerKeyHandler_TestKeys_Submit);
            ConsoleKeyInteractions_Utils.KeyHandler_TestBothSequence<long, long?>(
               () => new SilentPositiveIntegerKeyHandler(),
               PositiveIntegerKeyHandler_TestKeys_KeySequence.Select((pair) => (pair.Item1, (long?)pair.Item2)),
               PositiveIntegerKeyHandler_TestKeys_Submit);
        }
    }
}