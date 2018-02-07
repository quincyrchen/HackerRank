using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solve_Me_First;

namespace Solve_Me_First_Test_Cases
{
    [TestClass]
    public class UnitTests
    {
        [TestMethod]
        public void simpleAddition()
        {
            int a = 1;
            int b = 1;
            int expected = 2;
            // 1+1 = 2; This should be relatively self-evident.

            int actual = Solution.solveMeFirst(a, b);

            Assert.AreEqual(expected, actual, 0.001, "Test case Failed");
        }
    }
}
