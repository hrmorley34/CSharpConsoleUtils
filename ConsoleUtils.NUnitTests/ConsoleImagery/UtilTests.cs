using NUnit.Framework;
using System;
using System.Collections.Generic;
using ConsoleUtils.ConsoleImagery.Util.Linq;

namespace ConsoleUtils.NUnitTests
{
    public class ConsoleImagery_UtilTests
    {
        [Test]
        public void Util_Dejagged()
        {
            int[][] jagged1 = {
                new int[] { 1, 2, 3 },
                new int[] { 4, 5, 6 },
                new int[] { 7, 8, 9 },
            };
            IEnumerable<IEnumerable<int>> jaggedEnumerable1 = jagged1;
            int[,] twod1 = {
                { 1, 4, 7 },
                { 2, 5, 8 },
                { 3, 6, 9 },
            };

            Assert.That(jagged1.Dejagged(), Is.EqualTo(twod1));
            Assert.That(jaggedEnumerable1.Dejagged(), Is.EqualTo(twod1));

            int[][] jagged2 = {
                new int[] { 1, 2, 3, 4 },
                new int[] { 5, 6, 7, 8 },
            };
            IEnumerable<IEnumerable<int>> jaggedEnumerable2 = jagged2;
            int[,] twod2_plus1 = {
                { 2, 6 },
                { 3, 7 },
                { 4, 8 },
                { 5, 9 },
            };

            Assert.That(jagged2.Dejagged(i => i + 1), Is.EqualTo(twod2_plus1));
            Assert.That(jaggedEnumerable2.Dejagged(i => i + 1), Is.EqualTo(twod2_plus1));

            int[][] jagged3 = {
                new int[] { 1 },
            };
            IEnumerable<IEnumerable<int>> jaggedEnumerable3 = jagged3;
            string[,] twod3_string = {
                { "1" },
            };

            Assert.That(jagged3.Dejagged(i => i.ToString()), Is.EqualTo(twod3_string));
            Assert.That(jaggedEnumerable3.Dejagged(i => i.ToString()), Is.EqualTo(twod3_string));

            int[][] badJagged1 = {
                new int[] { 1, 2, 3, 4 },
                new int[] { 5, 6, 7, 8, 9 },
            };

            Assert.That(() => badJagged1.Dejagged(), Throws.TypeOf<ArgumentException>());
        }
    }
}