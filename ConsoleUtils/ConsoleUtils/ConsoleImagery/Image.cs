using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleUtils.ConsoleImagery.Color;
using ConsoleUtils.ConsoleImagery.Exceptions;
using ConsoleUtils.ConsoleImagery.Util.Linq;

namespace ConsoleUtils.ConsoleImagery.Image
{
    /// <summary>A character and a colour</summary>
    public class ColoredChar : System.Runtime.CompilerServices.ITuple, IEquatable<ColoredChar>
    {
        /// <summary>Colour of the character</summary>
        public readonly ConsoleColorPair Color;
        /// <summary>Character to print</summary>
        public readonly char Char;

        /// <summary>Create a space character in default colours</summary>
        public ColoredChar()
        {
            Color = ConsoleColorPair.Reset;
            Char = ' ';
        }

        /// <summary>Create a character in a colour</summary>
        public ColoredChar(ConsoleColorPair color, char c)
        {
            Color = color;
            Char = c;
        }

        /// <summary>Create a copy with colour <c>color</c></summary>
        public ColoredChar WithColor(ConsoleColorPair color) => new ColoredChar(color, Char);
        /// <summary>Create a copy with character <c>c</c></summary>
        public ColoredChar WithChar(char c) => new ColoredChar(Color, c);

        /// <summary>Return tuple length</summary>
        public int Length { get => 2; }
        /// <summary>Return tuple objects</summary>
        public object this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return Color;
                    case 1: return Char;
                }
                return null;
            }
        }

        public bool Equals(ColoredChar obj) => !ReferenceEquals(obj, null) && Color == obj.Color && Char == obj.Char;
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return Equals((ColoredChar)obj);
        }
        public override int GetHashCode() => Char.GetHashCode() ^ Color.GetHashCode();
        public static bool operator ==(ColoredChar a, ColoredChar b) => !ReferenceEquals(a, null) && a.Equals(b);
        public static bool operator !=(ColoredChar a, ColoredChar b) => ReferenceEquals(a, null) || !a.Equals(b);

        /// <summary>Deconstruct into colour and character</summary>
        public void Deconstruct(out ConsoleColorPair color, out char c)
        {
            color = Color;
            c = Char;
        }
    }

    /// <summary>An image made up of characters of colors</summary>
    public class ColoredTextImage : IEquatable<ColoredTextImage>
    {
        /// <summary>The 2D image</summary>
        public ColoredChar[,] Image;

        public (int, int) Dimensions { get => (XSize, YSize); }
        public int XSize { get => Image.GetLength(0); }
        public int YSize { get => Image.GetLength(1); }

        /// <summary>Set the image using <c>text</c> as characters and <c>color</c> for every cell</summary>
        protected void FillFromImage(IEnumerable<IEnumerable<char>> text, ConsoleColorPair color)
            => Image = text.Dejagged(c => new ColoredChar(color, c));

        /// <summary>Set the image using <c>text</c> as characters and colours</summary>
        protected void FillFromImage(IEnumerable<IEnumerable<ColoredChar>> text)
            => Image = text.Dejagged();

        /// <summary>Create a new uninitialised image</summary>
        public ColoredTextImage() { }

        /// <summary>Create an image with size <c>x</c> by <c>y</c></summary>
        public ColoredTextImage(int x, int y)
        {
            Image = new ColoredChar[x, y];
        }

        /// <summary>Create an image from the grid of characters</summary>
        public ColoredTextImage(IEnumerable<IEnumerable<char>> text, ConsoleColorPair color)
        {
            FillFromImage(text, color);
        }

        /// <summary>Create an image from the grid of characters</summary>
        public ColoredTextImage(IEnumerable<string> text, ConsoleColorPair color)
        {
            FillFromImage(text.Select(s => s.ToCharArray()), color);
        }

        /// <summary>Create an image from the grid of characters and colours</summary>
        public ColoredTextImage(IEnumerable<IEnumerable<ColoredChar>> text)
        {
            FillFromImage(text);
        }

        /// <summary>Create a <c>ColoredTextImage</c> containing text.</summary>
        public static ColoredTextImage Text(string text)
            => Text(text, ConsoleColorPair.Reset);

        /// <summary>Create a <c>ColoredTextImage</c> containing text.</summary>
        public static ColoredTextImage Text(string text, ConsoleColorPair color)
            => new ColoredTextImage(new string[] { text }, color);

        /// <summary>Create a <c>ColoredTextImage</c> containing text. This just passes through the object.</summary>
        public static ColoredTextImage Text(ColoredTextImage text) => text;

        /// <summary>Create a <c>ColoredTextImage</c> containing text.</summary>
        public static ColoredTextImage Text(ColoredTextImage text, ConsoleColorPair color)
        {
            ColoredTextImage t = text.Copy();
            for (int y = 0; y < text.YSize; y++)
            {
                for (int x = 0; x < text.XSize; x++)
                {
                    t.Image[x, y] = t.Image[x, y].WithColor(t.Image[x, y].Color.Render(color));
                }
            }
            return t;
        }

        /// <summary>Return a copy of the image</summary>
        public ColoredTextImage Copy()
        {
            ColoredTextImage copy = new ColoredTextImage();
            copy.Image = Image;
            return copy;
        }

        /// <summary>Iterate over a particular row of the image.</summary>
        private IEnumerable<ColoredChar> IterRow(int y)
        {
            for (int x = 0; x < XSize; x++)
            {
                yield return Image[x, y];
            }
        }

        /// <summary>Iterate over the rows of the image.</summary>
        public IEnumerable<IEnumerable<ColoredChar>> IterRows()
        {
            for (int y = 0; y < YSize; y++)
            {
                yield return IterRow(y);
            }
        }

        /// <summary>
        /// Iterate over the rows of the image, wrapped to <c>wrap</c> characters.<br />
        /// Wrapped lines will be appended after lines of the image, such that the whole image is wrapped, not each line.
        /// </summary>
        public IEnumerable<IEnumerable<ColoredChar>> IterWrappedRows(int wrap)
        {
            Queue<IEnumerable<ColoredChar>> rowqueue = new Queue<IEnumerable<ColoredChar>>(IterRows());
            while (rowqueue.TryDequeue(out IEnumerable<ColoredChar> row))
            {
                if (row.Count() > wrap)
                {
                    rowqueue.Enqueue(row.Skip(wrap));
                    row = row.Take(wrap);
                }
                yield return row;
            }
        }

        /// <summary>Print the image out using <c>Console.Write</c></summary>
        /// <param name="newline">Whether or not to print a newline after each line; most appropriate for single-line text elements</param>
        public void Print(bool newline = true)
        {
            int width = Console.WindowWidth;
            bool wrapnext = false;
            foreach (IEnumerable<ColoredChar> row in IterWrappedRows(width))
            {
                if (wrapnext) Console.WriteLine();
                wrapnext = row.Count() < width;

                using (ColorStore s = new ColorStore())
                {
                    foreach (ColoredChar c in row)
                    {
                        ColoredChar pc = c;
                        if (c == null) pc = new ColoredChar(ConsoleColorPair.Reset, ' ');
                        s.Set(pc.Color);
                        Console.Write(pc.Char);
                    }
                }
            }
            if (newline && wrapnext) Console.WriteLine();
        }

        /// <summary>Overlay another image over the top of this one, offset by <c>position</c></summary>
        public ColoredTextImage Overlay(ColoredTextImage image, (int, int) position)
        {
            (int xOff, int yOff) = position;
            for (int y = 0; y < image.YSize; y++)
            {
                if (y + yOff < 0) continue;
                if (y + yOff >= YSize) break;
                for (int x = 0; x < image.XSize; x++)
                {
                    if (x + xOff < 0) continue;
                    if (x + xOff >= XSize) break;
                    Image[x + xOff, y + yOff] = image.Image[x, y];
                }
            }
            return this;
        }

        /// <summary>Replace all undefined colours with those from <c>color</c></summary>
        public ColoredTextImage OverlayOnColor(ConsoleColorPair color)
            => new ColoredTextImage(IterRows().Select(row => row.Select(c => c.WithColor(c.Color.Render(color)))));

        /// <summary>Attach two images together. They must be of the same height.</summary>
        public ColoredTextImage HorizontalAppend(ColoredTextImage image, int separation = 0)
            => HorizontalAppend(new ColoredTextImage[] { this, image }, separation);

        /// <summary>Attach two images together. They must be of the same height.</summary>
        public static ColoredTextImage HorizontalAppend(IEnumerable<ColoredTextImage> images, int separation = 0)
        {
            ColoredTextImage[] imagearr = images.ToArray();
            if (imagearr.Length <= 0)
                throw new MismatchedHeightException();
            else if (imagearr.Length == 1)
                return imagearr[0];

            if (!imagearr.All(im => im.YSize == imagearr[0].YSize))
                throw new MismatchedHeightException();

            ColoredTextImage dest = new ColoredTextImage(imagearr.Sum(im => im.XSize + separation) - separation, imagearr[0].YSize);
            imagearr.Aggregate(0,
                (x, im) =>
                {
                    dest.Overlay(im, (x, 0));
                    return x + im.XSize + separation;
                }
            );
            return dest;
        }

        public bool Equals(ColoredTextImage obj) => !ReferenceEquals(obj, null) && IterRows().EnumerableEquals(obj.IterRows());
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;
            return Equals((ColoredTextImage)obj);
        }
        public override int GetHashCode() => Image.GetHashCode();
        public static bool operator ==(ColoredTextImage a, ColoredTextImage b) => !ReferenceEquals(a, null) && a.Equals(b);
        public static bool operator !=(ColoredTextImage a, ColoredTextImage b) => ReferenceEquals(a, null) || !a.Equals(b);

        public static ColoredTextImage operator +(ColoredTextImage a, ColoredTextImage b)
            => a.HorizontalAppend(b);
        public static ColoredTextImage operator +(ColoredTextImage a, string b)
        {
            if (a.YSize == 1)
                return a.HorizontalAppend(Text(b));
            throw new MismatchedHeightException();
        }
        public static ColoredTextImage operator +(string a, ColoredTextImage b)
        {
            if (b.YSize == 1)
                return Text(a).HorizontalAppend(b);
            throw new MismatchedHeightException();
        }
    }
}