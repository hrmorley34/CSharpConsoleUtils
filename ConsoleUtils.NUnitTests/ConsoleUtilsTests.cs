using NUnit.Framework;
using System;
using ConsoleUtils.ConsoleImagery.Color;
using ConsoleUtils.ConsoleImagery.Image;

namespace ConsoleUtils.NUnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ColoredTextImage_Equals()
        {
            ColoredTextImage original = new ColoredTextImage(new string[] { "abc", "def" }, new ConsoleColorPair(ConsoleColor.Green));
            ColoredTextImage copy = new ColoredTextImage(new string[] { "abc", "def" }, new ConsoleColorPair(ConsoleColor.Green));

            Assert.That(original, Is.EqualTo(copy));
        }

        [Test]
        public void ColoredTextImage_Overlay()
        {
            ConsoleColorPair originalcolor = new ConsoleColorPair(ConsoleColor.Green);
            ConsoleColorPair overlaycolor = new ConsoleColorPair(ConsoleColor.Blue);
            ColoredTextImage original = new ColoredTextImage(new string[] { "abc", "def" }, originalcolor);
            ColoredTextImage overlay = new ColoredTextImage(new string[] { "hello" }, overlaycolor);
            ColoredTextImage final1 = new ColoredTextImage(new ColoredChar[][] {
                new ColoredChar[] { new ColoredChar(originalcolor, 'a'), new ColoredChar(originalcolor, 'b'), new ColoredChar(originalcolor, 'c') },
                new ColoredChar[] { new ColoredChar(originalcolor, 'd'), new ColoredChar(overlaycolor, 'h'), new ColoredChar(overlaycolor, 'e') },
            });
            ColoredTextImage final2 = new ColoredTextImage(new ColoredChar[][] {
                new ColoredChar[] { new ColoredChar(overlaycolor, 'l'), new ColoredChar(overlaycolor, 'o'), new ColoredChar(originalcolor, 'c') },
                new ColoredChar[] { new ColoredChar(originalcolor, 'd'), new ColoredChar(overlaycolor, 'h'), new ColoredChar(overlaycolor, 'e') },
            });

            Assert.That(original.Overlay(overlay, (1, 1)), Is.EqualTo(final1));
            Assert.That(original.Overlay(overlay, (-3, 0)), Is.EqualTo(final2));
            Assert.That(original.Overlay(overlay, (0, -1)), Is.EqualTo(final2));
            Assert.That(original.Overlay(overlay, (0, 2)), Is.EqualTo(final2));
        }
    }
}