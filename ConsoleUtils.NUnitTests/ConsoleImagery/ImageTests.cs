using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleUtils.ConsoleImagery;
using ConsoleUtils.ConsoleImagery.Util.Linq;

namespace ConsoleUtils.NUnitTests
{
    public class ConsoleImagery_ImageTests
    {
        [Test]
        public void ColoredChar_Equals()
        {
            ColoredChar blank = new ColoredChar();
            ColoredChar blank2 = new ColoredChar();
            ColoredChar blank3 = new ColoredChar(null, null);
            ColoredChar a = new ColoredChar(null, 'a');
            ConsoleColorPair color = new ConsoleColorPair(ConsoleColor.Red);
            ColoredChar b = new ColoredChar(color, 'a');
            ColoredChar c = new ColoredChar(null, 'c');

            ConsoleImagery_Utils.Generic_NullEquality(blank);
            Assert.That(blank, Is.EqualTo(blank2));
            Assert.That(blank, Is.EqualTo(blank3));
            Assert.That(blank2, Is.EqualTo(blank3));
            Assert.That(blank.WithChar('a'), Is.EqualTo(a));
            Assert.That(a.WithColor(color), Is.EqualTo(b));
            Assert.That(b.WithColor(null), Is.EqualTo(a));
            Assert.That(b.WithColor(null).WithChar('c'), Is.EqualTo(c));
            Assert.That(a.WithColor(null), Is.EqualTo(a));
            Assert.That(a.WithChar(null), Is.EqualTo(blank));
        }

        [Test]
        public void ColoredChar_Deconstruct()
        {
            ConsoleColorPair color = new ConsoleColorPair(ConsoleColor.Black, ConsoleColor.White);
            char character = 'Z';

            (var newColor, var newCharacter) = new ColoredChar(color, character);

            Assert.That(newColor, Is.EqualTo(color));
            Assert.That(newCharacter, Is.EqualTo(character));
        }

        [Test]
        public void ColoredTextImage_Equals()
        {
            ColoredTextImage original = new ColoredTextImage(new string[] { "abc", "def" }, new ConsoleColorPair(ConsoleColor.Green));
            ColoredTextImage copy = original.Copy();
            ColoredTextImage copy2 = new ColoredTextImage(new string[] { "abc", "def" }, new ConsoleColorPair(ConsoleColor.Green));
            ColoredTextImage different = new ColoredTextImage(new string[] { "abc", "def", "ghi" }, new ConsoleColorPair(ConsoleColor.Green));

            ConsoleImagery_Utils.Generic_NullEquality(original);

            Assert.That(original, Is.EqualTo(copy));
            Assert.That(original, Is.EqualTo(copy2));
            Assert.That(copy, Is.EqualTo(copy2));
            Assert.That(original, Is.Not.EqualTo(different));
        }

        [Test]
        public void ColoredTextImage_IterWrappedRows()
        {
            ConsoleColorPair color = ConsoleColorPair.Reset;

            ColoredTextImage a = ColoredTextImage.Text("123456789", color);
            IEnumerable<IEnumerable<ColoredChar>> a_4 = new string[] { "1234", "5678", "9" }.Select(r => r.Select(c => new ColoredChar(color, c)));

            Assert.That(a.IterWrappedRows(4), Is.EqualTo(a_4));
            Assert.That(a.IterWrappedRows(4).EnumerableEquals(a_4));

            ColoredTextImage b = new ColoredTextImage(new string[] { "123456789", "abcdefghi" }, color);
            IEnumerable<IEnumerable<ColoredChar>> b_3 = new string[] { "123", "abc", "456", "def", "789", "ghi" }.Select(r => r.Select(c => new ColoredChar(color, c)));

            Assert.That(b.IterWrappedRows(3), Is.EqualTo(b_3));
            Assert.That(b.IterWrappedRows(3).EnumerableEquals(b_3));
        }

        [Test]
        public void ColoredTextImage_Overlay()
        {
            ConsoleColorPair originalcolor = new ConsoleColorPair(ConsoleColor.Green);
            ConsoleColorPair overlaycolor = new ConsoleColorPair(ConsoleColor.Blue);
            ColoredTextImage original = new ColoredTextImage(new string[] { "abc", "def" }, originalcolor);
            ColoredTextImage original2 = original.Copy();
            ColoredTextImage overlay = new ColoredTextImage(new string[] { "hello" }, overlaycolor);
            ColoredTextImage final1 = new ColoredTextImage(new ColoredChar[][] {
                new ColoredChar[] { new ColoredChar(originalcolor, 'a'), new ColoredChar(originalcolor, 'b'), new ColoredChar(originalcolor, 'c') },
                new ColoredChar[] { new ColoredChar(originalcolor, 'd'), new ColoredChar(overlaycolor, 'h'), new ColoredChar(overlaycolor, 'e') },
            });
            ColoredTextImage final2 = new ColoredTextImage(new ColoredChar[][] {
                new ColoredChar[] { new ColoredChar(overlaycolor, 'l'), new ColoredChar(overlaycolor, 'o'), new ColoredChar(originalcolor, 'c') },
                new ColoredChar[] { new ColoredChar(originalcolor, 'd'), new ColoredChar(overlaycolor, 'h'), new ColoredChar(overlaycolor, 'e') },
            });

            Assert.That(original, Is.EqualTo(original2));

            ColoredTextImage overlayed1 = original.Overlay(overlay, (1, 1));
            Assert.That(overlayed1, Is.EqualTo(final1));
            Assert.That(original, Is.EqualTo(original2));
            ColoredTextImage overlayed2 = overlayed1.Overlay(overlay, (-3, 0));
            Assert.That(overlayed2, Is.EqualTo(final2));
            ColoredTextImage overlayed3 = overlayed2.Overlay(overlay, (-3, 0));
            Assert.That(overlayed3, Is.EqualTo(final2));
            ColoredTextImage overlayed4 = overlayed3.Overlay(overlay, (-3, 0));
            Assert.That(overlayed4, Is.EqualTo(final2));
        }

        [Test]
        public void ColoredTextImage_OverlayOnColor()
        {
            string[] lines = new string[] { "abc", "def" };

            ConsoleColorPair colorA = new ConsoleColorPair(ConsoleColor.Green);
            ConsoleColorPair colorB = new ConsoleColorPair(ConsoleColor.Blue);
            ColoredTextImage blank = new ColoredTextImage(lines, ConsoleColorPair.Reset);
            ColoredTextImage imageA = new ColoredTextImage(lines, colorA);
            ColoredTextImage imageB = new ColoredTextImage(lines, colorB);

            Assert.That(blank.OverlayOnColor(colorA), Is.EqualTo(imageA));
            Assert.That(blank.OverlayOnColor(colorB), Is.EqualTo(imageB));
            Assert.That(imageA.OverlayOnColor(colorB), Is.EqualTo(imageA));
            Assert.That(imageB.OverlayOnColor(colorA), Is.EqualTo(imageB));
        }

        [Test]
        public void ColoredTextImage_HorizontalAppend()
        {
            ConsoleColorPair firstcolor = new ConsoleColorPair(ConsoleColor.Green);
            ConsoleColorPair secondcolor = new ConsoleColorPair(ConsoleColor.Blue);
            ColoredTextImage first = new ColoredTextImage(new string[] { "abc", "def" }, firstcolor);
            ColoredTextImage second = new ColoredTextImage(new string[] { "hel", "lo!" }, secondcolor);
            ColoredTextImage final1 = new ColoredTextImage(new ColoredChar[][] {
                new ColoredChar[] { new ColoredChar(firstcolor, 'a'), new ColoredChar(firstcolor, 'b'), new ColoredChar(firstcolor, 'c'), new ColoredChar(secondcolor, 'h'), new ColoredChar(secondcolor, 'e'), new ColoredChar(secondcolor, 'l') },
                new ColoredChar[] { new ColoredChar(firstcolor, 'd'), new ColoredChar(firstcolor, 'e'), new ColoredChar(firstcolor, 'f'), new ColoredChar(secondcolor, 'l'), new ColoredChar(secondcolor, 'o'), new ColoredChar(secondcolor, '!') },
            });

            ColoredTextImage third = ColoredTextImage.Text("abcdef");
            ColoredTextImage fourth = ColoredTextImage.Text("hello!");
            string fifth = "test";
            ColoredTextImage final2 = ColoredTextImage.Text("abcdefhello!");
            ColoredTextImage final3 = ColoredTextImage.Text("hello!test");
            ColoredTextImage final4 = ColoredTextImage.Text("testabcdef");

            Assert.That(first.HorizontalAppend(second), Is.EqualTo(final1));
            Assert.That(first + second, Is.EqualTo(final1));
            Assert.That(() => first.HorizontalAppend(third), Throws.TypeOf<MismatchedHeightException>());
            Assert.That(third.HorizontalAppend(fourth), Is.EqualTo(final2));
            Assert.That(third + fourth, Is.EqualTo(final2));
            Assert.That(() => second + fifth, Throws.TypeOf<MismatchedHeightException>());
            Assert.That(fourth + fifth, Is.EqualTo(final3));
            Assert.That(fifth + third, Is.EqualTo(final4));

            Assert.That(ColoredTextImage.HorizontalAppend(new ColoredTextImage[] { first }), Is.EqualTo(first));
            Assert.That(
                ColoredTextImage.HorizontalAppend(new ColoredTextImage[] { third, fourth, final2, final3, final4 }),
                Is.EqualTo(third + fourth + final2 + final3 + final4));
            Assert.That(() => ColoredTextImage.HorizontalAppend(new ColoredTextImage[] { }), Throws.TypeOf<ArgumentException>());
        }
    }
}