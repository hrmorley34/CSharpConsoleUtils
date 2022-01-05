using NUnit.Framework;
using System;
using ConsoleUtils.ConsoleImagery;

namespace ConsoleUtils.NUnitTests
{
    public class ConsoleImagery_ColorTests
    {
        [Test]
        public void ConsoleColorPair_Equals()
        {
            ConsoleColorPair a = new ConsoleColorPair(ConsoleColor.Red);
            ConsoleColorPair b = new ConsoleColorPair(ConsoleColor.Green);
            ConsoleColorPair c = new ConsoleColorPair(ConsoleColor.Blue);
            ConsoleColorPair d = new ConsoleColorPair(ConsoleColor.Red, ConsoleColor.White);
            ConsoleColorPair e = new ConsoleColorPair(ConsoleColor.Red);

            ConsoleImagery_Utils.Generic_NullEquality(a);

            Assert.That(a, Is.Not.EqualTo(b));
            Assert.That(b, Is.Not.EqualTo(c));
            Assert.That(c, Is.Not.EqualTo(a));
            Assert.That(a, Is.Not.EqualTo(d));
            Assert.That(a, Is.EqualTo(e));
        }

        [Test]
        public void ConsoleColorPair_With()
        {
            ConsoleColorPair a = new ConsoleColorPair(ConsoleColor.Red);
            ConsoleColorPair b = new ConsoleColorPair(ConsoleColor.Green);
            ConsoleColorPair c = new ConsoleColorPair(ConsoleColor.Blue);
            ConsoleColorPair d = new ConsoleColorPair(ConsoleColor.Red, ConsoleColor.White);

            Assert.That(a.WithBg(ConsoleColor.White), Is.EqualTo(d));
            Assert.That(a with { Bg = ConsoleColor.White }, Is.EqualTo(d));
            Assert.That(a.WithBg(d), Is.EqualTo(d));

            Assert.That(b.WithBg(ConsoleColor.White).WithFg(ConsoleColor.Red), Is.EqualTo(d));
            Assert.That(b with { Bg = ConsoleColor.White, Fg = ConsoleColor.Red }, Is.EqualTo(d));
            Assert.That(b.WithBg(d).WithFg(d), Is.EqualTo(d));
            Assert.That(b.WithFg(d).WithBg(ConsoleColor.White), Is.EqualTo(d));

            Assert.That(b.WithBg(ConsoleColor.Black).WithFg(ConsoleColor.Red), Is.EqualTo(d.WithBg(ConsoleColor.Black)));
            Assert.That(b.WithFg(ConsoleColor.Red).WithBg(ConsoleColor.Black), Is.EqualTo(d.WithBg(ConsoleColor.Black)));
            Assert.That(b with { Bg = ConsoleColor.Black, Fg = ConsoleColor.Red }, Is.EqualTo(d with { Bg = ConsoleColor.Black }));
            Assert.That(b.WithBg(ConsoleColor.Black).WithFg(d), Is.EqualTo(d.WithBg(ConsoleColor.Black)));

            Assert.That(d.WithBg((Nullable<ConsoleColor>)null), Is.EqualTo(a));
            Assert.That(d with { Bg = null }, Is.EqualTo(a));
            Assert.That(d.WithBg(a), Is.EqualTo(a));
        }

        [Test]
        public void ConsoleColorPair_Render()
        {
            ConsoleColorPair a = new ConsoleColorPair(ConsoleColor.Red, null);

            ConsoleColor bFg = ConsoleColor.Green;
            ConsoleColor bBg = ConsoleColor.Blue;
            ConsoleColorPair b = new ConsoleColorPair(bFg, bBg);
            ConsoleColorPair ab = new ConsoleColorPair(ConsoleColor.Red, bBg);

            ConsoleColorPair c = new ConsoleColorPair(null, ConsoleColor.Magenta);
            ConsoleColorPair ac = new ConsoleColorPair(ConsoleColor.Red, ConsoleColor.Magenta);
            ConsoleColorPair cb = new ConsoleColorPair(bFg, ConsoleColor.Magenta);

            ConsoleColorPair d = new ConsoleColorPair(null, null);

            Assert.That(a.Render(b), Is.EqualTo(ab));
            Assert.That(b.Overlay(a), Is.EqualTo(ab));
            Assert.That(b.Render(a), Is.EqualTo(b));
            Assert.That(a.Overlay(b), Is.EqualTo(b));

            Assert.That(a.Render(bFg, bBg), Is.EqualTo((ConsoleColor.Red, bBg)));

            Assert.That(a.Render(c), Is.EqualTo(ac));
            Assert.That(c.Overlay(a), Is.EqualTo(ac));
            Assert.That(c.Render(a), Is.EqualTo(ac));
            Assert.That(a.Overlay(c), Is.EqualTo(ac));

            Assert.That(c.Render(b), Is.EqualTo(cb));
            Assert.That(b.Overlay(c), Is.EqualTo(cb));
            Assert.That(b.Render(c), Is.EqualTo(b));
            Assert.That(c.Overlay(b), Is.EqualTo(b));

            foreach (var pair in new ConsoleColorPair[] { a, b, ab, c, ac, cb, d })
            {
                Assert.That(d.Render(pair), Is.EqualTo(pair));
                Assert.That(pair.Overlay(d), Is.EqualTo(pair));
                Assert.That(pair.Render(d), Is.EqualTo(pair));
                Assert.That(d.Overlay(pair), Is.EqualTo(pair));
            }
        }

        [Test]
        public void ConsoleColorPair_Deconstruct()
        {
            ConsoleColor fg = ConsoleColor.Green;
            ConsoleColor bg = ConsoleColor.Blue;

            (var newFg, var newBg) = new ConsoleColorPair(fg, bg);

            Assert.That(newFg, Is.EqualTo(fg));
            Assert.That(newBg, Is.EqualTo(bg));
        }

    }
}