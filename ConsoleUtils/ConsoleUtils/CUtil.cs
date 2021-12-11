using System;
using ConsoleUtils.ConsoleKeyInteractions;

namespace ConsoleUtils
{
    /// <summary>A collection of utilities for console interaction</summary>
    public static class CUtil
    {
        /// <summary>Ask the user for a [Y]es or [N]o response</summary>
        [Obsolete("Use AskYesNo instead")]
        public static bool AskYN() => AskYesNo();

        /// <summary>Ask the user for a [Y]es or [N]o response</summary>
        public static bool AskYesNo() => ((IKeyHandler<bool>)new YesNoKeyHandler()).ReadKeys();

        /// <summary>Ask the user for a positive integer</summary>
        [Obsolete("Use AskPositiveInteger instead")]
        public static int AskInt() => AskPositiveInteger();

        /// <summary>Ask the user for a positive integer</summary>
        public static int AskPositiveInteger() => ((IKeyHandler<int>)new PositiveIntegerKeyHandler()).ReadKeys();
    }
}