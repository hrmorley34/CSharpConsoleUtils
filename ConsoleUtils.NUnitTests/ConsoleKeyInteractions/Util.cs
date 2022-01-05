using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleUtils.ConsoleKeyInteractions;

namespace ConsoleUtils.NUnitTests
{
    static class ConsoleKeyInteractions_Utils
    {
        public static void KeyHandler_TestKeys<T, NT>(Func<IKeyHandler<T>> handler, IEnumerable<ConsoleKeyInfo> keys, NT result)
        {
            IKeyHandler<T> h = handler();

            foreach (ConsoleKeyInfo keyInfo in keys)
            {
                h.HandleKey(keyInfo);
            }

            if (result == null)
            {
                Assert.That(h.Finished(), Is.False);
                Assert.That(() => h.GetReturnValue(), Throws.TypeOf<NoValueException>());
            }
            else
            {
                Assert.That(h.Finished(), Is.True);
                Assert.That(() => h.HandleKey(new ConsoleKeyInfo()), Throws.TypeOf<FinishedException>());
                Assert.That(h.GetReturnValue(), Is.EqualTo(result));
            }
        }

        public static void KeyHandler_TestChars<T, NT>(Func<IKeyHandler<T>> handler, IEnumerable<char> chars, NT result)
        {
            IKeyHandler<T> h = handler();

            foreach (char c in chars)
            {
                h.HandleKey(c);
            }

            if (result == null)
            {
                Assert.That(h.Finished(), Is.False);
                Assert.That(() => h.GetReturnValue(), Throws.TypeOf<NoValueException>());
            }
            else
            {
                Assert.That(h.Finished(), Is.True);
                Assert.That(() => h.HandleKey('\x00'), Throws.TypeOf<FinishedException>());
                Assert.That(h.GetReturnValue(), Is.EqualTo(result));
            }
        }

        public static void KeyHandler_TestBoth<T, NT>(Func<IKeyHandler<T>> handler, IEnumerable<ConsoleKeyInfo> keys, NT result)
        {
            KeyHandler_TestKeys<T, NT>(handler, keys, result);
            KeyHandler_TestChars<T, NT>(handler, keys.Select(keyInfo => keyInfo.KeyChar), result);
        }

        public static void KeyHandler_TestKeySequence<T, NT>(Func<IKeyHandler<T>> handler, IEnumerable<(ConsoleKeyInfo, NT)> values, ConsoleKeyInfo submit)
        {
            for (int i = 0; i < values.Count(); i++)
            {
                KeyHandler_TestKeys<T, NT>(
                    handler,
                    values.Take(i + 1).Select(pair => pair.Item1).Append(submit),
                    values.Skip(i).First().Item2);
            }
        }

        public static void KeyHandler_TestCharSequence<T, NT>(Func<IKeyHandler<T>> handler, IEnumerable<(char, NT)> values, char submit)
        {
            for (int i = 0; i < values.Count(); i++)
            {
                KeyHandler_TestChars<T, NT>(
                    handler,
                    values.Take(i + 1).Select(pair => pair.Item1).Append(submit),
                    values.Skip(i).First().Item2);
            }
        }

        public static void KeyHandler_TestBothSequence<T, NT>(Func<IKeyHandler<T>> handler, IEnumerable<(ConsoleKeyInfo, NT)> values, ConsoleKeyInfo submit)
        {
            KeyHandler_TestKeySequence<T, NT>(handler, values, submit);
            KeyHandler_TestCharSequence<T, NT>(handler, values.Select(pair => (pair.Item1.KeyChar, pair.Item2)), submit.KeyChar);
        }
    }
}