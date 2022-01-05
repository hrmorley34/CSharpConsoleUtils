using NUnit.Framework;
using System;
using ConsoleUtils.ConsoleKeyInteractions;

namespace ConsoleUtils.NUnitTests
{
    public class SilentYesNoKeyHandler : YesNoKeyHandler
    {
        public override void Print(ConsoleKeyInfo keyInfo) { }
        public override void Print(char c) { }
    }

    public class ConsoleKeyInteractions_YesNoKeyHandlerTests
    {
        public static readonly (ConsoleKeyInfo, bool?)[] YesNoKeyHandler_TestKeys_Keys = {
            (new ConsoleKeyInfo('a', ConsoleKey.A, false, false, false), null),
            (new ConsoleKeyInfo('Q', ConsoleKey.Q, true, false, false), null),
            (new ConsoleKeyInfo('Z', ConsoleKey.Z, true, false, false), null),
            (new ConsoleKeyInfo('1', ConsoleKey.D1, false, false, false), null),
            (new ConsoleKeyInfo('\x1b', ConsoleKey.Escape, false, false, false), null),
            (new ConsoleKeyInfo('\b', ConsoleKey.Backspace, false, false, false), null),
            (new ConsoleKeyInfo('\n', ConsoleKey.Enter, false, false, false), null),
            (new ConsoleKeyInfo('y', ConsoleKey.Y, false, false, false), true),
            (new ConsoleKeyInfo('Y', ConsoleKey.Y, true, false, false), true),
            (new ConsoleKeyInfo('n', ConsoleKey.N, false, false, false), false),
            (new ConsoleKeyInfo('N', ConsoleKey.N, true, false, false), false),
        };

        [Test]
        public void YesNoKeyHandler_TestKeys()
        {
            foreach ((ConsoleKeyInfo keyInfo, bool? result) in YesNoKeyHandler_TestKeys_Keys)
            {
                ConsoleKeyInteractions_Utils.KeyHandler_TestKeys(() => new SilentYesNoKeyHandler(), new ConsoleKeyInfo[1] { keyInfo }, result);
            }
        }

        [Test]
        public void YesNoKeyHandler_TestChars()
        {
            foreach ((ConsoleKeyInfo keyInfo, bool? result) in YesNoKeyHandler_TestKeys_Keys)
            {
                ConsoleKeyInteractions_Utils.KeyHandler_TestChars(() => new SilentYesNoKeyHandler(), new char[1] { keyInfo.KeyChar }, result);
            }
        }
    }
}