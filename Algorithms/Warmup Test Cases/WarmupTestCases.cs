using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SolveMeFirst_TestCases
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void solveMeFirst()
        {
            int x = 1;
            int y = 2;
            int expected = 3;
            int actual = SolveMeFirst.Solution.solveMeFirst(x, y);
            Assert.AreEqual(expected, actual, "solveMeFirst Test Case Failed.");
        }
    }
}

namespace SimpleArraySum_TestCases
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void simpleArraySum() {
            int[] ar = { 1, 2, 3, 4, 5 };
            int n = 5;
            int expected = 15;
            int actual = SimpleArraySum.Solution.simpleArraySum(n, ar);
            Assert.AreEqual(expected, actual, "simpleArraySum Test Case Failed.");
        }
    }
}
