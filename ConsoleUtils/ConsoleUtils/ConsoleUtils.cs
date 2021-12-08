using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleUtils
{
    /// <summary>A collection of utilities for console interaction</summary>
    public static class CUtil
    {
        /// <summary>Ask the user for a [Y]es or [N]o response</summary>
        public static bool AskYN()
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            while (key.Key != ConsoleKey.Y && key.Key != ConsoleKey.N)
                key = Console.ReadKey(true);

            bool y = key.Key == ConsoleKey.Y;
            Console.WriteLine(y ? "y" : "n");
            return y;
        }

        /// <summary>Which keys represent base-10 digits</summary>
        static readonly Dictionary<ConsoleKey, int> Digits = new Dictionary<ConsoleKey, int>() {
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

        /// <summary>Ask the user for a positive integer</summary>
        public static int AskInt()
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            List<int> digits = new List<int>();
            while (key.Key != ConsoleKey.Enter || digits.Count <= 0)
            {
                if (Digits.ContainsKey(key.Key))
                {
                    int digit = Digits[key.Key];
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

            return digits.Aggregate((last, next) => last * 10 + next);
        }
    }
}