using NUnit.Framework;
using System;
using ConsoleUtils.ConsoleImagery;

namespace ConsoleUtils.NUnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ConsoleColorPair_Equals()
        {
            ConsoleColorPair a = new ConsoleColorPair(ConsoleColor.Red);
            ConsoleColorPair b = new ConsoleColorPair(ConsoleColor.Green);
            ConsoleColorPair c = new ConsoleColorPair(ConsoleColor.Blue);
            ConsoleColorPair d = new ConsoleColorPair(ConsoleColor.Red, ConsoleColor.White);

            Assert.That(a, Is.Not.EqualTo(b));
            Assert.That(b, Is.Not.EqualTo(c));
            Assert.That(c, Is.Not.EqualTo(a));
            Assert.That(a, Is.Not.EqualTo(d));
            Assert.That(a.WithBg(ConsoleColor.White), Is.EqualTo(d));
            Assert.That(b.WithBg(ConsoleColor.White).WithFg(ConsoleColor.Red), Is.EqualTo(d));
            Assert.That(b.WithBg(ConsoleColor.Black).WithFg(ConsoleColor.Red), Is.EqualTo(d.WithBg(ConsoleColor.Black)));
            Assert.That(d.WithBg((Nullable<ConsoleColor>)null), Is.EqualTo(a));
        }

        [Test]
        public void ColoredTextImage_Equals()
        {
            ColoredTextImage original = new ColoredTextImage(new string[] { "abc", "def" }, new ConsoleColorPair(ConsoleColor.Green));
            ColoredTextImage copy = original.Copy();
            ColoredTextImage copy2 = new ColoredTextImage(new string[] { "abc", "def" }, new ConsoleColorPair(ConsoleColor.Green));
            ColoredTextImage different = new ColoredTextImage(new string[] { "abc", "def", "ghi" }, new ConsoleColorPair(ConsoleColor.Green));

            Assert.That(original, Is.EqualTo(copy));
            Assert.That(original, Is.EqualTo(copy2));
            Assert.That(copy, Is.EqualTo(copy2));
            Assert.That(original, Is.Not.EqualTo(different));
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

            Assert.That(first.HorizontalAppend(second), Is.EqualTo(final1));
            Assert.That(first + second, Is.EqualTo(final1));
            Assert.That(() => first.HorizontalAppend(third), Throws.TypeOf<MismatchedHeightException>());
            Assert.That(third.HorizontalAppend(fourth), Is.EqualTo(final2));
            Assert.That(third + fourth, Is.EqualTo(final2));
            Assert.That(() => second + "abc", Throws.TypeOf<MismatchedHeightException>());
            Assert.That(fourth + fifth, Is.EqualTo(final3));
        }
    }
}