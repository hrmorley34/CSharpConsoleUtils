using System;
using ConsoleUtils.ConsoleImagery.Util;

namespace ConsoleUtils.ConsoleImagery
{
    /// <summary>A foreground and a background <c>ConsoleColor</c></summary>
    public class ConsoleColorPair : IEquatable<ConsoleColorPair>
    {
        /// <summary>Background colour</summary>
        public readonly Nullable<ConsoleColor> Bg = null;
        /// <summary>Foreground colour</summary>
        public readonly Nullable<ConsoleColor> Fg = null;

        /// <summary>New pair with a foreground colour</summary>
        public ConsoleColorPair(Nullable<ConsoleColor> fg)
        {
            Fg = fg;
        }
        /// <summary>New pair with a foreground and a background colour</summary>
        public ConsoleColorPair(Nullable<ConsoleColor> fg, Nullable<ConsoleColor> bg)
        {
            Fg = fg;
            Bg = bg;
        }

        /// <summary>Return a new pair using <c>fg</c> as the foreground colour</summary>
        public ConsoleColorPair WithFg(Nullable<ConsoleColor> fg)
            => new ConsoleColorPair(fg, Bg);
        /// <summary>Return a new pair using the foreground colour of <c>col</c> as the foreground colour</summary>
        public ConsoleColorPair WithFg(ConsoleColorPair col)
            => new ConsoleColorPair(col.Fg, Bg);
        /// <summary>Return a new pair using <c>bg</c> as the background colour</summary>
        public ConsoleColorPair WithBg(Nullable<ConsoleColor> bg)
            => new ConsoleColorPair(Fg, bg);
        /// <summary>Return a new pair using the background colour of <c>col</c> as the background colour</summary>
        public ConsoleColorPair WithBg(ConsoleColorPair col)
            => new ConsoleColorPair(Fg, col.Bg);

        /// <summary>Overlay colours from <c>col</c>, but keep <c>null</c>s in <c>col</c> as on this</summary>
        public ConsoleColorPair Overlay(ConsoleColorPair col) => col.Render(this);

        /// <summary>Return this, but with <c>null</c>s replaced by values from <c>default</c></summary>
        public ConsoleColorPair Render(ConsoleColorPair @default)
            => new ConsoleColorPair(!Fg.HasValue ? @default.Fg : Fg.Value, !Bg.HasValue ? @default.Bg : Bg.Value);

        /// <summary>Return this, but with <c>null</c>s replaced by <c>fg</c> or <c>bg</c></summary>
        public (ConsoleColor, ConsoleColor) Render(ConsoleColor fg, ConsoleColor bg)
            => (!Fg.HasValue ? fg : Fg.Value, !Bg.HasValue ? bg : Bg.Value);

        public bool Equals(ConsoleColorPair obj) => !ReferenceEquals(obj, null) && Fg == obj.Fg && Bg == obj.Bg;
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null) || GetType() != obj.GetType()) return false;
            return Equals((ConsoleColorPair)obj);
        }
        public override int GetHashCode() => (!Fg.HasValue ? 0 : ((int)Fg << 5) + (1 << 4)) + (!Bg.HasValue ? 0 : ((int)Bg << 1) + 1);
        public static bool operator ==(ConsoleColorPair a, ConsoleColorPair b) => EqualsUtils.EqualsCheckNull(a, b);
        public static bool operator !=(ConsoleColorPair a, ConsoleColorPair b) => !EqualsUtils.EqualsCheckNull(a, b);

        public void Deconstruct(out Nullable<ConsoleColor> fg, out Nullable<ConsoleColor> bg)
        {
            fg = Fg;
            bg = Bg;
        }

        /// <summary>Empty <c>ConsoleColorPair</c>, representing a reset to the default colours</summary>
        public static readonly ConsoleColorPair Reset = new ConsoleColorPair(null, null);
    }

    /// <summary>Set console colour within a <c>using</c> clause</summary>
    public class ColorStore : IDisposable
    {
        protected ConsoleColor OldFg;
        protected ConsoleColor OldBg;

        /// <summary>Reset console colour at end of a <c>using</c></summary>
        public ColorStore()
        {
            Store();
        }

        /// <summary>Set console colour within a <c>using</c> clause; <c>null</c> represents no change in colour</summary>
        public ColorStore(Nullable<ConsoleColor> fg = null, Nullable<ConsoleColor> bg = null)
        {
            Store();
            Set(fg, bg);
        }

        /// <summary>Set colour within a <c>using</c> clause</summary>
        public ColorStore(ConsoleColorPair color)
        {
            Store();
            Set(color.Fg, color.Bg);
        }

        /// <summary>Store the current console colour as the reset colour</summary>
        public void Store()
        {
            OldFg = Console.ForegroundColor;
            OldBg = Console.BackgroundColor;
        }

        /// <summary>Set the current console colours, or use the stored colours</summary>
        public void Set(ConsoleColorPair color) => Set(color.Fg, color.Bg);

        /// <summary>Set the current console colours, or use the stored colours</summary>
        public void Set(Nullable<ConsoleColor> fg, Nullable<ConsoleColor> bg)
        {
            if (fg.HasValue) Console.ForegroundColor = fg.Value;
            else Console.ForegroundColor = OldFg;
            if (bg.HasValue) Console.BackgroundColor = bg.Value;
            else Console.BackgroundColor = OldBg;
        }

        /// <summary>Set the current console colours, or keep the previous colour</summary>
        public void SetSome(Nullable<ConsoleColor> fg, Nullable<ConsoleColor> bg)
        {
            if (fg.HasValue) Console.ForegroundColor = fg.Value;
            if (bg.HasValue) Console.BackgroundColor = bg.Value;
        }


        /// <summary>Reset the console colours back to the stored colours</summary>
        public void Reset()
        {
            Console.ForegroundColor = OldFg;
            Console.BackgroundColor = OldBg;
        }

        /// <summary>Reset the console colours at the end of a <c>using</c> clause</summary>
        public void Dispose() => Reset();
    }
}