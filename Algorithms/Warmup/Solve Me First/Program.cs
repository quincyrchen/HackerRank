using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solve_Me_First
{
    public class Solution
    {
        public static int solveMeFirst(int a, int b)
        {
            //We're just adding two integers together here. Hopefully you don't have any trouble with this trial question!
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
