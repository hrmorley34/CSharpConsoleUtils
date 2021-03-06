using System;

namespace ConsoleUtils.ConsoleImagery
{
    /// <summary>A foreground and a background <c>ConsoleColor</c></summary>
    public record ConsoleColorPair : IEquatable<ConsoleColorPair>
    {
        /// <summary>Background colour</summary>
        public ConsoleColor? Bg { get; init; } = null;
        /// <summary>Foreground colour</summary>
        public ConsoleColor? Fg { get; init; } = null;

        /// <summary>New pair with a foreground colour</summary>
        public ConsoleColorPair(ConsoleColor? fg)
        {
            Fg = fg;
        }
        /// <summary>New pair with a foreground and a background colour</summary>
        public ConsoleColorPair(ConsoleColor? fg, ConsoleColor? bg)
        {
            Fg = fg;
            Bg = bg;
        }

        /// <summary>Return a new pair using <c>fg</c> as the foreground colour</summary>
        public ConsoleColorPair WithFg(ConsoleColor? fg)
            => this with { Fg = fg };
        /// <summary>Return a new pair using the foreground colour of <c>col</c> as the foreground colour</summary>
        public ConsoleColorPair WithFg(ConsoleColorPair? col)
            => col == null ? this : this with { Fg = col.Fg };
        /// <summary>Return a new pair using <c>bg</c> as the background colour</summary>
        public ConsoleColorPair WithBg(ConsoleColor? bg)
            => this with { Bg = bg };
        /// <summary>Return a new pair using the background colour of <c>col</c> as the background colour</summary>
        public ConsoleColorPair WithBg(ConsoleColorPair? col)
            => col == null ? this : this with { Bg = col.Bg };

        /// <summary>Overlay colours from <c>col</c>, but keep <c>null</c>s in <c>col</c> as on this</summary>
        public ConsoleColorPair Overlay(ConsoleColorPair? col) => col?.Render(this) ?? this;

        /// <summary>Return this, but with <c>null</c>s replaced by values from <c>default</c></summary>
        public ConsoleColorPair Render(ConsoleColorPair? @default)
            => @default == null ? this : new ConsoleColorPair(Fg ?? @default.Fg, Bg ?? @default.Bg);

        /// <summary>Return this, but with <c>null</c>s replaced by <c>fg</c> or <c>bg</c></summary>
        public (ConsoleColor, ConsoleColor) Render(ConsoleColor fg, ConsoleColor bg)
            => (Fg ?? fg, Bg ?? bg);

        public void Deconstruct(out ConsoleColor? fg, out ConsoleColor? bg)
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
        public ColorStore(ConsoleColor? fg = null, ConsoleColor? bg = null)
        {
            Store();
            Set(fg, bg);
        }

        /// <summary>Set colour within a <c>using</c> clause</summary>
        public ColorStore(ConsoleColorPair? color)
        {
            Store();
            if (color != null)
                Set(color.Fg, color.Bg);
        }

        /// <summary>Store the current console colour as the reset colour</summary>
        public void Store()
        {
            OldFg = Console.ForegroundColor;
            OldBg = Console.BackgroundColor;
        }

        /// <summary>Set the current console colours, or use the stored colours</summary>
        public void Set(ConsoleColorPair? color)
        {
            if (color == null)
                Reset();
            else
                Set(color.Fg, color.Bg);
        }

        /// <summary>Set the current console colours, or use the stored colours</summary>
        public void Set(ConsoleColor? fg, ConsoleColor? bg)
        {
            Console.ForegroundColor = fg ?? OldFg;
            Console.BackgroundColor = bg ?? OldBg;
        }

        /// <summary>Set the current console colours, or keep the previous colour</summary>
        public void SetSome(ConsoleColor? fg, ConsoleColor? bg)
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