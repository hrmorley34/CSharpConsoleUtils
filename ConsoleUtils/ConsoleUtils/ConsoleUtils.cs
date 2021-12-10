using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleUtils
{
    internal static class KeyGroups
    {
        /// <summary>Which keys represent yes and no</summary>
        public static readonly Dictionary<ConsoleKey, bool> YesNo = new Dictionary<ConsoleKey, bool>() {
            {ConsoleKey.Y, true}, {ConsoleKey.N, false},
        };

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

    /// <summary>A collection of utilities for console interaction</summary>
    public static class CUtil
    {
        /// <summary>Ask the user for a [Y]es or [N]o response</summary>
        [Obsolete("Use AskYesNo instead")]
        public static bool AskYN() => AskYesNo();

        /// <summary>Ask the user for a [Y]es or [N]o response</summary>
        public static bool AskYesNo() => AskYesNo(b => Console.WriteLine(b ? "y" : "n"));

        /// <summary>Ask the user for a [Y]es or [N]o response</summary>
        public static bool AskYesNo(Action<bool> print)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            while (!KeyGroups.YesNo.ContainsKey(key.Key))
                key = Console.ReadKey(true);

            bool y = KeyGroups.YesNo[key.Key];
            print(y);
            return y;
        }

        /// <summary>Ask the user for a positive integer</summary>
        [Obsolete("Use AskPositiveInteger instead")]
        public static int AskInt() => AskPositiveInteger();

        /// <summary>Ask the user for a positive integer</summary>
        public static int AskPositiveInteger()
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            List<byte> digits = new List<byte>();
            while (key.Key != ConsoleKey.Enter || digits.Count <= 0)
            {
                if (KeyGroups.Digits.ContainsKey(key.Key))
                {
                    byte digit = KeyGroups.Digits[key.Key];
                    digits.Add(digit);
                    Console.Write(digit);
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (digits.Count > 0)
                    {
                        Console.Write("\b \b");
                        digits.RemoveAt(digits.Count - 1);
                    }
                }

                key = Console.ReadKey(true);
            }
            Console.WriteLine();

            return digits.Aggregate(0, (last, next) => last * 10 + next);
        }
    }
}