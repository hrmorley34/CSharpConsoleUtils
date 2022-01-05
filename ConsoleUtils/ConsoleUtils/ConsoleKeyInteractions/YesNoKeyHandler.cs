using System;
using System.Collections.Generic;

namespace ConsoleUtils.ConsoleKeyInteractions
{
    public class YesNoKeyHandler : IKeyHandler<bool>
    {
        protected bool HasValue;
        protected bool Value;

        /// <summary>Which keys represent yes and no</summary>
        public static readonly Dictionary<ConsoleKey, bool> YesNoKeys = new Dictionary<ConsoleKey, bool>() {
            {ConsoleKey.Y, true}, {ConsoleKey.N, false},
        };
        /// <summary>Which characters represent yes and no</summary>
        public static readonly Dictionary<char, bool> YesNoChars = new Dictionary<char, bool>() {
            {'Y', true}, {'y', true}, {'N', false}, {'n', false},
        };

        /// <summary>Print output for this <c>ConsoleKeyInfo</c></summary>
        virtual public void Print(ConsoleKeyInfo keyInfo)
        {
            Console.WriteLine(YesNoKeys[keyInfo.Key] ? "y" : "n");
        }

        /// <summary>Print output for this <c>char</c></summary>
        virtual public void Print(char c)
        {
            Console.WriteLine(c.ToString().ToLower());
        }

        public bool HandleKey(ConsoleKeyInfo keyInfo)
        {
            if (Finished()) throw new FinishedException();

            if (!YesNoKeys.ContainsKey(keyInfo.Key))
                return false;

            Value = YesNoKeys[keyInfo.Key];
            Print(keyInfo);
            HasValue = true;
            return Finished();
        }

        public bool HandleKey(char c)
        {
            if (Finished()) throw new FinishedException();

            if (!YesNoChars.ContainsKey(c))
                return false;

            Value = YesNoChars[c];
            Print(c);
            HasValue = true;
            return Finished();
        }

        public bool Finished() => HasValue;

        public bool GetReturnValue()
        {
            if (!Finished()) throw new NoValueException();

            return Value;
        }

        public bool ReadKeys() => ReadKeysMethod.ReadKeys(this);
    }
}