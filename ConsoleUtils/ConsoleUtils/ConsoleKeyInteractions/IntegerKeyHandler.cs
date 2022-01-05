using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleUtils.ConsoleKeyInteractions
{
    internal static class NumericKeyGroups
    {
        /// <summary>Which keys represent base-10 digits</summary>
        public static readonly Dictionary<ConsoleKey, byte> Digits = new Dictionary<ConsoleKey, byte>() {
            {ConsoleKey.D0, 0}, {ConsoleKey.NumPad0, 0},
            {ConsoleKey.D1, 1}, {ConsoleKey.NumPad1, 1},
            {ConsoleKey.D2, 2}, {ConsoleKey.NumPad2, 2},
            {ConsoleKey.D3, 3}, {ConsoleKey.NumPad3, 3},
            {ConsoleKey.D4, 4}, {ConsoleKey.NumPad4, 4},
            {ConsoleKey.D5, 5}, {ConsoleKey.NumPad5, 5},
            {ConsoleKey.D6, 6}, {ConsoleKey.NumPad6, 6},
            {ConsoleKey.D7, 7}, {ConsoleKey.NumPad7, 7},
            {ConsoleKey.D8, 8}, {ConsoleKey.NumPad8, 8},
            {ConsoleKey.D9, 9}, {ConsoleKey.NumPad9, 9},
        };
    }

    public class PositiveIntegerKeyHandler : IKeyHandler<int>, IKeyHandler<long>
    {
        protected bool HasValue;
        protected List<byte> Digits;

        public PositiveIntegerKeyHandler()
        {
            HasValue = false;
            Digits = new List<byte>();
        }

        /// <summary>Output a digit</summary>
        virtual public void Print(byte digit) => Console.Write(digit);

        /// <summary>Delete the previous digit</summary>
        virtual public void PrintBackspace() => Console.Write("\b \b");

        /// <summary>Print a newline</summary>
        virtual public void PrintNewline() => Console.WriteLine();

        public bool HandleKey(ConsoleKeyInfo keyInfo)
        {
            if (Finished()) throw new FinishedException();

            if (keyInfo.Key == ConsoleKey.Enter)
                HandleNewline();
            else if (NumericKeyGroups.Digits.ContainsKey(keyInfo.Key))
                HandleDigit(NumericKeyGroups.Digits[keyInfo.Key]);
            else if (keyInfo.Key == ConsoleKey.Backspace)
                HandleBackspace();

            return Finished();
        }

        public bool HandleKey(char c)
        {
            if (Finished()) throw new FinishedException();

            if (c == '\n')
                HandleNewline();
            else if (byte.TryParse(c.ToString(), out byte digit))
                HandleDigit(digit);
            else if (c == '\b')
                HandleBackspace();

            return Finished();
        }

        protected void HandleNewline()
        {
            if (Digits.Count > 0)
            {
                HasValue = true;
                PrintNewline();
            }
        }

        protected void HandleDigit(byte digit)
        {
            Digits.Add(digit);
            Print(digit);
        }

        protected void HandleBackspace()
        {
            if (Digits.Count > 0)
            {
                Digits.RemoveAt(Digits.Count - 1);
                PrintBackspace();
            }
        }

        public bool Finished() => HasValue;

        public int GetReturnValue()
        {
            if (!Finished()) throw new NoValueException();

            return Digits.Aggregate(0, (last, next) => last * 10 + next);
        }
        long IKeyHandler<long>.GetReturnValue()
        {
            if (!Finished()) throw new NoValueException();

            return Digits.Aggregate((long)0, (last, next) => last * 10 + next);
        }

        public int ReadKeys() => ReadKeysMethod.ReadKeys<int>(this);
        long IKeyHandler<long>.ReadKeys() => ReadKeysMethod.ReadKeys<long>(this);
    }
}