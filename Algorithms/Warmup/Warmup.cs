using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolveMeFirst
{
    public class Solution
    {
        public static int solveMeFirst(int a, int b)
        {
            // Hint: Type return a+b; below  
            //We are just adding two numbers here; I hope you don't have any issues!
            return a + b;
        }
        static void Main(String[] args)
        {
            int val1 = Convert.ToInt32(Console.ReadLine());
            int val2 = Convert.ToInt32(Console.ReadLine());
            int sum = solveMeFirst(val1, val2);
            Console.WriteLine(sum);
        }
    }
}
