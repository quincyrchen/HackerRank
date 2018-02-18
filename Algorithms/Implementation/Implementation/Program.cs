using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Implementation
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}

namespace Kangaroo
{
    class Solution
    {

        static string kangaroo(int _x1, int _v1, int _x2, int _v2)
        {
            //We're going to do a little math here - Note that if we set v1t + x1 = v2t + x2, we can solve for t and find that t = (x2-x1)/(v1-v2).
            //If t is a positive integer, we know that at a real-time jump, the kangaroos will be at the same place in the same time.
            //we convert everything to double to ensure that we can capture the decimals after doing operations
            //If we don't convert to double, we will only get integer answers, and we need to know if "time" is an integer.
            double x1 = _x1;
            double x2 = _x2;
            double v1 = _v1;
            double v2 = _v2;
            //we immediately first check for nonsensical cases. If the time and positions are both the same, they will ALWAYS be at the same place in the same time.
            if (v1 == v2 && x1 == x2)
                return "YES";
            //if the speed is the same, we can't use our formula (divide by zero error) and we know that if the speed is the same but the positions are different, they'll NEVER intersect!
            if (v1 == v2 && x1 != x2)
                return "NO";
            //formula, which we solved in the comments above
            double time = (x2 - x1) / (v1 - v2);
            //So we understand time must be positive (Kangaroos can't jump back in time), but what's with the Math.Abs and the double.Epsilon?
            //Math.Abs(time%1) checks to see if there is a (positive) remainder after dividing by 1 - this tells us if time isn't a whole number. i.e. the kangaroos only meet in the middle of a jump.
            //double.Epsilon is basically the tiniest double ever -- we need this JUST in case we have some funky tiny leftovers from division with a double. Due to how math works in a computer, we could
            //actually end up with a small bit of rounding error where there shouldn't be. This takes care of that, and will help flag a non-whole time only if it is significant, as in our problem.
            if (time > 0 && Math.Abs(time % 1) <= double.Epsilon)
                return "YES";
            else
                return "NO";
        }

        static void Kangaroo(String[] args)
        {
            string[] tokens_x1 = Console.ReadLine().Split(' ');
            int x1 = Convert.ToInt32(tokens_x1[0]);
            int v1 = Convert.ToInt32(tokens_x1[1]);
            int x2 = Convert.ToInt32(tokens_x1[2]);
            int v2 = Convert.ToInt32(tokens_x1[3]);
            string result = kangaroo(x1, v1, x2, v2);
            Console.WriteLine(result);
        }
    }
}