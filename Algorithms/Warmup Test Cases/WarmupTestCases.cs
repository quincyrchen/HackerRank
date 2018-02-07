using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SolveMeFirst_TestCases
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void AddTwoNumbers()
        {
            int x = 1;
            int y = 2;
            int expected = 3;
            int actual = SolveMeFirst.Solution.solveMeFirst(x, y);

            Assert.AreEqual(expected, actual, "AddTwoNumbers Test Case Failed.");
        }
    }
}
